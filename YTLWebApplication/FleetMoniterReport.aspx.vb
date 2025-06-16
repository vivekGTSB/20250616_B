Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Web.Script.Services
Imports AspMap
Imports Newtonsoft.Json

Partial Class FleetMoniterReport
    Inherits System.Web.UI.Page
    Public Shared ec As String = "false"
    Public show As Boolean = False
    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        Try
            If Request.Cookies("userinfo") Is Nothing Then
                Response.Redirect("Login.aspx")
            End If

        Catch ex As Exception

        Finally
            MyBase.OnInit(e)
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Request.Cookies("userinfo") Is Nothing Then
                Response.Redirect("Login.aspx")
            End If
            Dim conn As SqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
            Dim cmd As New SqlCommand("select userid ,username  from YTLDB .dbo .userTBL where userid  not in (7144,7145,7146,7147,7148,7099,7180) and companyname  like 'ytl%' and role ='User' order by username", conn)
            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            ddltransportername.Items.Clear()
            ddltransportername.Items.Add(New ListItem("ALL", "ALL"))
            While dr.Read()
                ddltransportername.Items.Add(New ListItem(dr("username"), dr("userid")))
            End While
            conn.Close()
        Catch ex As Exception

        End Try
    End Sub

    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)>
    Public Shared Function GetData(ByVal type As String, ByVal username As String) As String
        Dim conn As SqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
        Dim aa As New ArrayList()
        Dim a As ArrayList
        Dim userid As String = HttpContext.Current.Request.Cookies("userinfo")("userid")
        Dim role As String = HttpContext.Current.Request.Cookies("userinfo")("role")
        Dim uname As String = HttpContext.Current.Request.Cookies("userinfo")("username")
        Dim userslist As String = HttpContext.Current.Request.Cookies("userinfo")("userslist")
        Dim query As String = ""
        Try
            Dim cmd As New SqlCommand()
            cmd.Connection = conn
            If username = "ALL" Then
                If type = "ALL" Then
                    cmd.CommandText = "select t1.plateno ,isnull(pmid,'-') as pmid,isnull(remark,'') as remark,isnull(status,'Running') as status  from vehicletbl t1 left outer join vehicle_status_tracked2 t2 on t1.plateno =t2.plateno order by t1.plateno"
                ElseIf type = "1" Then
                    cmd.CommandText = "select t1.plateno ,isnull(pmid,'-') as pmid,isnull(remark,'') as remark,isnull(status,'Running') as status  from vehicletbl t1 left outer join vehicle_status_tracked2 t2 on t1.plateno =t2.plateno where t1.userid in (select userid  from YTLDB .dbo .userTBL where companyname  like 'ytl%' and role ='User') order by t1.plateno"
                Else
                    cmd.CommandText = "select t1.plateno ,isnull(pmid,'-') as pmid,isnull(remark,'') as remark,isnull(status,'Running') as status  from vehicletbl t1 left outer join vehicle_status_tracked2 t2 on t1.plateno =t2.plateno  where t1.userid in (select userid  from YTLDB .dbo .userTBL where companyname not like 'ytl%' and role ='User') order by t1.plateno"
                End If
            Else
                cmd.CommandText = "select t1.plateno ,isnull(pmid,'-') as pmid,isnull(remark,'') as remark,isnull(status,'Running') as status  from vehicletbl t1 left outer join vehicle_status_tracked2 t2 on t1.plateno =t2.plateno where t1.userid=" & username & " order by t1.plateno"
            End If

            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            Dim i As Integer = 1
            While dr.Read()
                a = New ArrayList()
                a.Add(i)
                a.Add(dr("pmid"))
                a.Add(dr("plateno"))
                a.Add(dr("status"))
                a.Add(dr("remark"))
                aa.Add(a)
                i += 1
            End While
            Dim t As New DataTable
            t.Columns.Add(New DataColumn("S NO"))
            t.Columns.Add(New DataColumn("PM Id"))
            t.Columns.Add(New DataColumn("Plate NO"))
            t.Columns.Add(New DataColumn("Status"))
            t.Columns.Add(New DataColumn("Remarks"))
            Try
                Dim r As DataRow
                For Each a In aa
                    r = t.NewRow()
                    For i = 0 To 4
                        r(i) = a(i)
                    Next
                    t.Rows.Add(r)
                Next
                HttpContext.Current.Session.Remove("exceltable")
                HttpContext.Current.Session.Remove("exceltable2")
                HttpContext.Current.Session("exceltable") = t

            Catch ex As Exception
                a = New ArrayList()
                a.Add(ex.Message)
                a.Add(ex.StackTrace)
                aa.Add(a)
            End Try


        Catch ex As Exception
            a = New ArrayList()
            a.Add(ex.Message)
            a.Add(ex.StackTrace)
            aa.Add(a)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
        Dim jss As New Newtonsoft.Json.JsonSerializer()
        Dim json As String = "{""aaData"":" & JsonConvert.SerializeObject(aa, Formatting.None) & "}"
        Return json
    End Function
    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)>
    Public Shared Function GetUsers(ByVal type As String) As String
        Dim conn As SqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
        Dim aa As New ArrayList()
        Dim a As ArrayList
        Try
            Dim cmd As New SqlCommand()
            cmd.Connection = conn
            If type = "ALL" Then
                cmd.CommandText = "select userid,username from usertbl where role='User' order by username"
            ElseIf type = "1" Then
                cmd.CommandText = "select userid ,username  from YTLDB .dbo .userTBL where userid  not in (7144,7145,7146,7147,7148,7099,7180) and companyname  like 'ytl%' and role ='User' order by username"
            Else
                cmd.CommandText = "select userid ,username  from userTBL where companyname not like 'ytl%' and role ='User' order by username"
            End If
            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            While dr.Read()
                a = New ArrayList()
                a.Add(dr("userid"))
                a.Add(dr("username"))
                aa.Add(a)
            End While
        Catch ex As Exception
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
        Dim jss As New Newtonsoft.Json.JsonSerializer()
        Dim json As String = "{""aaData"":" & JsonConvert.SerializeObject(aa, Formatting.None) & "}"
        Return json
    End Function






End Class
