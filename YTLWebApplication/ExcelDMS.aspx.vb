Imports System.Data.SqlClient
Imports ExcelLibrary
Imports ExcelLibrary.SpreadSheet
Imports System.IO
Partial Class ExcelDMS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try



            Dim wb As New Workbook()
            Dim sheetrowcounter As Integer
            Dim sheetrowcounter2 As Integer = 0
            sheetrowcounter = 0
            Dim tplateno As String = Request.QueryString("tplateno")
            Dim tbdt As String = Request.QueryString("tbdt")
            Dim tedt As String = Request.QueryString("tedt")
            Dim sheet As New ExcelLibrary.SpreadSheet.Worksheet("DMS (PORJECT GRANDE)")




            sheet.Cells(sheetrowcounter, 0) = New Cell("DELIVERY MONITORING SUMMARY(PROJECT GRANDE) ")
            sheet.Cells(sheetrowcounter, 2) = New Cell("")

            sheet.Cells(sheetrowcounter, 3) = New Cell("Report Date: " & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & "                                                                                                                   ")
            sheet.Cells(sheetrowcounter, 5) = New Cell("")
            sheet.Cells(sheetrowcounter, 6) = New Cell("S=Suspect, V/V2=Valid/Double Check, P=Pending")
            sheetrowcounter += 1
            sheetrowcounter += 1
            sheet.Cells(sheetrowcounter, 8) = New Cell("Out Weight Brige")
            sheet.Cells(sheetrowcounter, 10) = New Cell("")
            sheet.Cells(sheetrowcounter, 18) = New Cell("Journey Trial")
            sheet.Cells(sheetrowcounter, 19) = New Cell("")
            sheet.Cells(sheetrowcounter, 22) = New Cell("Journey Back")
            sheet.Cells(sheetrowcounter, 23) = New Cell("")
            sheetrowcounter += 1

            sheet.Cells(sheetrowcounter, 0) = New Cell("Customer/Ship To Name")
            sheet.Cells.ColumnWidth(0) = 15000
            sheet.Cells(sheetrowcounter, 1) = New Cell("SHIP TO CODE")
            sheet.Cells.ColumnWidth(1) = 3300
            sheet.Cells(sheetrowcounter, 2) = New Cell("ORDER NO")
            sheet.Cells.ColumnWidth(2) = 3300
            sheet.Cells(sheetrowcounter, 3) = New Cell("EX")
            sheet.Cells.ColumnWidth(3) = 1500
            sheet.Cells(sheetrowcounter, 4) = New Cell("TPT")
            sheet.Cells.ColumnWidth(4) = 10000
            sheet.Cells(sheetrowcounter, 5) = New Cell("Vehicle No")
            sheet.Cells.ColumnWidth(5) = 3000
            sheet.Cells(sheetrowcounter, 6) = New Cell("MT")
            sheet.Cells.ColumnWidth(6) = 3000
            sheet.Cells(sheetrowcounter, 7) = New Cell("DN No")
            sheet.Cells.ColumnWidth(7) = 3000
            sheet.Cells(sheetrowcounter, 8) = New Cell("Date")
            sheet.Cells.ColumnWidth(8) = 3000
            sheet.Cells(sheetrowcounter, 9) = New Cell("Time")
            sheet.Cells.ColumnWidth(9) = 3000
            sheet.Cells(sheetrowcounter, 10) = New Cell("Journey to Cust")
            sheet.Cells.ColumnWidth(10) = 4000
            sheet.Cells(sheetrowcounter, 11) = New Cell("ATA")
            sheet.Cells.ColumnWidth(11) = 3000
            sheet.Cells(sheetrowcounter, 12) = New Cell("ATD")
            sheet.Cells.ColumnWidth(12) = 3000
            sheet.Cells(sheetrowcounter, 13) = New Cell("Time Spent at Site")
            sheet.Cells.ColumnWidth(13) = 3000
            sheet.Cells(sheetrowcounter, 14) = New Cell("Back to Source")
            sheet.Cells.ColumnWidth(14) = 3000
            sheet.Cells(sheetrowcounter, 15) = New Cell("PTO ON Time")
            sheet.Cells.ColumnWidth(15) = 3000
            sheet.Cells(sheetrowcounter, 16) = New Cell("PTO OFF Time")
            sheet.Cells.ColumnWidth(16) = 3000

            sheet.Cells(sheetrowcounter, 17) = New Cell("Stop/Idling >15 Mins")
            sheet.Cells.ColumnWidth(17) = 3000
            sheet.Cells(sheetrowcounter, 18) = New Cell("Geofence")
            sheet.Cells.ColumnWidth(18) = 3000
            sheet.Cells(sheetrowcounter, 19) = New Cell("Duration")
            sheet.Cells.ColumnWidth(19) = 3000
            sheet.Cells(sheetrowcounter, 20) = New Cell("PTO")
            sheet.Cells.ColumnWidth(20) = 3000
            sheet.Cells(sheetrowcounter, 21) = New Cell("PTO Status")
            sheet.Cells.ColumnWidth(21) = 3000

            sheet.Cells(sheetrowcounter, 22) = New Cell("Stop/Idling >15 Mins")
            sheet.Cells.ColumnWidth(22) = 3000
            sheet.Cells(sheetrowcounter, 23) = New Cell("Geofence")
            sheet.Cells.ColumnWidth(23) = 3000
            sheet.Cells(sheetrowcounter, 24) = New Cell("Duration")
            sheet.Cells.ColumnWidth(24) = 3000
            sheet.Cells(sheetrowcounter, 25) = New Cell("PTO")
            sheet.Cells.ColumnWidth(25) = 3000
            sheet.Cells(sheetrowcounter, 26) = New Cell("PTO Status")
            sheet.Cells.ColumnWidth(26) = 3000
            sheet.Cells(sheetrowcounter, 27) = New Cell("Data Lost & V-Data")
            sheet.Cells.ColumnWidth(27) = 3000
            sheet.Cells(sheetrowcounter, 28) = New Cell("Driver Name")
            sheet.Cells.ColumnWidth(28) = 3000
            sheet.Cells(sheetrowcounter, 29) = New Cell("DN Qty")
            sheet.Cells.ColumnWidth(29) = 3000

            sheet.Cells(sheetrowcounter, 30) = New Cell("Travelling {Mins}")
            sheet.Cells.ColumnWidth(30) = 3000
            sheet.Cells(sheetrowcounter, 31) = New Cell("Distance")
            sheet.Cells.ColumnWidth(31) = 3000
            sheet.Cells(sheetrowcounter, 32) = New Cell("Loading {Mins}")
            sheet.Cells.ColumnWidth(32) = 3000
            sheet.Cells(sheetrowcounter, 33) = New Cell("Waiting {Mins}")
            sheet.Cells.ColumnWidth(33) = 3000
            sheet.Cells(sheetrowcounter, 34) = New Cell("Unloading {Mins}")
            sheet.Cells.ColumnWidth(34) = 3000

            sheetrowcounter += 1
            Dim t As DataTable = Session("exceltable")
            For i As Integer = 0 To t.Rows.Count - 1
                Try
                    sheet.Cells(sheetrowcounter, 0) = New Cell(t.DefaultView.Item(i)(0))
                    sheet.Cells(sheetrowcounter, 1) = New Cell(Convert.ToDouble(t.DefaultView.Item(i)(1)))

                    Try
                        If IsDBNull(t.DefaultView.Item(i)(2)) Then
                            sheet.Cells(sheetrowcounter, 2) = New Cell("-")
                        Else
                            sheet.Cells(sheetrowcounter, 2) = New Cell(Convert.ToDouble(t.DefaultView.Item(i)(2)))
                        End If
                    Catch ex As Exception
                        ' WriteLog("21." & t.DefaultView.Item(i)(2))
                    End Try

                    ' sheet.Cells(sheetrowcounter, 2) = New Cell(Convert.ToDouble(t.DefaultView.Item(i)(2)))
                    sheet.Cells(sheetrowcounter, 3) = New Cell(t.DefaultView.Item(i)(3))
                    sheet.Cells(sheetrowcounter, 4) = New Cell(t.DefaultView.Item(i)(4))
                    sheet.Cells(sheetrowcounter, 5) = New Cell(t.DefaultView.Item(i)(5))
                    sheet.Cells(sheetrowcounter, 6) = New Cell(t.DefaultView.Item(i)(6))

                    Try
                        sheet.Cells(sheetrowcounter, 7) = New Cell(t.DefaultView.Item(i)(7))
                    Catch ex As Exception

                    End Try

                    sheet.Cells(sheetrowcounter, 8) = New Cell(t.DefaultView.Item(i)(8))
                    sheet.Cells(sheetrowcounter, 9) = New Cell(t.DefaultView.Item(i)(9))
                    sheet.Cells(sheetrowcounter, 10) = New Cell(t.DefaultView.Item(i)(10))
                    sheet.Cells(sheetrowcounter, 11) = New Cell(Convert.ToDateTime(t.DefaultView.Item(i)(11)).ToString("yyyy/MM/dd HH:mm:ss"))

                    sheet.Cells(sheetrowcounter, 12) = New Cell(t.DefaultView.Item(i)(12))
                    sheet.Cells(sheetrowcounter, 13) = New Cell(t.DefaultView.Item(i)(13))
                    sheet.Cells(sheetrowcounter, 14) = New Cell(t.DefaultView.Item(i)(14))
                    sheet.Cells(sheetrowcounter, 15) = New Cell(t.DefaultView.Item(i)(15))
                    sheet.Cells(sheetrowcounter, 16) = New Cell(t.DefaultView.Item(i)(16))

                    sheet.Cells(sheetrowcounter, 17) = New Cell(t.DefaultView.Item(i)(17))
                    sheet.Cells(sheetrowcounter, 18) = New Cell(t.DefaultView.Item(i)(18).ToString().Replace("<br/>", vbCrLf))
                    sheet.Cells(sheetrowcounter, 19) = New Cell(t.DefaultView.Item(i)(19).ToString().Replace("<br/>", vbCrLf))
                    sheet.Cells(sheetrowcounter, 20) = New Cell(t.DefaultView.Item(i)(20).ToString().Replace("<br/>", vbCrLf))

                    sheet.Cells(sheetrowcounter, 21) = New Cell(t.DefaultView.Item(i)(25).ToString())

                    sheet.Cells(sheetrowcounter, 22) = New Cell(t.DefaultView.Item(i)(21))
                    sheet.Cells(sheetrowcounter, 23) = New Cell(t.DefaultView.Item(i)(22).ToString().Replace("<br/>", vbCrLf))
                    sheet.Cells(sheetrowcounter, 24) = New Cell(t.DefaultView.Item(i)(23).ToString().Replace("<br/>", vbCrLf))
                    sheet.Cells(sheetrowcounter, 25) = New Cell(t.DefaultView.Item(i)(24).ToString().Replace("<br/>", vbCrLf))
                    sheet.Cells(sheetrowcounter, 26) = New Cell(t.DefaultView.Item(i)(26).ToString())
                    sheet.Cells(sheetrowcounter, 27) = New Cell(t.DefaultView.Item(i)(31).ToString())
                    Try
                        sheet.Cells(sheetrowcounter, 28) = New Cell(t.DefaultView.Item(i)(32).ToString())
                    Catch ex As Exception

                    End Try

                    Try
                        sheet.Cells(sheetrowcounter, 29) = New Cell(t.DefaultView.Item(i)(33).ToString())
                    Catch ex As Exception

                    End Try

                    Try
                        sheet.Cells(sheetrowcounter, 30) = New Cell(t.DefaultView.Item(i)(34).ToString())
                    Catch ex As Exception

                    End Try
                    Try
                        sheet.Cells(sheetrowcounter, 31) = New Cell(t.DefaultView.Item(i)(35).ToString())
                    Catch ex As Exception

                    End Try
                    Try
                        sheet.Cells(sheetrowcounter, 32) = New Cell(t.DefaultView.Item(i)(36).ToString())
                    Catch ex As Exception

                    End Try
                    Try
                        sheet.Cells(sheetrowcounter, 33) = New Cell(t.DefaultView.Item(i)(37).ToString())
                    Catch ex As Exception

                    End Try
                    Try
                        sheet.Cells(sheetrowcounter, 34) = New Cell(t.DefaultView.Item(i)(38).ToString())
                    Catch ex As Exception

                    End Try
                    sheetrowcounter += 1
                Catch ex As Exception
                    '    Response.Write("Line no- > " & i & " and error = " & ex.Message)
                End Try

            Next
            Dim isLog As Boolean = False
            Try



                Dim sheet2 As New ExcelLibrary.SpreadSheet.Worksheet(tplateno & " Log report ")
                sheet2.Cells(sheetrowcounter2, 0) = New Cell("Here Log report For plateno : " & tplateno & " From " & tbdt & "  To  " & tedt)
                sheet2.Cells(sheetrowcounter2, 9) = New Cell("")
                Try
                    If tplateno <> "ALL PLATES" Then
                        If DateDiff(DateInterval.Minute, Convert.ToDateTime(tbdt), Convert.ToDateTime(tedt)) <= 1440 Then
                            Dim locObj As New Location()
                            isLog = True
                            Try
                                sheetrowcounter2 += 1
                                sheetrowcounter2 += 3
                                sheet2.Cells(sheetrowcounter2, 0) = New Cell("S No")
                                sheet.Cells.ColumnWidth(0) = 3000
                                sheet2.Cells(sheetrowcounter2, 1) = New Cell("Date Time")
                                sheet.Cells.ColumnWidth(1) = 3000
                                sheet2.Cells(sheetrowcounter2, 2) = New Cell("GPS")
                                sheet.Cells.ColumnWidth(2) = 3000
                                sheet2.Cells(sheetrowcounter2, 3) = New Cell("Speed")
                                sheet.Cells.ColumnWidth(3) = 3000
                                sheet2.Cells(sheetrowcounter2, 4) = New Cell("Odometer")
                                sheet.Cells.ColumnWidth(4) = 9000
                                sheet2.Cells(sheetrowcounter2, 5) = New Cell("Ignition")
                                sheet.Cells.ColumnWidth(5) = 3000
                                sheet2.Cells(sheetrowcounter2, 6) = New Cell("PTO")
                                sheet.Cells.ColumnWidth(6) = 3000
                                sheet2.Cells(sheetrowcounter2, 7) = New Cell("Address")
                                sheet.Cells.ColumnWidth(7) = 10000
                                sheet2.Cells(sheetrowcounter2, 8) = New Cell("Nearest Town")
                                sheet.Cells.ColumnWidth(8) = 10000
                                sheet2.Cells(sheetrowcounter2, 9) = New Cell("Lat")
                                sheet.Cells.ColumnWidth(9) = 3000
                                sheet2.Cells(sheetrowcounter2, 10) = New Cell("Lon")
                                sheet.Cells.ColumnWidth(10) = 3000

                                sheetrowcounter2 += 1
                            Catch ex As Exception
                                WriteLog("Jaffa " & ex.Message)
                            End Try


                            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
                            Dim cmd As SqlCommand = New SqlCommand("select distinct convert(varchar(19),timestamp,120) as datetime,alarm,vt.pto,gps_av,speed,gps_odometer,ignition_sensor,lat,lon from vehicle_history    vht Join  vehicleTBL vt on vt.plateno=vht.plateno and " &
              "  vt.plateno ='" & tplateno & "' and gps_av='A' and ignition_sensor<>0  and timestamp between '" & tbdt & "' and '" & tedt & "'", conn)
                            Dim dr As SqlDataReader
                            Dim i As Integer = 1
                            '   WriteLog(cmd.CommandText)
                            Try
                                conn.Open()
                                dr = cmd.ExecuteReader()

                                While dr.Read()
                                    Try
                                        sheet2.Cells(sheetrowcounter2, 0) = New Cell(i)
                                        sheet2.Cells(sheetrowcounter2, 1) = New Cell(Convert.ToDateTime(dr("datetime")).ToString("yyyy/MM/dd HH:mm:ss"))
                                        sheet2.Cells(sheetrowcounter2, 2) = New Cell(dr("gps_av").ToString())
                                        sheet2.Cells(sheetrowcounter2, 3) = New Cell(Convert.ToDouble(System.Convert.ToDouble(dr("speed")).ToString("0.00")))
                                        sheet2.Cells(sheetrowcounter2, 4) = New Cell(Convert.ToDouble((System.Convert.ToDouble(dr("gps_odometer")) / 100.0).ToString("0.00")))

                                        If dr("ignition_sensor") = 1 Then
                                            sheet2.Cells(sheetrowcounter2, 5) = New Cell("ON")
                                        Else
                                            sheet2.Cells(sheetrowcounter2, 5) = New Cell("OFF")
                                        End If

                                        If dr("pto") Then
                                            sheet2.Cells(sheetrowcounter2, 6) = New Cell(dr("alarm").ToString())
                                        Else
                                            sheet2.Cells(sheetrowcounter2, 6) = New Cell("--")
                                        End If

                                        sheet2.Cells(sheetrowcounter2, 7) = New Cell(StripTags(locObj.GetLocation(dr("lat"), dr("lon")).ToString()))
                                        sheet2.Cells(sheetrowcounter2, 8) = New Cell(locObj.GetNearestTown(dr("lat"), dr("lon")).ToString())
                                        sheet2.Cells(sheetrowcounter2, 9) = New Cell(Convert.ToDouble(Convert.ToDouble(dr("lat")).ToString("0.0000")))
                                        sheet2.Cells(sheetrowcounter2, 10) = New Cell(Convert.ToDouble(Convert.ToDouble(dr("lon")).ToString("0.0000")))
                                        sheetrowcounter2 += 1
                                        i += 1
                                    Catch ex As Exception
                                        WriteLog("My DR Loop  : " & ex.Message)
                                    End Try
                                End While

                            Catch ex As Exception
                                WriteLog("DR loop : " & ex.Message)
                            Finally
                                conn.Close()

                            End Try
                        End If
                    End If

                Catch ex As Exception
                    WriteLog("Outer loop : " & ex.Message)
                Finally

                End Try
                wb.Worksheets.Add(sheet)
                If isLog Then
                    wb.Worksheets.Add(sheet2)
                End If



            Catch ex As Exception
                WriteLog("p : " & ex.Message)
            End Try


          



            Response.Clear()
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("content-disposition", "attachment;filename=DeliveryMonitoringSummaryDaily.xls")

            Dim m As System.IO.MemoryStream = New System.IO.MemoryStream()
            wb.SaveToStream(m)
            m.WriteTo(Response.OutputStream)

        Catch ex As Exception
            WriteLog("Final loop : " & ex.Message)
        End Try
    End Sub

    Function StripTags(ByVal html As String) As String
        ' Remove HTML tags.
        Return Regex.Replace(html, "<.*?>", "")
    End Function

    Protected Sub WriteLog(ByVal message As String)
        Try
            If (message.Length > 0) Then
                Dim sw As New StreamWriter(Server.MapPath("") & "\MyLog.txt", FileMode.Append)
                sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & " - " & message)
                sw.Close()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
