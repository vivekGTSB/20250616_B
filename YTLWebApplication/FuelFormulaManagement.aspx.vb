Imports System.Data.SqlClient
Imports System.Data

Namespace AVLS

    Partial Class FuelFormulaManagement
        Inherits System.Web.UI.Page
        Public ddlchange As Boolean = False
        Public ec As String = "false"

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub


        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            Try

                If Request.Cookies("userinfo") Is Nothing Then
                    Response.Redirect("Login.aspx")
                End If

                Dim userid As String = Request.Cookies("userinfo")("userid")
                Dim role As String = Request.Cookies("userinfo")("role")
                Dim userslist As String = Request.Cookies("userinfo")("userslist")

                Dim suserid As String = Request.QueryString("userid")


                Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
                Dim cmd As SqlCommand = New SqlCommand("select userid,username from userTBL where role='User' order by username", conn)
                Dim dr As SqlDataReader

                If role = "User" Then
                    cmd = New SqlCommand("select userid,username from userTBL where userid='" & userid & "' order by username", conn)
                ElseIf role = "SuperUser" Or role = "Operator" Then
                    cmd = New SqlCommand("select userid,username from userTBL where userid in(" & userslist & ") order by username", conn)
                End If

                conn.Open()
                dr = cmd.ExecuteReader()
                While dr.Read()
                    ddlusers.Items.Add(New ListItem(dr("username"), dr("userid")))
                End While
                conn.Close()

                If Not suserid = "" Then
                    ddlusers.SelectedValue = suserid
                End If


            Catch ex As Exception

            Finally

                MyBase.OnInit(e)

            End Try
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
            Try

                If Page.IsPostBack = False Then
                    ImageButton1.Attributes.Add("onclick", "return deleteconfirmation();")
                    ImageButton2.Attributes.Add("onclick", "return deleteconfirmation();")

                    FillGrid()
                End If
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
        End Sub

        Private Sub FillGrid()
            Try

                Dim userid As String = ddlusers.SelectedValue

                Dim t As New DataTable
                t.Columns.Add(New DataColumn("chk"))
                t.Columns.Add(New DataColumn("sno"))
                t.Columns.Add(New DataColumn("Plate No"))
                t.Columns.Add(New DataColumn("Level ID"))
                t.Columns.Add(New DataColumn("Formula"))
                t.Columns.Add(New DataColumn("Offset Value"))
                t.Columns.Add(New DataColumn("Type"))
                t.Columns.Add(New DataColumn("Tank"))
                t.Columns.Add(New DataColumn("Remarks"))

                Dim r As DataRow


                If Not userid = "--Select User Name--" Then
                    Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
                    'Dim cmd As SqlCommand = New SqlCommand("select * from vehicleTBL v, fuel_tank_check c, fuel_tank_formula f where c.formulaname=f.formulaname and v.userid='" & userid & "' and c.plateno=v.plateno and (f.formulatype='Tank Volume' or f.formulatype='Tank Cylinder') order by c.plateno,c.tankno,c.formulaname", conn)
                    Dim cmd As SqlCommand = New SqlCommand("select * from (vehicleTBL v left join fuel_tank_check c on v.plateno=c.plateno) left join fuel_tank_formula f on c.formulaname=f.formulaname  where v.userid='" & userid & "' and (f.formulatype='Tank Volume' or f.formulatype='Tank Cylinder' or f.formulatype is null) order by c.plateno,c.tankno,c.formulaname", conn)
                    Dim dr As SqlDataReader

                    conn.Open()

                    dr = cmd.ExecuteReader()
                    Dim i As Int32 = 1
                    While dr.Read
                        r = t.NewRow
                        'r(0) = "<input type=""checkbox"" name=""chk"" value=""" & dr("plateno") & """/>"
                        r(0) = "<input type=""checkbox"" name=""chk"" value=""" & dr("plateno") & ";" & dr("tankno") & """/>"
                        r(1) = i.ToString()
                        'r(2) = "<a rel=""balloon1""><img src=""vehiclesmallimages/" & dr("smallimage") & """ alt=""" & dr("plateno") & """ title=""" & dr("plateno") & """ width=""20px"" height=""20px"" onmouseover=""javascript:mouseover('" & dr("bigimage") & "');"" style=""vertical-align:middle;""/></a>&nbsp;<a href=""UpdateVehicle.aspx?pno=" & dr("plateno") & "&uid=" & dr("userid") & """>" & dr("plateno") & "</a>"
                        'r(2) = dr("plateno")
                        r(2) = "<a href=""UpdateFuelFormula.aspx?uno=" & userid & "&pno=" & dr("plateno") & "&fno=" & dr("formulaname") & """>" & dr("plateno") & "</a>"
                        r(3) = dr("formulaname")
                        r(4) = dr("formula")
                        r(5) = dr("value")
                        r(6) = dr("formulatype")
                        r(7) = dr("tankno")
                        r(8) = dr("remark")
                        If r(3) Is DBNull.Value Then
                            r(3) = "-"
                        End If
                        If r(4) Is DBNull.Value Then
                            r(4) = "-"
                        End If
                        If r(5) Is DBNull.Value Then
                            r(5) = "-"
                        End If
                        If r(6) Is DBNull.Value Then
                            r(6) = "-"
                        End If
                        If r(7) Is DBNull.Value Then
                            r(7) = "-"
                        End If
                        If r(8) Is DBNull.Value Then
                            r(8) = "-"
                        End If
                        If r(3) = "-" Then
                            r(2) = dr("plateno")
                        End If

                        If r(6) = "Tank Volume" Then
                            r(6) = "Volume"
                        ElseIf r(6) = "Tank Cylinder" Then
                            r(6) = "Cylinder"
                        End If

                        t.Rows.Add(r)
                        i = i + 1
                    End While

                    conn.Close()
                End If

                If t.Rows.Count = 0 Then
                    r = t.NewRow
                    r(0) = "<input type=""checkbox"" name=""chk"" />"
                    r(1) = "--"
                    r(2) = "--"
                    r(3) = "--"
                    r(4) = "--"
                    r(5) = "--"
                    r(6) = "--"
                    r(7) = "--"
                    r(8) = "--"
                    t.Rows.Add(r)
                End If

                ec = "true"
                vehiclesgrid.DataSource = t
                vehiclesgrid.DataBind()


            Catch ex As SystemException
                Response.Write(ex.Message)
            End Try
        End Sub

        Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
            DeleteVehicles()
        End Sub

        Protected Sub ImageButton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
            DeleteVehicles()
        End Sub

        Protected Sub DeleteVehicles()
            Try
                Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
                Dim command As SqlCommand
                Dim arraystring() As String = Split(Request.Form("chk"), ",")
                Dim strings() As String

                For x As Int16 = 0 To arraystring.Length - 1
                    strings = Convert.ToString(arraystring(x)).Split(";")
                    'Response.Write("delete from fuel_tank_check where plateno='" & strings(0) & "' and tankno='" & strings(1) & "'")
                    conn.Open()
                    command = New SqlCommand("delete from fuel_tank_check where plateno='" & strings(0) & "' and tankno='" & strings(1) & "'", conn)
                    command.ExecuteNonQuery()
                    conn.Close()
                Next

                'For i As Int16 = 0 To arraystring.Length - 1
                'conn.Open()
                'command = New SqlCommand("delete from fuel_tank_check where plateno='" & plateno(i) & "'", conn)
                'command.ExecuteNonQuery()
                'conn.Close()
                'Next
                FillGrid()
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
        End Sub

        Protected Sub ddlusers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlusers.SelectedIndexChanged
            Try
                FillGrid()
            Catch ex As SystemException
                Response.Write(ex.Message)
            End Try
        End Sub

    End Class

End Namespace
