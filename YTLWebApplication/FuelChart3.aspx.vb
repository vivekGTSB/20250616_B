Imports System.Data.SqlClient
Imports ChartDirector
Imports System.Collections.Generic

Partial Class FuelChart3
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            DisplayChartFull()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub DisplayChartFull()
        Try
            Dim userid As String = Request.QueryString("userid")
            Dim username As String = Request.QueryString("username")
            Dim plateno As String = Request.QueryString("plateno")
            Dim bdt As String = Request.QueryString("bdt")
            Dim edt As String = Request.QueryString("edt")
            Dim begindatetime As String = Request.QueryString("day") & " 00:00:00"
            Dim enddatetime As String = Request.QueryString("day") & " 23:59:59"
            Dim tdate As Date = Date.Parse(begindatetime)
            Dim type As String = Request.QueryString("type")

            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))

            'Dim conn As New SqlConnection("Server=127.0.1.1;Database=avlsdev;User ID=sa;Password=sa123;MultipleActiveResultSets=True;")
            'Dim conn As New SqlConnection("Server=192.168.1.223;Database=avlsdev;User ID=sa;Password=baad0987654321;MultipleActiveResultSets=True;")
            Dim cmd As SqlCommand = New SqlCommand("select timestamp as datetime,gps_av,speed,ignition,odometer,level1,volume1,level2,volume2 from vehicle_history2 where plateno ='" & plateno & "' and timestamp between '" & begindatetime & "' and '" & enddatetime & "' and odometer<>-1 order by timestamp", conn)
            Dim dr As SqlDataReader

            Dim dataY1() As Double = {}
            Dim dataY2() As Double = {}
            Dim dataY3() As Double = {}
            Dim dataY4() As Double = {}
            Dim dataX1() As String = {}

            Dim maxvolume As Double = 0
            Dim maxlevel As Double = 0

            Try
                conn.Open()
                dr = cmd.ExecuteReader()

                Dim vehicleRecordList As New List(Of VehicleRecord)

                Dim v1 As Double = 0
                Dim v2 As Double = 0
                Dim l1 As Double = 0
                Dim l2 As Double = 0
                cmd = New SqlCommand("select * from fuel_tank_check where plateno='" & plateno & "'", conn)
                Dim tankdr As SqlDataReader

                tankdr = cmd.ExecuteReader()
                Dim Is2Tank As Boolean = False

                While tankdr.Read()
                    Try
                        If (tankdr("tankno") = "2") Then
                            Is2Tank = True
                            Exit While
                        End If
                    Catch ex As Exception

                    End Try
                End While

                While dr.Read()
                    Try
                        Dim vr As New VehicleRecord()

                        vr.timeStamp = DateTime.Parse(dr("datetime"))
                        vr.seconds = vr.timeStamp.TimeOfDay.TotalMinutes
                        vr.gpsAV = dr("gps_av")
                        vr.speed = dr("speed")
                        vr.odometer = dr("odometer")
                        l1 = dr("level1")
                        l2 = dr("level2")

                        v1 = dr("volume1")
                        v2 = dr("volume2")

                        If Is2Tank Then
                            If v1 > -1 And v2 > -1 Then
                                vr.volumn = v1 + v2
                            ElseIf v1 > -1 And v2 <= -1 Then
                                vr.volumn = v1
                            ElseIf v1 <= -1 And v2 > -1 Then
                                vr.volumn = v2
                            End If
                        Else
                            vr.volumn = v1
                        End If

                        If l1 > -1 And l2 > -1 Then
                            vr.level = l1 + l2
                        ElseIf l1 > -1 And l2 <= -1 Then
                            vr.level = l1
                        ElseIf l1 <= -1 And l2 > -1 Then
                            vr.level = l2
                        End If

                        If (vr.level > -1 And vr.volumn > -1) Then
                            vehicleRecordList.Add(vr)
                        End If

                    Catch ex As Exception

                    End Try

                End While

                Dim baseodometer As Double = vehicleRecordList.Item(0).odometer

                For j As Int32 = 0 To vehicleRecordList.Count - 1
                    Try
                        ReDim Preserve dataX1(j)
                        ReDim Preserve dataY1(j)
                        ReDim Preserve dataY2(j)
                        ReDim Preserve dataY3(j)
                        ReDim Preserve dataY4(j)

                        dataX1(j) = vehicleRecordList.Item(j).timeStamp.ToString("yyyy/MM/dd HH:mm:ss")
                        dataY1(j) = vehicleRecordList.Item(j).level
                        dataY2(j) = vehicleRecordList.Item(j).volumn
                        dataY3(j) = vehicleRecordList.Item(j).odometer
                        dataY4(j) = vehicleRecordList.Item(j).speed

                        If (maxvolume < vehicleRecordList.Item(j).volumn) Then
                            maxvolume = vehicleRecordList.Item(j).volumn
                        End If

                        If (maxlevel < vehicleRecordList.Item(j).level) Then
                            maxlevel = vehicleRecordList.Item(j).level
                        End If

                    Catch ex As Exception

                    End Try
                Next

            Catch ex As Exception

            Finally
                conn.Close()
            End Try

            'Create a XYChart object of size 400 x 250 pixels
            Dim c As XYChart = New XYChart(750, 450)

            c.addTitle(username.ToUpper() & "  -  " & plateno & "  -  " & tdate.ToString("dd MMMM yyyy"), "Arial Bold", 10, &H9900).setBackground(&HFFFFFF)
            c.setPlotArea(130, 25, 500, 300, &HF4FDEF).setGridColor(&HCCCCCC, &HCCCCCC)
            'c.setPlotArea(130, 100, 500, 300, &HF4FDEF).setGridColor(&HCCCCCC, &HCCCCCC)

            Dim legendBox As LegendBox = c.addLegend(370, 20, False, "Arial Bold", 8)
            legendBox.setAlignment(Chart.BottomCenter)
            legendBox.setBackground(Chart.Transparent, Chart.Transparent)

            ''Set the labels on the x axis.
            'c.xAxis().setLabels(dataX1)
            ''c.xAxis.setLabelStep(60)
            ''c.xAxis.setLabelFormat("{={value}/60}")
            'c.xAxis().setTitle("Timestamp (Hours)", "Verdana Bold")
            'c.xAxis().setColors(&HCC, &HCC, &HCC)

            c.xAxis().setLabels(dataX1)
            c.xAxis().setLabels(dataX1).setFontAngle(45)

            If UBound(dataX1) <= 24 Then
                c.xAxis().setLabelStep(3, 1)
            Else
                c.xAxis().setLabelStep(Convert.ToInt32(UBound(dataX1) / 24))
            End If

            
            'Add a title to the x-axis
            c.xAxis().setTitle("Timestamp")

            'Dim layert As StepLineLayer = c.addStepLineLayer(dataY0, &HFF0000, "Volumn")
            'layert.setXData(dataX0)
            'layert.setLineWidth(0)

            'Add a title on top of the primary (left) y axis.
            c.yAxis().setTitle("Odometer (Km)") '.setAlignment(Chart.TopLeft2)
            c.yAxis().setColors(&HCC0000, &HCC0000, &HCC0000)

            'Add a title on top of the secondary (right) y axis.
            c.yAxis2().setTitle("Tank (mm)") '.setAlignment(Chart.TopRight2)
            c.yAxis2().setColors(&H8000, &H8000, &H8000)

            'Add the third y-axis at 50 pixels to the left of the plot area
            Dim leftAxis As Axis = c.addAxis(Chart.Left, 60)
            leftAxis.setTitle("Speed (Km/h)") '.setAlignment(Chart.TopLeft2)
            leftAxis.setColors(&HCC, &HCC, &HCC)

            'Add the fouth y-axis at 50 pixels to the right of the plot area
            Dim rightAxis As Axis = c.addAxis(Chart.Right, 50)
            rightAxis.setTitle("Tank (l)") '.setAlignment(Chart.TopRight2)
            rightAxis.setColors(&H880088, &H880088, &H880088)

            c.yAxis().setLinearScale(CDbl(dataY3(0)) - 100, CDbl(dataY3(dataY3.Length - 1)) + 100)

            If (maxlevel < 700 And maxvolume < 550) Then
                c.yAxis2().setLinearScale(0, 700)
                rightAxis.setLinearScale(0, 550)
            Else
                c.yAxis2().setLinearScale(0, maxlevel + 100)
                rightAxis.setLinearScale(0, maxvolume + 100)
            End If
          
            Dim layer0 As LineLayer = c.addLineLayer(dataY3, &HCC0000, "Odometer (KM)")
            layer0.setLineWidth(2)

            Dim layer1 As LineLayer = c.addLineLayer(dataY1, &H8000, "Tank Level (MM)")
            layer1.setLineWidth(2)
            layer1.setUseYAxis2()

            Dim layer2 As LineLayer = c.addLineLayer(dataY4, &HCC, "Speed (KM/H)")
            layer2.setUseYAxis(leftAxis)

            Dim layer3 As LineLayer = c.addLineLayer(dataY2, &H880088, "Tank Volume (L)")
            layer3.setLineWidth(2)
            layer3.setUseYAxis(rightAxis)

          

            Response.BinaryWrite(c.makeChart2(1))
            Response.ContentType = "image/gif"

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Structure VehicleRecord
        Dim timeStamp As DateTime
        Dim seconds As Double
        Dim gpsAV As Char
        Dim lat As Double
        Dim lon As Double
        Dim speed As Double
        Dim odometer As Double
        Dim ignition As Boolean
        Dim level As Double
        Dim volumn As Double
    End Structure
End Class
