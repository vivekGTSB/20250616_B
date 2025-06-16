Imports System.Data.SqlClient
Imports System.Data

Partial Class FuelManagement
    Inherits System.Web.UI.Page
    Public show As Boolean = False
    Public addfuelpage As String = "AddFuel.aspx"
    Public divgrid As Boolean = False
    Public ec As String = "false"

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
        Try
            Dim userid As String = Request.Cookies("userinfo")("userid")
            Dim role As String = Request.Cookies("userinfo")("role")
            Dim userslist As String = Request.Cookies("userinfo")("userslist")

            If userid = "0214" Then
                addfuelpage = "AddFuelToAll.aspx"
            End If

            ImageButton1.Attributes.Add("onclick", "return mysubmit()")

            If Page.IsPostBack = False Then
                txtBeginDate.Value = Now().ToString("yyyy/MM/dd")
                txtEndDate.Value = Now().ToString("yyyy/MM/dd")
                delete1.Attributes.Add("onclick", "return deleteconfirmation();")
                delete2.Attributes.Add("onclick", "return deleteconfirmation();")

                If Request.QueryString("s") <> "" Then
                    CompleteUpdateFuel()
                End If

            End If

            LimitUserAccess()

            FillGrid()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub FillGrid()
        Try

            Dim userid As String = ddlusername.SelectedValue
            Dim plateno As String = ddlpleate.SelectedValue
            Dim begintimestamp = txtBeginDate.Value & " " & ddlbh.SelectedValue & ":" & ddlbm.SelectedValue & ":00"
            Dim endtimestamp = txtEndDate.Value & " " & ddleh.SelectedValue & ":" & ddlem.SelectedValue & ":59"

            Dim userstable As New DataTable
            Dim ok As String = "no"
            Dim condition As String = ""
            Dim r As DataRow
            userstable.Rows.Clear()
            userstable.Columns.Add(New DataColumn("chk"))
            userstable.Columns.Add(New DataColumn("S No"))
            userstable.Columns.Add(New DataColumn("Plate No"))
            userstable.Columns.Add(New DataColumn("Date Time"))
            userstable.Columns.Add(New DataColumn("Fuel Id"))
            userstable.Columns.Add(New DataColumn("Fuel Station Code"))
            userstable.Columns.Add(New DataColumn("Fuel Type"))
            userstable.Columns.Add(New DataColumn("Liters"))
            userstable.Columns.Add(New DataColumn("Cost"))

            Dim conn As SqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))

            'If plateno = "--All Plate Numbers--" Then
            '    condition = ""
            '    fuelgrid.Columns.Item(2).Visible = True
            'Else
            '    condition = " and plateno='" & plateno & "'"
            '    fuelgrid.Columns.Item(2).Visible = False
            'End If

            If Not userid = "--Select User Name--" And Not plateno = "--Select Plate No--" Then

                'Dim cmd As SqlCommand = New SqlCommand("select fuelid,userid,plateno,convert(varchar(19),timestamp,120) as timestamp,stationcode,fueltype,liters,cost from fuel where userid='" & userid & "'" & condition & " and timestamp between '" & begintimestamp & "' and '" & endtimestamp & "' order by plateno,timestamp", conn)
                'If userid = "--All Users--" Then
                '    Dim role As String = Session("role")
                '    Dim userslist As String = Session("userslist")
                '    cmd = New SqlCommand("select fuelid,userid,plateno,convert(varchar(19),timestamp,120) as timestamp,stationcode,fueltype,liters,cost from fuel where timestamp between '" & begintimestamp & "' and '" & endtimestamp & "'" & condition & "order by plateno,timestamp", conn)
                '    If role = "User" Then
                '        cmd = New SqlCommand("select fuelid,userid,plateno,convert(varchar(19),timestamp,120) as timestamp,stationcode,fueltype,liters,cost from fuel where userid='" & userid & "' and " & condition & " timestamp between '" & begintimestamp & "' and '" & endtimestamp & "' order by plateno,timestamp", conn)
                '    ElseIf role = "SuperUser" Or role = "Operator" Then
                '        cmd = New SqlCommand("select fuelid,userid,plateno,convert(varchar(19),timestamp,120) as timestamp,stationcode,fueltype,liters,cost from fuel where userid in(" & userslist & ")  " & condition & " and timestamp between '" & begintimestamp & "' and '" & endtimestamp & "' order by plateno,timestamp", conn)
                '    End If
                'End If
                Dim cmd As SqlCommand = New SqlCommand("select fuelid,userid,plateno,convert(varchar(19),timestamp,120) as timestamp,stationcode,fueltype,liters,cost from fuel where plateno='" & plateno & "' and " & condition & " timestamp between '" & begintimestamp & "' and '" & endtimestamp & "' order by plateno,timestamp", conn)
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader()

                Dim i As Int32 = 1
                While dr.Read()
                    r = userstable.NewRow
                    If LimitUserAccess() = True Then
                        r(0) = ""
                        r(2) = dr("plateno")
                    Else
                        r(0) = "<input type=""checkbox"" name=""chk"" value=""" & dr("fuelid") & """/>"
                        r(2) = "<a href= UpdateFuel.aspx?fuelid=" & dr("fuelid") & " title='Update'> " & dr("plateno") & " </a>"
                    End If
                    r(1) = i.ToString()
                    r(3) = dr("timestamp")
                    r(4) = dr("fuelid")
                    r(5) = dr("stationcode")
                    r(6) = dr("fueltype")
                    r(7) = System.Convert.ToDouble(dr("liters")).ToString("0.00")
                    r(8) = System.Convert.ToDouble(dr("cost")).ToString("0.00")
                    userstable.Rows.Add(r)
                    i = i + 1
                    ok = "yes"
                End While
                'End While
                conn.Close()
            End If
            If ok = "no" Then
                'No Records Found

                r = userstable.NewRow
                r(0) = "-"
                r(1) = "-"
                r(2) = "-"
                r(3) = "-"
                r(4) = "-"
                r(5) = "-"
                r(6) = "-"
                r(7) = "-"
                r(8) = "-"
                userstable.Rows.Add(r)

            End If

            Session.Remove("exceltable")
            Session.Remove("exceltable2")
            fuelgrid.PageSize = noofrecords.SelectedValue
            Session("exceltable") = userstable
            fuelgrid.DataSource = userstable
            fuelgrid.DataBind()
            ec = "true"
            If fuelgrid.PageCount > 1 Then
                show = True
            End If

            If LimitUserAccess() = True Then
                fuelgrid.Columns(0).Visible = False
            End If

        Catch ex As Exception
            Response.Write(ex.Message)

        End Try
    End Sub

    Protected Sub DeleteDriver()

        Try
            Dim conn As SqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
            Dim command As SqlCommand
            Dim fuelid As String = ""

            Dim fuelids() As String = Request.Form("chk").Split(",")

            For i As Int32 = 0 To fuelids.Length - 1
                command = New SqlCommand("delete from fuel where fuelid='" & fuelids(i) & "'", conn)
                Try
                    conn.Open()
                    command.ExecuteNonQuery()
                Catch ex As Exception
                Finally
                    conn.Close()
                End Try
            Next
            FillGrid()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub fuelgrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles fuelgrid.PageIndexChanging
        'If ddlpleate.SelectedValue = "--All Plate Numbers--" Then
        '    fuelgrid.Columns.Item(2).Visible = True
        'Else
        '    fuelgrid.Columns.Item(2).Visible = False
        'End If
        ec = "true"
        fuelgrid.PageSize = noofrecords.SelectedValue
        fuelgrid.DataSource = Session("exceltable")
        fuelgrid.PageIndex = e.NewPageIndex
        fuelgrid.DataBind()
    End Sub

    Protected Sub delete2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles delete2.Click
        DeleteDriver()
    End Sub

    Protected Sub delete1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles delete1.Click
        DeleteDriver()
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        FillGrid()
    End Sub

    Protected Sub ddluser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlusername.SelectedIndexChanged
        getPlateNo(ddlusername.SelectedValue)
    End Sub

    Protected Sub getPlateNo(ByVal uid As String)
        Try
            If ddlUsername.SelectedValue <> "--Select User Name--" Then
                ddlpleate.Items.Clear()
                ddlpleate.Items.Add("--Select Plate No--")
                Dim cmd As SqlCommand
                Dim dr As SqlDataReader
                Dim connection As New Redirect(ddlUsername.SelectedValue)
                Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings(connection.sqlConnection))

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

    Protected Sub CompleteUpdateFuel()
        Dim startdate As String = Request.QueryString("s")
        Dim userid As String = Request.QueryString("u")
        Dim plateno As String = Request.QueryString("p")

        txtBeginDate.Value = Convert.ToDateTime(startdate).ToString("yyyy/MM/dd")
        txtEndDate.Value = Convert.ToDateTime(startdate).ToString("yyyy/MM/dd")

        ddlusername.SelectedValue = userid
        getPlateNo(userid)
        ddlpleate.SelectedValue = plateno
        FillGrid()
    End Sub

    Function getUserLevel() As String
        Try
            Dim cmd As SqlCommand
            Dim Userlevel As String
            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
            cmd = New SqlCommand("select usertype from userTBL where userid='" & Request.Cookies("userinfo")("userid") & "'", conn)
            conn.Open()
            Userlevel = cmd.ExecuteScalar()
            conn.Close()

            Return Userlevel
        Catch ex As SystemException
            Response.Write(ex.Message)
        End Try
    End Function

    Function LimitUserAccess() As Boolean
        If getUserLevel() = "7" Then
            delete1.Visible = False
            delete2.Visible = False
            Return True
        Else
            Return False
        End If
    End Function

End Class
