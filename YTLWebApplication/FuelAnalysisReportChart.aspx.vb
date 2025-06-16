Imports System.Data.SqlClient
Imports AspMap
Imports ADODB
Imports System.Data
Imports ChartDirector
Imports System.IO

Partial Class FuelAnalysisReportChart
    Inherits System.Web.UI.Page
    Public show As Boolean = False
    Public ec As String = "false"
    Dim suspectRefuel As Boolean = False
    Dim suspectTime As String
    Public suser As String
    Public sgroup As String
    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        Try

            If Request.Cookies("userinfo") Is Nothing Then
                Response.Redirect("Login.aspx")
            End If

            Dim cmd As SqlCommand
            Dim dr As SqlDataReader

            Dim userid As String = Request.Cookies("userinfo")("userid")
            Dim role As String = Request.Cookies("userinfo")("role")
            Dim userslist As String = Request.Cookies("userinfo")("userslist")
            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
            cmd = New SqlCommand("select userid, username,dbip from userTBL where role='User' order by username", conn)
            If role = "User" Then
                cmd = New SqlCommand("select userid, username, dbip from userTBL where userid='" & userid & "'", conn)
            ElseIf role = "SuperUser" Or role = "Operator" Then
                cmd = New SqlCommand("select userid, username, dbip from userTBL where userid in (" & userslist & ") order by username", conn)
            End If
            conn.Open()
            dr = cmd.ExecuteReader()
            While dr.Read()
                ddlUsername.Items.Add(New ListItem(dr("username"), dr("userid")))
            End While
            dr.Close()
            If role = "User" Then
                ddlUsername.Items.Remove("--Select User Name--")
                ddlUsername.SelectedValue = userid
                getPlateNo(userid)
            End If
            conn.Close()

        Catch ex As Exception


        End Try
        MyBase.OnInit(e)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ImageButton1.Attributes.Add("onclick", "return mysubmit()")
        Try
            If Page.IsPostBack = False Then
                ImageButton1.Attributes.Add("onclick", "return mysubmit()")
                txtBeginDate.Value = Now().ToString("yyyy/MM/dd")
                txtEndDate.Value = Now().ToString("yyyy/MM/dd")
                Label2.Visible = False
                Label3.Visible = False

                Dim plateno As String = Request.QueryString("p")
                Dim userid As String = Request.QueryString("u")
                If userid.IndexOf(",") > 0 Then
                    Dim sgroupname As String() = userid.Split(",")
                    suser = sgroupname(0)
                    sgroup = sgroupname(1)
                End If
                Dim cmd As SqlCommand
                Dim dr As SqlDataReader

                Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))

                If suser <> "" Then
                    cmd = New SqlCommand("select plateno from vehicleTBL where userid='" & suser & "' order by plateno", conn)
                Else
                    cmd = New SqlCommand("select plateno from vehicleTBL where userid='" & userid & "' order by plateno", conn)
                End If
                conn.Open()
                dr = cmd.ExecuteReader()
                While dr.Read()
                    ddlpleate.Items.Add(New ListItem(dr("plateno"), dr("plateno")))
                End While
                dr.Close()
                If suser <> "" Then
                    ddlUsername.SelectedValue = suser
                Else
                    ddlUsername.SelectedValue = userid
                End If
                ddlpleate.SelectedValue = Request.QueryString("p")

                conn.Close()

                If (plateno <> "") Then
                    Dim begindatetime As String
                    Dim enddatetime As String
                    If Request.QueryString("bdt") Is Nothing Then

                        begindatetime = txtBeginDate.Value & " " & ddlbh.SelectedValue & ":" & ddlbm.SelectedValue & ":00"
                        enddatetime = txtEndDate.Value & " " & ddleh.SelectedValue & ":" & ddlem.SelectedValue & ":00"

                    Else
                        begindatetime = Request.QueryString("bdt")
                        enddatetime = Request.QueryString("edt")
                        txtBeginDate.Value = DateTime.Parse(begindatetime).ToString("yyyy/MM/dd")
                        txtEndDate.Value = DateTime.Parse(enddatetime).ToString("yyyy/MM/dd")

                    End If
                    ddlUsername.SelectedValue = userid
                    ddlpleate.SelectedValue = Request.QueryString("p")

                    DisplayChart(plateno, begindatetime, enddatetime, ddlUsername.SelectedItem.Text)
                    DisplaySummary(plateno, begindatetime, enddatetime, ddlUsername.SelectedValue)
                    DisplayRecords(plateno, begindatetime, enddatetime, ddlUsername.SelectedValue)

                End If

            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageButton1.Click
        Try
            Dim plateno As String = platenotemp.Value
            Dim begindatetime As String = txtBeginDate.Value & " " & ddlbh.SelectedValue & ":" & ddlbm.SelectedValue & ":00"
            Dim enddatetime As String = txtEndDate.Value & " " & ddleh.SelectedValue & ":" & ddlem.SelectedValue & ":00"
            Dim userid As String = ddlUsername.SelectedValue

            DisplayChart(plateno, begindatetime, enddatetime, ddlUsername.SelectedItem.Text)
            DisplaySummary(plateno, begindatetime, enddatetime, userid)
            DisplayRecords(plateno, begindatetime, enddatetime, userid)
            Label2.Visible = True
            Label3.Visible = True

        Catch ex As Exception

        End Try


    End Sub
    Protected Sub DisplaySummary(ByVal plateno As String, ByVal begindatetime As String, ByVal enddatetime As String, ByVal userid As String)

        Try

            Dim dieselPrice As Double

            Dim bdate As DateTime = Convert.ToDateTime(begindatetime)
            Dim edate As DateTime = Convert.ToDateTime(enddatetime)
            Dim dateCounter As Integer = 0
            Dim exitloop As Boolean = False

            Do While exitloop = False
                If bdate.Day <> edate.Day Then
                    bdate = bdate.AddDays(1)
                    dateCounter += 1
                Else
                    exitloop = True
                End If
            Loop

            '##########################################################################
            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
            Dim cmd As SqlCommand
            cmd = New SqlCommand("select subsidyprice from user_customize where userid='" & ddlUsername.SelectedValue & "' and subsidy='true'", conn)
            conn.Open()
            Dim subsidy As Double = cmd.ExecuteScalar()
            conn.Close()
            '##########################################################################

            '##########################################################################
            cmd = New SqlCommand("select value from vehicle_average_idling where plateno='" & ddlpleate.SelectedValue & "'", conn)
            conn.Open()
            Dim idlingValue As Double = cmd.ExecuteScalar()
            conn.Close()

            If idlingValue = 0 Then
                idlingValue = 3
            End If
            '##########################################################################

            Dim r As DataRow

            Dim t2 As New DataTable
            t2.Columns.Add(New DataColumn("Begin Date Time"))
            t2.Columns.Add(New DataColumn("End Date Time"))
            t2.Columns.Add(New DataColumn("Mileage"))
            t2.Columns.Add(New DataColumn("Fuel"))
            t2.Columns.Add(New DataColumn("Fuel Cost"))
            t2.Columns.Add(New DataColumn("Liter/KM"))
            t2.Columns.Add(New DataColumn("KM/Liter"))
            t2.Columns.Add(New DataColumn("Cost/liter"))

            r = t2.NewRow

            Dim dailyOdometer As Double = 0
            Dim dailyFuelConsumption As Double = 0
            Dim dailyFuelCost As Double = 0
            Dim displaytext As String = ""
            Dim displayMinute As Double = 0
            Dim dailyIdlingTime As TimeSpan
            ' Dim userid As String = ddlUsername.SelectedValue
            Dim startDate As String = ""
            Dim endDate As String = ""


            For x As Int32 = 0 To dateCounter
                Dim begindatetimex As DateTime
                Dim enddatetimex As DateTime

                If x = 0 Then
                    begindatetimex = Convert.ToDateTime(begindatetime).AddDays(x).ToString("yyyy/MM/dd HH:mm:ss")
                Else
                    begindatetimex = Convert.ToDateTime(begindatetime).AddDays(x).ToString("yyyy/MM/dd 00:00:00")
                End If

                If x = dateCounter Then
                    enddatetimex = Convert.ToDateTime(enddatetime).ToString("yyyy/MM/dd HH:mm:ss")
                Else
                    enddatetimex = Convert.ToDateTime(begindatetime).AddDays(x).ToString("yyyy/MM/dd 23:59:59")
                End If



                Dim fuelobj As New FuelMath1(plateno)
                Dim dFuel As New RefuelBeta(plateno, begindatetimex, enddatetimex)
                Dim dTable As New DataTable
                Dim dPrice As New DataTable

                Dim selectionDateTime As String = " timestamp between '" & Convert.ToDateTime(begindatetimex).ToString("yyyy/MM/dd HH:mm:ss") & "' and '" & Convert.ToDateTime(enddatetimex).ToString("yyyy/MM/dd HH:mm:ss") & "'"
                dTable = dFuel.RefuelOnly(userid, plateno, selectionDateTime, 18)
                dPrice = dFuel.fuelPrice()
                Dim drPrice2 As DataRow()


                '###### Fuel Consumption Table ###############################################################################
                If dFuel.fuelstartdate <> "" And dFuel.fuelenddate <> "" Then
                    drPrice2 = dPrice.Select("StartDate <= #" & dFuel.fuelstartdate & "# And EndDate >= #" & dFuel.fuelstartdate & "#")
                    dieselPrice = CDbl(drPrice2(0)(2))
                    If startDate = "" Then
                        startDate = Convert.ToDateTime(dFuel.fuelstartdate).ToString("yyyy/MM/dd HH:mm:ss")
                    End If
                    If x = dateCounter Then
                        endDate = Convert.ToDateTime(dFuel.fuelenddate).ToString("yyyy/MM/dd HH:mm:ss")
                    End If
                    dailyOdometer = dailyOdometer + CDbl(dFuel.fuelOdometerTotal)
                    If dFuel.fuelConsumptionTotal > 0 Then
                        dailyFuelConsumption = dailyFuelConsumption + CDbl(dFuel.fuelConsumptionTotal)
                        dailyFuelCost = dailyFuelCost + (CDbl(CDbl(dFuel.fuelConsumptionTotal) * CDbl(CDbl(drPrice2(0)(2)))))
                    End If
                    '####################################################################################################### 
                ElseIf fuelobj.formula1 <> "" Then
                End If
                'response.write(Convert.ToDateTime(begindatetime).AddDays(x).ToString("yyyy/MM/dd HH:mm:ss"))
                '@@@@@@@@@@@@@@@@@@@@@@@@@ idling start @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                Dim totalidlingtime As TimeSpan = dFuel.getidlingNew(plateno, begindatetimex, enddatetimex)
                dailyIdlingTime = dailyIdlingTime.Add(TimeSpan.FromMinutes((totalidlingtime.Days * 24 * 60) + (totalidlingtime.Hours * 60) + totalidlingtime.Minutes))
                If dailyIdlingTime.Days > 0 Then
                    displaytext = dailyIdlingTime.Days & " Days " & dailyIdlingTime.Hours & " Hours " & dailyIdlingTime.Minutes & " Minutes"
                    displayMinute = (dailyIdlingTime.Days * 24 * 60) + (dailyIdlingTime.Hours * 60) + dailyIdlingTime.Minutes
                Else
                    displaytext = dailyIdlingTime.Hours & " Hours " & dailyIdlingTime.Minutes & " Minutes"
                    displayMinute = (dailyIdlingTime.Hours * 60) + dailyIdlingTime.Minutes
                End If
                '@@@@@@@@@@@@@@@@@@@@@@@@@ idling end @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            Next

            If dailyOdometer > 0 Then
                r(0) = startDate
                r(1) = endDate
                r(2) = dailyOdometer.ToString("0.00")
                If dailyFuelConsumption > 0 Then
                    r(3) = CDbl(dailyFuelConsumption).ToString("0.00")
                    r(4) = CDbl(dailyFuelCost).ToString("0.00")
                    If dailyOdometer > 50 Then
                        r(5) = CDbl(dailyFuelConsumption / dailyOdometer).ToString("0.00")
                        r(6) = CDbl(dailyOdometer / dailyFuelConsumption).ToString("0.00")
                        r(7) = CDbl(dailyFuelCost / dailyOdometer).ToString("0.00")
                    Else
                        r(5) = "--"
                        r(6) = "--"
                        r(7) = "--"
                    End If
                Else
                    r(3) = "--"
                    r(4) = "--"
                    r(5) = "--"
                    r(6) = "--"
                    r(7) = "--"
                End If

            Else
                r(0) = "--"
                r(1) = "--"
                r(2) = "--"
                r(3) = "--"
                r(4) = "--"
                r(5) = "--"
                r(6) = "--"
                r(7) = "--"
            End If

            t2.Rows.Add(r)

            Dim t3 As New DataTable
            t3.Columns.Add(New DataColumn("Idling Time"))
            t3.Columns.Add(New DataColumn("Idling Fuel"))
            t3.Columns.Add(New DataColumn("Hour Idling Fuel"))
            t3.Columns.Add(New DataColumn("Idling Cost"))
            t3.Columns.Add(New DataColumn("Total Idling Cost"))

            If displayMinute > 0 Then
                r = t3.NewRow
                r(0) = displaytext
                r(1) = CDbl(displayMinute / 60 * idlingValue).ToString("0.00")
                r(2) = CDbl(idlingValue).ToString("0.00")
                r(3) = CDbl(idlingValue * dieselPrice).ToString("0.00")
                r(4) = CDbl(r(1) * dieselPrice).ToString("0.00")
                If subsidy > 0 Then
                    r(4) = "(" & CDbl(r(1) * subsidy).ToString("0.00") & ") " & CDbl(r(1) * dieselPrice).ToString("0.00")
                End If
                t3.Rows.Add(r)
            Else
                r = t3.NewRow
                r(0) = "--"
                r(1) = "--"
                r(2) = "--"
                r(3) = "--"
                r(4) = "--"
                t3.Rows.Add(r)
            End If

            Session.Remove("exceltable")
            Session.Remove("exceltable2")

            Session("exceltable") = t2
            Session("exceltable2") = t3

            GridView2.DataSource = t2
            GridView2.DataBind()
            GridView3.DataSource = t3
            GridView3.DataBind()

            ec = "true"
            If GridView1.PageCount > 1 Then
                show = True
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub


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

    'Protected Sub DisplaySummary(ByVal plateno As String, ByVal begindatetime As String, ByVal enddatetime As String, ByVal userid As String)
    '    Try

    '        Dim dieselPrice As Double

    '        '##########################################################################

    '        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
    '        Dim cmd As SqlCommand
    '        cmd = New SqlCommand("select subsidyprice from user_customize where userid='" & userid & "' and subsidy='true'", conn)
    '        conn.Open()
    '        Dim subsidy As Double = cmd.ExecuteScalar()
    '        conn.Close()
    '        '##########################################################################

    '        Dim fuelobj As New FuelMath1(ddlpleate.SelectedValue)
    '        Dim dFuel As New RefuelBeta(ddlpleate.SelectedValue, begindatetime, enddatetime)
    '        Dim dTable As New DataTable
    '        Dim dPrice As New DataTable

    '        Dim r As DataRow
    '        dTable = dFuel.rTable
    '        dPrice = dFuel.fuelPrice()

    '        Dim t2 As New DataTable
    '        t2.Columns.Add(New DataColumn("Begin Date Time"))
    '        t2.Columns.Add(New DataColumn("End Date Time"))
    '        t2.Columns.Add(New DataColumn("Mileage"))
    '        t2.Columns.Add(New DataColumn("Fuel"))
    '        t2.Columns.Add(New DataColumn("Fuel Cost"))
    '        t2.Columns.Add(New DataColumn("Liter/KM"))
    '        t2.Columns.Add(New DataColumn("KM/Liter"))
    '        t2.Columns.Add(New DataColumn("Cost/liter"))


    '        r = t2.NewRow
    '        If dFuel.fuelstartdate <> "" And dFuel.fuelenddate <> "" Then
    '            r(0) = dFuel.fuelstartdate
    '            r(1) = dFuel.fuelenddate
    '            r(2) = CDbl(dFuel.fuelOdometerTotal).ToString("0.00")
    '            If dFuel.fuelConsumptionTotal > 0 Then
    '                Dim drPrice2 As DataRow() = dPrice.Select("StartDate <= #" & dFuel.fuelstartdate & "# And EndDate >= #" & dFuel.fuelstartdate & "#")
    '                dieselPrice = CDbl(drPrice2(0)(2))
    '                r(3) = CDbl(dFuel.fuelConsumptionTotal).ToString("0.00")
    '                r(4) = CDbl(CDbl(dFuel.fuelConsumptionTotal) * CDbl(dieselPrice)).ToString("0.00")
    '                If subsidy > 0 Then
    '                    r(4) = "(" & CDbl(CDbl(dFuel.fuelConsumptionTotal) * subsidy).ToString("0.00") & ") " & CDbl(CDbl(dFuel.fuelConsumptionTotal) * CDbl(dieselPrice)).ToString("0.00")
    '                    lblSubsidy.Visible = True
    '                Else : lblSubsidy.Visible = False
    '                End If
    '                If dFuel.fuelOdometerTotal > 50 Then
    '                    r(5) = CDbl(r(3) / r(2)).ToString("0.00")
    '                    r(6) = CDbl(r(2) / r(3)).ToString("0.00")
    '                    r(7) = CDbl((CDbl(dFuel.fuelConsumptionTotal) * CDbl(dieselPrice)) / r(2)).ToString("0.00")
    '                    If subsidy > 0 Then
    '                        r(7) = "(" & CDbl((CDbl(dFuel.fuelConsumptionTotal) * subsidy / r(2))).ToString("0.00") & ") " & CDbl((CDbl(dFuel.fuelConsumptionTotal) * CDbl(dieselPrice)) / r(2)).ToString("0.00")
    '                    End If
    '                Else
    '                    r(5) = "--"
    '                    r(6) = "--"
    '                    r(7) = "--"
    '                End If
    '            Else
    '                r(3) = "--"
    '                r(4) = "--"
    '                r(5) = "--"
    '                r(6) = "--"
    '                r(7) = "--"
    '            End If
    '        Else
    '            r(0) = "--"
    '            r(1) = "--"
    '            r(2) = "--"
    '            r(3) = "--"
    '            r(4) = "--"
    '            r(5) = "--"
    '            r(6) = "--"
    '            r(7) = "--"
    '        End If
    '        t2.Rows.Add(r)

    '        Dim t3 As New DataTable
    '        t3.Columns.Add(New DataColumn("Idling Time"))
    '        t3.Columns.Add(New DataColumn("Idling Fuel"))
    '        t3.Columns.Add(New DataColumn("Hour Idling Fuel"))
    '        t3.Columns.Add(New DataColumn("Idling Cost"))
    '        t3.Columns.Add(New DataColumn("Total Idling Cost"))

    '        Dim totalidlingtime As TimeSpan = dFuel.getidlingNew(ddlpleate.SelectedValue, begindatetime, enddatetime)
    '        Dim displaytext As String
    '        Dim displayMinute As Double

    '        If totalidlingtime.Days > 0 Then
    '            displaytext = totalidlingtime.Days & " Days " & totalidlingtime.Hours & " Hours " & totalidlingtime.Minutes & " Minutes"
    '            displayMinute = (totalidlingtime.Days * 24 * 60) + (totalidlingtime.Hours * 60) + totalidlingtime.Minutes
    '        Else
    '            displaytext = totalidlingtime.Hours & " Hours " & totalidlingtime.Minutes & " Minutes"
    '            displayMinute = (totalidlingtime.Hours * 60) + totalidlingtime.Minutes
    '        End If

    '        If displayMinute > 0 Then

    '            '##########################################################################

    '            conn = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
    '            cmd = New SqlCommand("select value from vehicle_average_idling where plateno='" & ddlpleate.SelectedValue & "'", conn)
    '            conn.Open()
    '            Dim idlingValue As Double = cmd.ExecuteScalar()
    '            conn.Close()

    '            If idlingValue = 0 Then
    '                idlingValue = 3
    '            End If
    '            '##########################################################################

    '            Dim fuelstartdate As String
    '            If dFuel.fuelstartdate = "" Then
    '                fuelstartdate = begindatetime
    '            Else
    '                fuelstartdate = dFuel.fuelstartdate
    '            End If
    '            Dim drPrice2 As DataRow() = dPrice.Select("StartDate <= #" & fuelstartdate & "# And EndDate >= #" & fuelstartdate & "#")
    '            dieselPrice = CDbl(drPrice2(0)(2))

    '            r = t3.NewRow
    '            'r(0) = dFuel.getIdling(ddlpleate.SelectedValue, begindatetime, enddatetime)
    '            r(0) = displaytext
    '            r(1) = CDbl(displayMinute / 60 * idlingValue).ToString("0.00")
    '            r(2) = CDbl(idlingValue).ToString("0.00")
    '            r(3) = CDbl(idlingValue * dieselPrice).ToString("0.00")
    '            r(4) = CDbl(r(1) * dieselPrice).ToString("0.00")
    '            If subsidy > 0 Then
    '                r(4) = "(" & CDbl(r(1) * subsidy).ToString("0.00") & ") " & CDbl(r(1) * dieselPrice).ToString("0.00")
    '            End If
    '            t3.Rows.Add(r)
    '        Else
    '            r = t3.NewRow
    '            r(0) = "--"
    '            r(1) = "--"
    '            r(2) = "--"
    '            r(3) = "--"
    '            r(4) = "--"
    '            t3.Rows.Add(r)
    '        End If

    '        GridView2.DataSource = t2
    '        GridView2.DataBind()
    '        GridView3.DataSource = t3
    '        GridView3.DataBind()
    '        Session.Remove("exceltable")
    '        Session.Remove("exceltable2")
    '        Session("exceltable") = t2
    '        Session("exceltable2") = t3

    '        ec = "true"
    '        If GridView1.PageCount > 1 Then
    '            show = True
    '        End If

    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try

    'End Sub

    Protected Sub DisplayRecords(ByVal plateno As String, ByVal begindatetime As String, ByVal enddatetime As String, ByVal userid As String)
        'On Error Resume Next
        Try

            'Dim begindatetime = txtBeginDate.Value & " " & ddlbh.SelectedValue & ":" & ddlbm.SelectedValue & ":00"
            'Dim enddatetime = txtEndDate.Value & " " & ddleh.SelectedValue & ":" & ddlem.SelectedValue & ":59"

            Dim t As New DataTable
            t.Columns.Add(New DataColumn("S No"))
            t.Columns.Add(New DataColumn("Date Time"))
            t.Columns.Add(New DataColumn("GPS AV"))
            t.Columns.Add(New DataColumn("Speed"))
            t.Columns.Add(New DataColumn("Ignition"))
            t.Columns.Add(New DataColumn("Odometer"))
            t.Columns.Add(New DataColumn("OdometerIncrement"))
            t.Columns.Add(New DataColumn("Tank Level"))
            t.Columns.Add(New DataColumn("Tank Volume"))
            t.Columns.Add(New DataColumn("Tank Level 2"))
            t.Columns.Add(New DataColumn("Tank Volume 2"))
            t.Columns.Add(New DataColumn("Total Volume"))


            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
            Dim fuelObj As New FuelMath1(ddlpleate.SelectedValue)
            Dim da As SqlDataAdapter
            'previously included checking oil_tank_level with 9876,9998,9999 to avoid return 0
            'If fuelObj.f2Found = True Then
            '    da = New SqlDataAdapter("select distinct convert(varchar(19),timestamp,120) as datetime,gps_av,speed,ignition_sensor,gps_odometer, oil_tank_level1, oil_tank_level2 from vehicle_history where plateno ='" & ddlpleate.SelectedValue & "' and timestamp between '" & begindatetime & "' and '" & enddatetime & "' and gps_odometer<>99 and oil_tank_level1 not in (9876,9998,9999) and oil_tank_level2 not in (9876,9998,9999) Order By datetime", conn)
            'Else
            '    da = New SqlDataAdapter("select distinct convert(varchar(19),timestamp,120) as datetime,gps_av,speed,ignition_sensor,gps_odometer, oil_tank_level1 from vehicle_history where plateno ='" & ddlpleate.SelectedValue & "' and timestamp between '" & begindatetime & "' and '" & enddatetime & "' and gps_odometer<>99 and oil_tank_level1 not in (9876,9998,9999) Order By datetime", conn)
            'End If

            Dim f2Found As Boolean = fuelObj.f2Found
            If f2Found = True Then
                da = New SqlDataAdapter("select distinct convert(varchar(19),timestamp,120) as datetime,gps_av,speed,ignition_sensor,gps_odometer, oil_tank_level1, oil_tank_level2 from vehicle_history where plateno ='" & plateno & "' and timestamp between '" & begindatetime & "' and '" & enddatetime & "' and gps_odometer<>99 Order By datetime", conn)
            Else
                da = New SqlDataAdapter("select distinct convert(varchar(19),timestamp,120) as datetime,gps_av,speed,ignition_sensor,gps_odometer, oil_tank_level1 from vehicle_history where plateno ='" & plateno & "' and timestamp between '" & begindatetime & "' and '" & enddatetime & "' and gps_odometer<>99 Order By datetime", conn)
            End If

            Dim ds As New Data.DataSet

            'Try

            da.Fill(ds)

            Dim r As DataRow

            Dim OdometerIncrement As Double = 0
            Dim rawtanklevel As Double
            Dim rawtanklevel2 As Double
            ' #########

            Dim prevOdometer, prevTankLevel1, prevTankVolume1, prevTankLevel2, prevTankVolume2, prevTankTotal As String
            Dim curOdometer, curSpeed, curTankLevel1, curTankVolume1, curTankLevel2, curTankVolume2, curTankTotal As String

            Dim curDateTime, curData, curIgnition As String

            For i As Int32 = 0 To ds.Tables(0).Rows.Count - 1

                rawtanklevel = ds.Tables(0).Rows(i)("oil_tank_level1")

                If f2Found = True Then
                    rawtanklevel2 = ds.Tables(0).Rows(i)("oil_tank_level2")
                Else
                    rawtanklevel2 = 0
                End If

                curOdometer = CDbl(fuelObj.CalcOdometer(ds.Tables(0).Rows(i)("gps_odometer"))).ToString("0.00")
                curSpeed = CDbl(ds.Tables(0).Rows(i)("speed")).ToString("0.00")
                curDateTime = ds.Tables(0).Rows(i)("datetime")
                curData = ds.Tables(0).Rows(i)("gps_av")
                curIgnition = ds.Tables(0).Rows(i)("ignition_sensor")

                If rawtanklevel <> "9876" And rawtanklevel <> "9998" And rawtanklevel <> "9999" Then
                    curTankLevel1 = CDbl(fuelObj.CalcTankLevel(rawtanklevel)).ToString("0.00")
                    curTankVolume1 = CDbl(fuelObj.CalcTankVolume(fuelObj.CalcTankLevel(rawtanklevel))).ToString("0.00")
                    curTankTotal = CDbl(CDbl(curTankVolume1)).ToString("0.00")
                Else
                    curTankLevel1 = "-"
                    curTankVolume1 = "-"
                    curTankTotal = "-"
                End If
                If rawtanklevel2 <> "9876" And rawtanklevel2 <> "9998" And rawtanklevel2 <> "9999" Then
                    curTankLevel2 = CDbl(fuelObj.CalcTankLevel2(rawtanklevel2)).ToString("0.00")
                    curTankVolume2 = CDbl(fuelObj.CalcTankVolume2(fuelObj.CalcTankLevel2(rawtanklevel2))).ToString("0.00")
                    If curTankVolume1 <> "-" Then
                        curTankTotal = CDbl(CDbl(curTankVolume1) + CDbl(curTankVolume2)).ToString("0.00")
                    Else
                        curTankTotal = "-"
                    End If
                Else
                    curTankLevel2 = "-"
                    curTankVolume2 = "-"
                    curTankTotal = "-"
                End If



                r = t.NewRow
                r(0) = (i + 1).ToString()
                r(1) = curDateTime
                r(2) = curData
                r(3) = curSpeed
                r(4) = "OFF"
                If curIgnition = 1 Then
                    r(4) = "ON"
                End If

                r(5) = curOdometer
                If i > 0 Then
                    r(6) = Convert.ToDouble((curOdometer - prevOdometer)).ToString("0.00")
                    If r(6) >= 2.5 Then
                        OdometerIncrement = OdometerIncrement + r(6)
                    End If
                Else
                    r(6) = "0.00"
                End If

                If rawtanklevel <> "9876" And rawtanklevel <> "9998" And rawtanklevel <> "9999" Then
                    r(7) = curTankLevel1
                    r(8) = curTankVolume1
                Else

                    If i = 0 Then
                        curTankLevel1 = "-"
                        curTankVolume1 = "-"
                        r(7) = "-"
                        r(8) = "-"
                    Else
                        r(7) = "<span style='font-style:italic;text-decoration:underline;'>" & prevTankLevel1 & "</span>"
                        r(8) = "<span style='font-style:italic;text-decoration:underline;'>" & prevTankVolume1 & "</span>"
                    End If

                End If

                If f2Found = True Then
                    If rawtanklevel2 <> "9876" And rawtanklevel2 <> "9998" And rawtanklevel2 <> "9999" Then
                        r(9) = curTankLevel2
                        r(10) = curTankVolume2
                        If rawtanklevel <> "9876" And rawtanklevel <> "9998" And rawtanklevel <> "9999" Then
                            r(11) = curTankTotal
                        Else
                            If i = 0 Then
                                curTankTotal = "-"
                                r(11) = "-"
                            Else
                                r(11) = "<span style='font-style:italic;text-decoration:underline;'>" & prevTankTotal & "</span>"
                            End If

                        End If

                    Else
                        If i = 0 Then
                            curTankLevel2 = "-"
                            curTankVolume2 = "-"
                            curTankTotal = "-"
                            r(9) = "-"
                            r(10) = "-"
                            r(11) = "-"
                        Else
                            r(9) = "<span style='font-style:italic;text-decoration:underline;'>" & prevTankLevel2 & "</span>"
                            r(10) = "<span style='font-style:italic;text-decoration:underline;'>" & prevTankVolume2 & "</span>"
                            r(11) = "<span style='font-style:italic;text-decoration:underline;'>" & prevTankTotal & "</span>"
                        End If

                    End If
                    Session("f2") = True
                Else
                    r(9) = "-"
                    r(10) = "-"
                    r(11) = "-"
                    Session("f2") = False
                End If

                t.Rows.Add(r)

                '########################################################
                'assigning current variables to previous variables
                prevOdometer = curOdometer

                prevTankLevel1 = curTankLevel1
                prevTankVolume1 = curTankVolume1
                prevTankLevel2 = curTankLevel2
                prevTankVolume2 = curTankVolume2
                prevTankTotal = curTankTotal
                '##########################################################

            Next

            If t.Rows.Count = 0 Then
                r = t.NewRow
                r(0) = "--"
                r(1) = "--"
                r(2) = "--"
                r(3) = "--"
                r(4) = "--"
                r(5) = "--"
                r(6) = "--"
                r(7) = "--"
                r(8) = "--"
                r(9) = "--"
                r(10) = "--"
                r(11) = "--"
                t.Rows.Add(r)

            End If

            'Catch ex As Exception
            'Response.Write(ex.Message)
            'Finally
            conn.Close()
            'End Try

            GridView1.PageSize = noofrecords.SelectedValue
            GridView1.DataSource = t
            GridView1.DataBind()
            ec = "true"

            If f2Found = False Then
                GridView1.Columns(9).Visible = False
                GridView1.Columns(10).Visible = False
                GridView1.Columns(11).Visible = False
            End If
            If GridView1.PageCount > 1 Then
                show = True
            End If

            Dim sessionTable As DataTable = t.Copy()

            If f2Found = False Then
                sessionTable.Columns.Remove("Tank Level 2")
                sessionTable.Columns.Remove("Tank Volume 2")
                sessionTable.Columns.Remove("Total Volume")
            End If
            Session("exceltable3") = sessionTable
            Session("gridview1") = t
            ViewState("exceltable") = t

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Protected Sub DisplayChart(ByVal plateno As String, ByVal begindatetime As String, ByVal enddatetime As String, ByVal userid As String)
        Try

            'Dim platenoCount As Int16 = ddlpleate.SelectedItem.Text.Length
            'Dim username As String = ddlUsername.SelectedItem.Text
            'Dim plateno As String = ddlpleate.SelectedItem.Text
            'Dim begindatetime As String = txtBeginDate.Value & " " & ddlbh.SelectedValue & ":" & ddlbm.SelectedValue & ":00"
            'Dim enddatetime As String = txtEndDate.Value & " " & ddleh.SelectedValue & ":" & ddlem.SelectedValue & ":59"

            Dim fuelObj As New FuelMath1(plateno)

            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
            'Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
            'Dim cmd As New SqlCommand("select * from fuel_calibration where formulaname in (select formulaname from fuel_tank_check where plateno='" & ddlpleate.SelectedItem.Text & "')", conn)
            Dim dr As SqlDataReader
            Dim i As Integer = 0

            Dim cmdString As String = ""
            If fuelObj.f2Found = True Then
                cmdString = "select distinct convert(varchar(19),timestamp,120) as datetime,gps_av,speed,ignition_sensor,gps_odometer, oil_tank_level1, oil_tank_level2 from vehicle_history where plateno ='" & plateno & "' and timestamp between '" & begindatetime & "' and '" & enddatetime & "' and gps_odometer<>99 and oil_tank_level1 not in (9876,9998,9999) and oil_tank_level2 not in (9876,9998,9999) order by datetime"
            Else
                cmdString = "select distinct convert(varchar(19),timestamp,120) as datetime,gps_av,speed,ignition_sensor,gps_odometer, oil_tank_level1 from vehicle_history where plateno ='" & plateno & "' and timestamp between '" & begindatetime & "' and '" & enddatetime & "' and gps_odometer<>99 and oil_tank_level1 not in (9876,9998,9999) order by datetime"
            End If

            Dim cmd As New SqlCommand(cmdString, conn)

            'Data for the chart
            Dim data0() As Double = {}
            Dim data1() As Double = {}
            Dim data2() As Double = {}
            Dim data3() As Double = {}
            Dim data4() As Double = {}

            'Labels for the chart
            Dim labels() As String = {}

            conn.Open()
            dr = cmd.ExecuteReader()
            i = 0

            Dim enter As String = "no"
            Dim imagestatus As String = "no"
            Dim fuelMin As Double = 0
            Dim fuelMax As Double = 0
            Dim fuelCount As Int16 = 0
            Dim fuelValue As Double = 0
            Dim prevDate As DateTime
            Dim prevOdometer, prevLevel, prevLitre, prevIgnition, odoIncrement, levelIncrement, litreIncrement, minuteDivider, odoCount, levelCount, litreCount, oneTimeCount As Double
            Dim curOdo, prevOdo As Double
            Dim pLevel, pVolume As Double
            Dim zeroFound As Boolean = False

            While dr.Read()
                imagestatus = "yes"
                ReDim Preserve data0(i)
                ReDim Preserve data1(i)
                ReDim Preserve data2(i)
                ReDim Preserve data3(i)
                ReDim Preserve data4(i)
                ReDim Preserve labels(i)

                data0(i) = fuelObj.CalcOdometer(dr("gps_odometer")) ' Convert.ToDouble((dr("gps_odometer") / 100.0)).ToString("0.00")
                data1(i) = dr("speed")
                If fuelObj.f2Found = True Then
                    data2(i) = fuelObj.CalcTankVolume(fuelObj.CalcTankLevel(dr("oil_tank_level1"))) + fuelObj.CalcTankVolume2(fuelObj.CalcTankLevel2(dr("oil_tank_level2")))
                    data3(i) = (fuelObj.CalcTankLevel(dr("oil_tank_level1")) + fuelObj.CalcTankLevel2(dr("oil_tank_level2"))) / 2
                Else
                    data2(i) = fuelObj.CalcTankVolume(fuelObj.CalcTankLevel(dr("oil_tank_level1")))
                    data3(i) = fuelObj.CalcTankLevel(dr("oil_tank_level1"))
                End If
                ' Response.Write(data2(i))
                '  WriteLog("Test" & data2(i))
                data4(i) = dr("ignition_sensor")
                labels(i) = dr("datetime")
                curOdo = data0(i)

                If i > 0 Then

                    If zeroFound = False Then
                        pVolume = data2(i - 1)
                        pLevel = data3(i - 1)
                    End If

                    If i > 2 Then
                        If data2(i) - data2(i - 2) < 3 And data2(i) - data2(i - 2) > -3 And data2(i) - data2(i - 1) < -10 Then
                            data2(i - 1) = data2(i - 2)
                        ElseIf data2(i) - data2(i - 2) < 3 And data2(i) - data2(i - 2) > -3 And data2(i) - data2(i - 1) > 10 Then
                            data2(i - 1) = data2(i - 2)
                        End If
                    End If

                    If i > 2 Then
                        If data3(i) - data3(i - 2) < 3 And data3(i) - data3(i - 2) > -3 And data3(i) - data3(i - 1) < -10 Then
                            data3(i - 1) = data3(i - 2)
                        ElseIf data3(i) - data3(i - 2) < 3 And data3(i) - data3(i - 2) > -3 And data3(i) - data3(i - 1) > 10 Then
                            data3(i - 1) = data3(i - 2)
                        End If
                    End If

                    'If DateDiff(DateInterval.Minute, prevDate, dr("datetime")) > 30 And (data2(i) - prevLevel < 0) And (data0(i) - prevOdometer > 10) Then
                    If (DateDiff(DateInterval.Minute, prevDate, dr("datetime")) > 30 And (data0(i) - prevOdometer > 30) And (data2(i) - prevLevel < -10) And (data4(i) = 1 And prevIgnition = 1)) _
                    Or (DateDiff(DateInterval.Minute, prevDate, dr("datetime")) > 30 And (data0(i) - prevOdometer > 50) And (data2(i) - prevLevel < -10) And (data4(i) = 0 And prevIgnition = 1)) _
                    Or (DateDiff(DateInterval.Minute, prevDate, dr("datetime")) > 30 And (data0(i) - prevOdometer > 50) And (data2(i) - prevLevel < -10) And (data4(i) = 0 And prevIgnition = 0)) _
                    Or (DateDiff(DateInterval.Minute, prevDate, dr("datetime")) > 30 And (data0(i) - prevOdometer > 50) And (data2(i) - prevLevel < -10) And (data4(i) = 1 And prevIgnition = 0)) Then
                        minuteDivider = Convert.ToDouble(DateDiff(DateInterval.Minute, prevDate, dr("datetime")))
                        odoIncrement = (curOdo - prevOdo) / minuteDivider
                        levelIncrement = (data2(i) - prevLevel) / minuteDivider
                        litreIncrement = (data3(i) - prevLitre) / minuteDivider

                        For w As Int16 = 0 To minuteDivider - 1

                            ReDim Preserve data0(i)
                            ReDim Preserve data2(i)
                            ReDim Preserve data3(i)
                            ReDim Preserve labels(i)

                            If oneTimeCount = 0 Then
                                odoCount = data0(i - 1) + odoIncrement 'odoCount = prevOdometer + odoIncrement
                                levelCount = prevLevel + levelIncrement
                                litreCount = prevLitre + litreIncrement
                                oneTimeCount = 1
                            Else
                                odoCount = odoCount + odoIncrement
                                levelCount = levelCount + levelIncrement
                                litreCount = litreCount + litreIncrement
                            End If

                            data0(i) = odoCount.ToString("0.00")
                            data2(i) = levelCount.ToString("0.00")
                            data3(i) = litreCount.ToString("0.00")
                            labels(i) = prevDate.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss")
                            prevDate = labels(i)
                            i = i + 1

                            If w = minuteDivider - 1 Then
                                i = i - 1
                            End If

                        Next
                    End If

                    If oneTimeCount = 0 Then
                        '''' ## odometer increment for each record 
                        If curOdo - prevOdo < 0 Then
                            data0(i) = data0(i - 1)
                            'curOdo = data0(i)' this will make odometer reset cannot add back the increment, comment out first
                        ElseIf curOdo - prevOdo > 10 Then ' And DateDiff(DateInterval.Minute, prevDate, dr("datetime")) < 5 Then ' added before for the date counter
                            data0(i) = data0(i - 1)
                        Else
                            data0(i) = (curOdo - prevOdo) + data0(i - 1)
                        End If
                    End If

                    If data3(i) = 0 Then
                        data3(i) = pLevel
                        data2(i) = pVolume
                        zeroFound = True
                    Else
                        zeroFound = False
                    End If

                End If

                prevOdo = curOdo

                prevDate = dr("datetime")
                prevIgnition = dr("ignition_sensor")
                prevOdometer = fuelObj.CalcOdometer(dr("gps_odometer"))
                If fuelObj.f2Found = True Then
                    prevLevel = fuelObj.CalcTankVolume(fuelObj.CalcTankLevel(dr("oil_tank_level1"))) + fuelObj.CalcTankVolume2(fuelObj.CalcTankLevel2(dr("oil_tank_level2")))
                    prevLitre = (fuelObj.CalcTankLevel(dr("oil_tank_level1")) + fuelObj.CalcTankLevel2(dr("oil_tank_level2"))) / 2
                Else
                    prevLevel = fuelObj.CalcTankVolume(fuelObj.CalcTankLevel(dr("oil_tank_level1")))
                    prevLitre = fuelObj.CalcTankLevel(dr("oil_tank_level1"))
                End If


                oneTimeCount = 0

                fuelValue = data2(i)

                If fuelCount = 0 Then
                    fuelMin = fuelValue
                    fuelCount = 1
                End If

                If fuelValue > fuelMax Then
                    fuelMax = fuelValue
                End If

                If fuelValue < fuelMin Then
                    fuelMin = fuelValue
                End If

                i = i + 1

            End While
            conn.Close()


            If imagestatus = "no" Then
                WebChartViewer1.Visible = False
                Image1.Visible = True
                Image1.ImageUrl = "~/images/NoDataWide.jpg"
            End If


            'Create a XYChart object of size 600 x 250 pixels
            Dim c As XYChart = New XYChart(750, 510)

            c.addTitle("(" & userid & ")  " & plateno & "  Fuel Analysis Chart", "Arial Bold Italic", 15).setBackground(&HFFFFFF)
            c.setPlotArea(130, 100, 500, 300, &HF4FDEF).setGridColor(&HCCCCCC, &HCCCCCC)

            Dim legendBox As LegendBox = c.addLegend(370, 90, False, "Arial Bold", 8)
            legendBox.setAlignment(Chart.BottomCenter)
            legendBox.setBackground(Chart.Transparent, Chart.Transparent)

            'Set the labels on the x axis.
            c.xAxis().setLabels(labels)
            c.xAxis().setLabels(labels).setFontAngle(45)

            If UBound(labels) <= 24 Then
                c.xAxis().setLabelStep(3, 1)
            Else
                c.xAxis().setLabelStep(Convert.ToInt32(UBound(labels) / 24))
            End If

            values.Value = labels.Length & "," & data0.Length & "," & data2.Length & "," & data1.Length & "," & data3.Length & ","

            'Add a title to the x-axis
            c.xAxis().setTitle("Timestamp")

            'Add a title on top of the primary (left) y axis.
            c.yAxis().setTitle("Odometer<*br*>(KM)").setAlignment(Chart.TopLeft2)
            c.yAxis().setColors(&HCC0000, &HCC0000, &HCC0000)

            'Add a title on top of the secondary (right) y axis.
            c.yAxis2().setTitle("Tank<*br*>(MM)").setAlignment(Chart.TopRight2)
            c.yAxis2().setColors(&H8000, &H8000, &H8000)

            'Add the third y-axis at 50 pixels to the left of the plot area
            Dim leftAxis As Axis = c.addAxis(Chart.Left, 60)
            leftAxis.setTitle("Speed<*br*>(KM/H)").setAlignment(Chart.TopLeft2)
            leftAxis.setColors(&HCC, &HCC, &HCC)

            'Add the fouth y-axis at 50 pixels to the right of the plot area
            Dim rightAxis As Axis = c.addAxis(Chart.Right, 50)
            rightAxis.setTitle("Tank<*br*>(Litre)").setAlignment(Chart.TopRight2)
            rightAxis.setColors(&H880088, &H880088, &H880088)

            c.yAxis().setLinearScale(CDbl(data0(0)) - 100, CDbl(data0(data0.Length - 1)) + 100)
            If fuelObj.f2Found = True Then
                c.yAxis2().setLinearScale(0, ((CDbl(fuelObj.CalcMaxLevel) + CDbl(fuelObj.CalcMaxLevel2)) / 2) + 100)
                rightAxis.setLinearScale(0, CDbl(fuelObj.calcMaxVolume) + CDbl(fuelObj.calcMaxVolume2))
            Else
                c.yAxis2().setLinearScale(0, CDbl(fuelObj.CalcMaxLevel) + 100)
                rightAxis.setLinearScale(0, CDbl(fuelObj.calcMaxVolume) + 100)
            End If



            Dim layer0 As LineLayer = c.addLineLayer(data0, &HCC0000, "Odometer (KM)")
            layer0.setLineWidth(2)

            Dim layer1 As LineLayer = c.addLineLayer(data3, &H8000, "Tank Level (MM)")
            layer1.setLineWidth(2)
            layer1.setUseYAxis2()

            Dim layer2 As LineLayer = c.addLineLayer(data1, &HCC, "Speed (KM/H)")
            layer2.setUseYAxis(leftAxis)

            Dim layer3 As LineLayer = c.addLineLayer(data2, &H880088, "Tank Volume (L)")
            layer3.setLineWidth(2)
            layer3.setUseYAxis(rightAxis)

            Dim tank1size, tank2size, tank1shape, tank2shape As String

            cmd = New SqlCommand("select tank1size,tank2size,tank1shape,tank2shape from vehicleTBL where plateno='" & ddlpleate.SelectedItem.Text & "'", conn)
            conn.Open()
            dr = cmd.ExecuteReader()
            If dr.Read() Then
                If dr("tank1size") Is DBNull.Value Then
                    tank1size = ""
                    tank1shape = ""
                Else
                    tank1size = dr("tank1size")
                    If dr("tank1shape") Is DBNull.Value Then
                        tank1shape = "<empty>"
                    Else
                        tank1shape = dr("tank1shape")
                    End If
                End If

                If dr("tank2size") Is DBNull.Value Then
                    tank2size = ""
                    tank2shape = ""
                Else
                    tank2size = dr("tank2size")
                    If dr("tank2shape") Is DBNull.Value Then
                        tank2shape = "<empty>"
                    Else
                        tank2shape = dr("tank2shape")
                    End If
                End If
            End If
            dr.Close()
            If tank1size = "" And tank2size = "" Then
                Dim textbox As ChartDirector.TextBox = c.addText(130, 30, "<*block*> Minimum Odometer<*br*>Maximum Odometer<*/*> <*block*>: " & data0(0) & " KM<*br*>: " & data0(data0.Length - 1) & " KM<*/*>", "Tahoma", 8, &H889988)
                Dim textbox2 As ChartDirector.TextBox = c.addText(345, 30, "<*block*> Minimum Volume<*br*>Maximum Volume<*/*> <*block*>: " & fuelMin.ToString("0.00") & " L<*br*>: " & fuelMax.ToString("0.00") & " L<*/*>", "Tahoma", 8, &H889988)
                Dim textbox3 As ChartDirector.TextBox = c.addText(530, 30, "<*block*>Travelled<*br*>Level ID<*/*> <*block*>: " & (data0(data0.Length - 1) - data0(0)).ToString("0.00") & " KM<*br*>: " & fuelObj.formula1 & "<*/*>", "Tahoma", 8, &H889988) '<*/*>", "Segoe UI", 8, &H889988)
                If fuelObj.f2Found = True Then
                    Dim textbox4 As ChartDirector.TextBox = c.addText(640, 30, "<*block*><*br*>Level2 ID<*/*> <*block*> <*br*>:" & fuelObj.formula2 & "<*/*>", "Tahoma", 8, &H889988) '<*/*>", "Segoe UI", 8, &H889988)
                End If
            End If
            If tank1size <> "" And tank2size = "" Then
                Dim textbox As ChartDirector.TextBox = c.addText(40, 30, "<*block*> Min. Odometer<*br*>Max. Odometer<*/*> <*block*>: " & data0(0) & " KM<*br*>: " & data0(data0.Length - 1) & " KM<*/*>", "Tahoma", 8, &H889988)
                Dim textbox2 As ChartDirector.TextBox = c.addText(240, 30, "<*block*> Min. Volume<*br*>Max. Volume<*/*> <*block*>: " & fuelMin.ToString("0.00") & " L<*br*>: " & fuelMax.ToString("0.00") & " L<*/*>", "Tahoma", 8, &H889988)
                Dim textbox3 As ChartDirector.TextBox = c.addText(405, 30, "<*block*>Travelled<*/*> <*block*>: " & (data0(data0.Length - 1) - data0(0)).ToString("0.00") & " KM<*/*>", "Tahoma", 8, &H889988) '<*/*>", "Segoe UI", 8, &H889988)
                Dim textbox4 As ChartDirector.TextBox = c.addText(405, 30, "<*block*><*br*> Level ID<*/*> <*block*> <*br*>: " & fuelObj.formula1 & "<*/*>", "Tahoma", 8, &H889988)
                Dim textbox5 As ChartDirector.TextBox = c.addText(540, 30, "<*block*>Tank Size<*br*>Tank Shape<*/*><*block*>: " & tank1size & "<*br*>: " & tank1shape & "<*/*>", "Tahoma", 8, &H889988)
            End If
            If tank1size <> "" And tank2size <> "" Then
                Dim textbox As ChartDirector.TextBox = c.addText(45, 30, "<*block*> Min. Odometer<*br*>Max. Odometer<*/*> <*block*>: " & data0(0) & " KM<*br*>: " & data0(data0.Length - 1) & " KM<*/*>", "Tahoma", 8, &H889988)
                Dim textbox2 As ChartDirector.TextBox = c.addText(215, 30, "<*block*> Min. Volume<*br*>Max. Volume<*/*> <*block*>: " & fuelMin.ToString("0.00") & " L<*br*>: " & fuelMax.ToString("0.00") & " L<*/*>", "Tahoma", 8, &H889988)
                Dim textbox3 As ChartDirector.TextBox = c.addText(485, 30, "<*block*>Tank1 <*br*>Tank2 <*/*><*block*>: " & tank1size & " (" & tank1shape & ")<*br*>: " & tank2size & " (" & tank2shape & ")<*/*>", "Tahoma", 8, &H889988)
                Dim textbox4 As ChartDirector.TextBox = c.addText(365, 30, "<*block*>Level1 ID<*br*>Level2 ID<*/*><*block*>: " & fuelObj.formula1 & "<*br*>: " & fuelObj.formula2 & "<*/*>", "Tahoma", 8, &H889988)
            End If
            WebChartViewer1.Image = c.makeWebImage(Chart.PNG)
            WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
            "title='{dataSetName} at {xLabel}: {value}'")
            If imagestatus = "yes" Then
                Image1.Visible = False
                WebChartViewer1.Visible = True
            End If

        Catch ex As Exception
            'Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            GridView1.PageSize = noofrecords.SelectedValue
            GridView1.DataSource = ViewState("exceltable")
            GridView1.PageIndex = e.NewPageIndex
            GridView1.DataBind()
            GridView2.DataSource = Session("exceltable")
            GridView2.DataBind()

            GridView3.DataSource = Session("exceltable2")
            GridView3.DataBind()

            If Session("f2") = False Then
                GridView1.Columns(9).Visible = False
                GridView1.Columns(10).Visible = False
                GridView1.Columns(11).Visible = False
            Else
                GridView1.Columns(9).Visible = True
                GridView1.Columns(10).Visible = True
                GridView1.Columns(11).Visible = False
            End If


            ec = "true"
            show = True
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try
            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    If CStr(e.Row.Cells(7).Text) = "9876" Then
            '        e.Row.Cells(7).ColumnSpan = 2
            '        e.Row.Cells(8).Visible = False
            '        e.Row.Cells(7).Text = "Unit Defected"
            '        e.Row.Cells(7).Font.Bold = True
            '        e.Row.Style.Add("background-color", "#ECE5FA") 'C4D46B
            '        e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Center
            '        'e.Row.Cells(7).Width = 200
            '    ElseIf CStr(e.Row.Cells(7).Text) = "9998" Then
            '        e.Row.Cells(7).ColumnSpan = 2
            '        e.Row.Cells(8).Visible = False
            '        e.Row.Cells(7).Text = "Fuel Sensor Disconnected"
            '        e.Row.Cells(7).Font.Bold = True
            '        e.Row.Style.Add("background-color", "#F0FAE5") 'C4D46B
            '        e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Center
            '        'e.Row.Cells(7).Width = 200
            '    ElseIf CStr(e.Row.Cells(7).Text) = "9999" Then
            '        e.Row.Cells.Remove(e.Row.Cells(7))
            '        e.Row.Cells(7).ColumnSpan = 2
            '        'e.Row.Cells(8).Visible = False
            '        e.Row.Cells(7).Text = "Fuel Sensor Reading Error"
            '        e.Row.Cells(7).Font.Bold = True
            '        e.Row.Style.Add("background-color", "#FAF0E5") 'C4D46B
            '        e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Center
            '        'e.Row.Cells(7).Width = 200
            '    End If
            '    If CStr(e.Row.Cells(9).Text) = "9876" Then
            '        e.Row.Cells(9).ColumnSpan = 2
            '        e.Row.Cells(10).Visible = False
            '        e.Row.Cells(9).Text = "Unit Defected"
            '        e.Row.Cells(9).Font.Bold = True
            '        e.Row.Style.Add("background-color", "#ECE5FA") 'C4D46B
            '        e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Center
            '        'e.Row.Cells(9).Width = 200
            '    ElseIf CStr(e.Row.Cells(9).Text) = "9998" Then
            '        e.Row.Cells(9).ColumnSpan = 2
            '        e.Row.Cells(10).Visible = False
            '        e.Row.Cells(9).Text = "Fuel Sensor Disconnected"
            '        e.Row.Cells(9).Font.Bold = True
            '        e.Row.Style.Add("background-color", "#F0FAE5") 'C4D46B
            '        e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Center
            '        'e.Row.Cells(9).Width = 200
            '    ElseIf CStr(e.Row.Cells(9).Text) = "9999" Then
            '        e.Row.Cells(9).ColumnSpan = 2
            '        e.Row.Cells(10).Visible = False
            '        e.Row.Cells(9).Text = "Fuel Sensor Reading Error"
            '        e.Row.Cells(9).Font.Bold = True
            '        e.Row.Style.Add("background-color", "#FAF0E5") 'C4D46B
            '        e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Center
            '        'e.Row.Cells(9).Width = 200
            '    End If
            'End If
        Catch ex As SystemException
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub ddlUsername_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUsername.SelectedIndexChanged
        getPlateNo(ddlUsername.SelectedValue)
    End Sub

    Protected Sub getPlateNo(ByVal uid As String)
        Try

            WebChartViewer1.Visible = False
            Image1.Visible = False

            If ddlUsername.SelectedValue <> "--Select User Name--" Then
                ddlpleate.Items.Clear()
                ddlpleate.Items.Add("--Select Plate No--")
                Dim cmd As SqlCommand
                Dim dr As SqlDataReader

                Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))

                cmd = New SqlCommand("select plateno from vehicleTBL where userid='" & uid & "' order by plateno", conn)
                conn.Open()
                dr = cmd.ExecuteReader()
                While dr.Read()
                    ddlpleate.Items.Add(New ListItem(dr("plateno"), dr("plateno")))
                End While
                dr.Close()

                conn.Close()
            Else
                ddlpleate.Items.Clear()
                ddlpleate.Items.Add("--Select User Name--")
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub


End Class
