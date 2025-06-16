Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Web.Script.Services
Imports AspMap
Imports Newtonsoft.Json

Partial Class ECRLReport
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
            uid.Value = Request.Cookies("userinfo")("userid")
            If Page.IsPostBack = False Then
                txtBeginDate.Value = Now().ToString("yyyy/MM/dd")
                txtEndDate.Value = Now().ToString("yyyy/MM/dd")
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
    Public Shared Function GetData(ByVal fromd As String) As String
        Dim conn As SqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection2"))
        Dim aa As New ArrayList()
        Dim a As ArrayList
        Dim userid As String = HttpContext.Current.Request.Cookies("userinfo")("userid")
        Dim role As String = HttpContext.Current.Request.Cookies("userinfo")("role")
        Dim uname As String = HttpContext.Current.Request.Cookies("userinfo")("username")
        Dim userslist As String = HttpContext.Current.Request.Cookies("userinfo")("userslist")
        Dim query As String = ""
        Try
            Dim cmd As New SqlCommand("select * from dbo.fn_getecrlsummary(@date) order by shiptocode", conn)
            cmd.Parameters.AddWithValue("@date", fromd)
            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            Dim counter As Integer = 0
            Dim datastruct As New Dictionary(Of String, shiptocode)
            Dim statedict As New Dictionary(Of String, String)
            Dim shipcd As shiptocode
            statedict.Add("76940", "PAHANG;6-1-SGL;B.I.MAHKOTA")
            statedict.Add("111683", "PAHANG;6-1-CH;PANCHING SELATAN331")
            statedict.Add("100031", "PAHANG;6-1-DE;PANCHING TIMURSA")
            statedict.Add("77922", "PAHANG;6-1-KG;KG BELIMBING, MARANB")
            statedict.Add("100123", "PAHANG;6-2-CH;AUR GADING, MARAN373")
            statedict.Add("77631", "PAHANG;6-2-BTG;KG BENTUNG, MARAN")
            statedict.Add("111006", "PAHANG;6-2-CH;PEKAN TAJAU392")
            statedict.Add("100419", "PAHANG;6-3-CH;KG JENGKA BATU 13406")
            statedict.Add("112203", "PAHANG;6-3-CH;KG KETAM, MARAN372")
            statedict.Add("77033", "PAHANG;6-3-KGT;CHENOR")
            statedict.Add("78380", "PAHANG;6-3-TML;KG TUALANG, TEMERLOH")
            statedict.Add("112017", "PAHANG;6-3-CH;PEKAN AWAH, TEMERLOH409")
            statedict.Add("78704", "PAHANG;6-1-GMB;BATU 16, JALAN GAMBANG")
            statedict.Add("111511", "PAHANG;6-1-CH;KG MELAYU, GAMBANG340")

            statedict.Add("78011", "TERENGGANU;5-3-BSR;PLANT 3, JABOR")
            statedict.Add("75847", "TERENGGANU;5-4-JBO;PLANT 4, JABOR")
            statedict.Add("75916", "TERENGGANU;4-3-CM3;DUNGUN")
            statedict.Add("99484", "TERENGGANU;4-7-T11;BRIDGE T118,DUNGUN8")
            statedict.Add("79138", "TERENGGANU;4-8-DSM;CHRONG SERDANG, PAKA PAKA")
            statedict.Add("111980", "TERENGGAN;5-1-CH2;CH237,KERTEH37U")
            statedict.Add("78317", "TERENGGANU;5-1-KER;KERTEH")
            statedict.Add("78747", "TERENGGANU;3-1-CH1;CHALOK02")
            statedict.Add("111918", "TERENGGAN;5-2-CH2;CUKAI69U")
            statedict.Add("79458", "TERENGGANU;5-2-MLG;MAK LAGAM, CUKAI")
            statedict.Add("111294", "TERENGGAN;5-2-CH2;KIJAL59U")
            statedict.Add("99729", "TERENGGANU;3-2-CH1;BELARA, SETIU23")
            statedict.Add("100239", "TERENGGAN;4-2-CM2;BKT PAK MERIAH, MARANGU")
            statedict.Add("100163", "TERENGGAN;3-4-CH1;BKT TINGGI SERDANG38U")
            statedict.Add("99748", "TERENGGANU;4-1-CM1;MARANG")
            statedict.Add("79185", "TERENGGANU;2-4-WGL;TEMBILA")

            statedict.Add("110818", "KELANTAN;1-3-JRS;BUKIT JAWA")
            statedict.Add("100543", "KELANTAN;2-1-PSP;PASIR PUTIH")
            statedict.Add("99485", "KELANTAN;1-1-TJG;TUNJONG")
            While dr.Read()
                If datastruct.ContainsKey(dr("shiptocode")) Then
                    shipcd = datastruct(dr("shiptocode"))
                    Select Case dr("shifts")
                        Case "11"
                            shipcd.s11 = dr("counts")
                        Case "12"
                            shipcd.s12 = dr("counts")
                        Case "21"
                            shipcd.s21 = dr("counts")
                        Case "22"
                            shipcd.s22 = dr("counts")
                        Case "31"
                            shipcd.s31 = dr("counts")
                        Case "32"
                            shipcd.s32 = dr("counts")
                    End Select
                    datastruct(dr("shiptocode")) = shipcd
                Else
                    shipcd = New shiptocode()
                    shipcd.shiptoid = dr("shiptocode")
                    Select Case dr("shifts")
                        Case "11"
                            shipcd.s11 = dr("counts")
                        Case "12"
                            shipcd.s12 = dr("counts")
                        Case "21"
                            shipcd.s21 = dr("counts")
                        Case "22"
                            shipcd.s22 = dr("counts")
                        Case "31"
                            shipcd.s31 = dr("counts")
                        Case "32"
                            shipcd.s32 = dr("counts")
                    End Select
                    datastruct.Add(dr("shiptocode"), shipcd)
                End If
            End While

            If Not dr.IsClosed() Then
                dr.Close()
            End If

            cmd.CommandText = "select * from dbo.fn_getecrlatasummary(@date) order by shiptocode"
            dr = cmd.ExecuteReader()
            While dr.Read()
                If datastruct.ContainsKey(dr("shiptocode")) Then
                    shipcd = datastruct(dr("shiptocode"))
                    Select Case dr("shifts")
                        Case "11"
                            shipcd.a11 = dr("counts")
                        Case "12"
                            shipcd.a12 = dr("counts")
                        Case "21"
                            shipcd.a21 = dr("counts")
                        Case "22"
                            shipcd.a22 = dr("counts")
                        Case "31"
                            shipcd.a31 = dr("counts")
                        Case "32"
                            shipcd.a32 = dr("counts")
                    End Select
                    datastruct(dr("shiptocode")) = shipcd
                Else
                    shipcd = New shiptocode()
                    shipcd.shiptoid = dr("shiptocode")
                    Select Case dr("shifts")
                        Case "11"
                            shipcd.a11 = dr("counts")
                        Case "12"
                            shipcd.a12 = dr("counts")
                        Case "21"
                            shipcd.a21 = dr("counts")
                        Case "22"
                            shipcd.a22 = dr("counts")
                        Case "31"
                            shipcd.a31 = dr("counts")
                        Case "32"
                            shipcd.a32 = dr("counts")
                    End Select
                    datastruct.Add(dr("shiptocode"), shipcd)
                End If
            End While


            Dim data1 As KeyValuePair(Of String, shiptocode)
            a = New ArrayList()
            a.Add("")
            a.Add("PAHANG")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            aa.Add(a)
            Dim counter1 As Integer = 0
            Dim infostr As String = ""
            Dim infos() As String
            Dim lastcounter As Integer = 0
            Dim ts11, ts12, ts1, ts21, ts22, ts2, ts31, ts32, ts3, gtotal As Integer
            ts11 = 0
            ts12 = 0
            ts1 = 0
            ts21 = 0
            ts22 = 0
            ts2 = 0
            ts31 = 0
            ts32 = 0
            ts3 = 0
            gtotal = 0
            For Each data1 In datastruct
                If statedict.ContainsKey(data1.Key) Then
                    infostr = statedict(data1.Key)
                    infos = infostr.Split(";")
                    If infos(0) = "PAHANG" Then
                        a = New ArrayList()
                        a.Add("")
                        If infos.Length >= 2 Then
                            a.Add(infos(1))
                            a.Add(infos(2))
                        End If
                        a.Add(data1.Key)
                        a.Add("Weightout")
                        a.Add(data1.Value.s11)
                        a.Add(data1.Value.s12)
                        a.Add(data1.Value.s11 + data1.Value.s12)
                        a.Add(data1.Value.s21)
                        a.Add(data1.Value.s22)
                        a.Add(data1.Value.s21 + data1.Value.s22)
                        a.Add(data1.Value.s31)
                        a.Add(data1.Value.s32)
                        a.Add(data1.Value.s31 + data1.Value.s32)
                        a.Add(data1.Value.s11 + data1.Value.s12 + data1.Value.s21 + data1.Value.s22 + data1.Value.s31 + data1.Value.s32)
                        ts11 += data1.Value.s11
                        ts12 += data1.Value.s12
                        ts1 += data1.Value.s11 + data1.Value.s12
                        ts21 += data1.Value.s21
                        ts22 += data1.Value.s22
                        ts2 += data1.Value.s21 + data1.Value.s22
                        ts31 += data1.Value.s31
                        ts32 += data1.Value.s32
                        ts3 += data1.Value.s31 + data1.Value.s32
                        gtotal += data1.Value.s11 + data1.Value.s12 + data1.Value.s21 + data1.Value.s22 + data1.Value.s31 + data1.Value.s32
                        aa.Add(a)
                        a = New ArrayList()
                        a.Add("")
                        a.Add("")
                        a.Add("")
                        a.Add("")
                        a.Add("ATA Trips")
                        a.Add(data1.Value.a11)
                        a.Add(data1.Value.a12)
                        a.Add(data1.Value.a11 + data1.Value.a12)
                        a.Add(data1.Value.a21)
                        a.Add(data1.Value.a22)
                        a.Add(data1.Value.a21 + data1.Value.a22)
                        a.Add(data1.Value.a31)
                        a.Add(data1.Value.a32)
                        a.Add(data1.Value.a31 + data1.Value.a32)
                        a.Add(data1.Value.a11 + data1.Value.a12 + data1.Value.a21 + data1.Value.a22 + data1.Value.a31 + data1.Value.a32)
                        ts11 += data1.Value.a11
                        ts12 += data1.Value.a12
                        ts1 += data1.Value.a11 + data1.Value.a12
                        ts21 += data1.Value.a21
                        ts22 += data1.Value.a22
                        ts2 += data1.Value.a21 + data1.Value.a22
                        ts31 += data1.Value.a31
                        ts32 += data1.Value.a32
                        ts3 += data1.Value.a31 + data1.Value.a32
                        gtotal += data1.Value.a11 + data1.Value.a12 + data1.Value.a21 + data1.Value.a22 + data1.Value.a31 + data1.Value.a32
                        aa.Add(a)
                    End If
                Else

                End If
            Next




            a = New ArrayList()
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("Total")
            a.Add(ts11)
            a.Add(ts12)
            a.Add(ts1)
            a.Add(ts21)
            a.Add(ts22)
            a.Add(ts2)
            a.Add(ts31)
            a.Add(ts32)
            a.Add(ts3)
            a.Add(gtotal)
            aa.Add(a)


            a = New ArrayList()
            a.Add("")
            a.Add("TERENGGANU")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            aa.Add(a)
            ts11 = 0
            ts12 = 0
            ts1 = 0
            ts21 = 0
            ts22 = 0
            ts2 = 0
            ts31 = 0
            ts32 = 0
            ts3 = 0
            gtotal = 0
            For Each data1 In datastruct
                If statedict.ContainsKey(data1.Key) Then
                    infostr = statedict(data1.Key)
                    infos = infostr.Split(";")
                    If infos(0) = "TERENGGANU" Then
                        a = New ArrayList()
                        a.Add("")
                        If infos.Length >= 2 Then
                            a.Add(infos(1))
                            a.Add(infos(2))
                        End If
                        a.Add(data1.Key)
                        a.Add("Weightout")
                        a.Add(data1.Value.s11)
                        a.Add(data1.Value.s12)
                        a.Add(data1.Value.s11 + data1.Value.s12)
                        a.Add(data1.Value.s21)
                        a.Add(data1.Value.s22)
                        a.Add(data1.Value.s21 + data1.Value.s22)
                        a.Add(data1.Value.s31)
                        a.Add(data1.Value.s32)
                        a.Add(data1.Value.s31 + data1.Value.s32)
                        a.Add(data1.Value.s11 + data1.Value.s12 + data1.Value.s21 + data1.Value.s22 + data1.Value.s31 + data1.Value.s32)
                        aa.Add(a)
                        ts11 += data1.Value.s11
                        ts12 += data1.Value.s12
                        ts1 += data1.Value.s11 + data1.Value.s12
                        ts21 += data1.Value.s21
                        ts22 += data1.Value.s22
                        ts2 += data1.Value.s21 + data1.Value.s22
                        ts31 += data1.Value.s31
                        ts32 += data1.Value.s32
                        ts3 += data1.Value.s31 + data1.Value.s32
                        gtotal += data1.Value.s11 + data1.Value.s12 + data1.Value.s21 + data1.Value.s22 + data1.Value.s31 + data1.Value.s32
                        a = New ArrayList()
                        a.Add("")
                        a.Add("")
                        a.Add("")
                        a.Add("")
                        a.Add("ATA Trips")
                        a.Add(data1.Value.a11)
                        a.Add(data1.Value.a12)
                        a.Add(data1.Value.a11 + data1.Value.a12)
                        a.Add(data1.Value.a21)
                        a.Add(data1.Value.a22)
                        a.Add(data1.Value.a21 + data1.Value.a22)
                        a.Add(data1.Value.a31)
                        a.Add(data1.Value.a32)
                        a.Add(data1.Value.a31 + data1.Value.a32)
                        a.Add(data1.Value.a11 + data1.Value.a12 + data1.Value.a21 + data1.Value.a22 + data1.Value.a31 + data1.Value.a32)
                        ts11 += data1.Value.a11
                        ts12 += data1.Value.a12
                        ts1 += data1.Value.a11 + data1.Value.a12
                        ts21 += data1.Value.a21
                        ts22 += data1.Value.a22
                        ts2 += data1.Value.a21 + data1.Value.a22
                        ts31 += data1.Value.a31
                        ts32 += data1.Value.a32
                        ts3 += data1.Value.a31 + data1.Value.a32
                        gtotal += data1.Value.a11 + data1.Value.a12 + data1.Value.a21 + data1.Value.a22 + data1.Value.a31 + data1.Value.a32
                        aa.Add(a)
                    End If
                End If
            Next

            a = New ArrayList()
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("Total")
            a.Add(ts11)
            a.Add(ts12)
            a.Add(ts1)
            a.Add(ts21)
            a.Add(ts22)
            a.Add(ts2)
            a.Add(ts31)
            a.Add(ts32)
            a.Add(ts3)
            a.Add(gtotal)
            aa.Add(a)

            a = New ArrayList()
            a.Add("")
            a.Add("KELANTAN")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            aa.Add(a)
            ts11 = 0
            ts12 = 0
            ts1 = 0
            ts21 = 0
            ts22 = 0
            ts2 = 0
            ts31 = 0
            ts32 = 0
            ts3 = 0
            gtotal = 0
            For Each data1 In datastruct
                If statedict.ContainsKey(data1.Key) Then
                    infostr = statedict(data1.Key)
                    infos = infostr.Split(";")
                    If infos(0) = "KELANTAN" Then
                        a = New ArrayList()
                        a.Add("")
                        If infos.Length >= 2 Then
                            a.Add(infos(1))
                            a.Add(infos(2))
                        End If
                        a.Add(data1.Key)
                        a.Add("Weightout")
                        a.Add(data1.Value.s11)
                        a.Add(data1.Value.s12)
                        a.Add(data1.Value.s11 + data1.Value.s12)
                        a.Add(data1.Value.s21)
                        a.Add(data1.Value.s22)
                        a.Add(data1.Value.s21 + data1.Value.s22)
                        a.Add(data1.Value.s31)
                        a.Add(data1.Value.s32)
                        a.Add(data1.Value.s31 + data1.Value.s32)
                        a.Add(data1.Value.s11 + data1.Value.s12 + data1.Value.s21 + data1.Value.s22 + data1.Value.s31 + data1.Value.s32)
                        aa.Add(a)
                        ts11 += data1.Value.s11
                        ts12 += data1.Value.s12
                        ts1 += data1.Value.s11 + data1.Value.s12
                        ts21 += data1.Value.s21
                        ts22 += data1.Value.s22
                        ts2 += data1.Value.s21 + data1.Value.s22
                        ts31 += data1.Value.s31
                        ts32 += data1.Value.s32
                        ts3 += data1.Value.s31 + data1.Value.s32
                        gtotal += data1.Value.s11 + data1.Value.s12 + data1.Value.s21 + data1.Value.s22 + data1.Value.s31 + data1.Value.s32
                        a = New ArrayList()
                        a.Add("")
                        a.Add("")
                        a.Add("")
                        a.Add("")
                        a.Add("ATA Trips")
                        a.Add(data1.Value.a11)
                        a.Add(data1.Value.a12)
                        a.Add(data1.Value.a11 + data1.Value.a12)
                        a.Add(data1.Value.a21)
                        a.Add(data1.Value.a22)
                        a.Add(data1.Value.a21 + data1.Value.a22)
                        a.Add(data1.Value.a31)
                        a.Add(data1.Value.a32)
                        a.Add(data1.Value.a31 + data1.Value.a32)
                        a.Add(data1.Value.a11 + data1.Value.a12 + data1.Value.a21 + data1.Value.a22 + data1.Value.a31 + data1.Value.a32)
                        ts11 += data1.Value.a11
                        ts12 += data1.Value.a12
                        ts1 += data1.Value.a11 + data1.Value.a12
                        ts21 += data1.Value.a21
                        ts22 += data1.Value.a22
                        ts2 += data1.Value.a21 + data1.Value.a22
                        ts31 += data1.Value.a31
                        ts32 += data1.Value.a32
                        ts3 += data1.Value.a31 + data1.Value.a32
                        gtotal += data1.Value.a11 + data1.Value.a12 + data1.Value.a21 + data1.Value.a22 + data1.Value.a31 + data1.Value.a32
                        aa.Add(a)
                    End If
                End If
            Next

            a = New ArrayList()
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("")
            a.Add("Total")
            a.Add(ts11)
            a.Add(ts12)
            a.Add(ts1)
            a.Add(ts21)
            a.Add(ts22)
            a.Add(ts2)
            a.Add(ts31)
            a.Add(ts32)
            a.Add(ts3)
            a.Add(gtotal)
            aa.Add(a)

            Dim t As New DataTable
            t.Columns.Add(New DataColumn("S NO"))
            t.Columns.Add(New DataColumn("Area Code"))
            t.Columns.Add(New DataColumn("Location"))
            t.Columns.Add(New DataColumn("ShipToCode"))
            t.Columns.Add(New DataColumn(""))
            t.Columns.Add(New DataColumn("07: 00 - 11:00"))
            t.Columns.Add(New DataColumn("11:00 - 15:00"))
            t.Columns.Add(New DataColumn("Total1"))
            t.Columns.Add(New DataColumn("15:00 - 19:00"))
            t.Columns.Add(New DataColumn("19:00 - 23:00"))
            t.Columns.Add(New DataColumn("Total2"))
            t.Columns.Add(New DataColumn("23:00 - 03:00"))
            t.Columns.Add(New DataColumn("03:00 - 06:59"))
            t.Columns.Add(New DataColumn("Total3"))
            t.Columns.Add(New DataColumn("Total"))
            Try
                Dim r As DataRow
                For Each a In aa
                    r = t.NewRow()
                    For i As Integer = 0 To 14
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


    Structure shiptocode
        Dim shiptoid As String
        Dim area As String
        Dim location As String
        Dim s11 As Int32
        Dim s12 As Int32
        Dim total1 As Int32
        Dim s21 As Int32
        Dim s22 As Int32
        Dim total2 As Int32
        Dim s31 As Int32
        Dim s32 As Int32
        Dim total3 As Int32
        Dim a11 As Int32
        Dim a12 As Int32
        Dim totalata1 As Int32
        Dim a21 As Int32
        Dim a22 As Int32
        Dim totalata2 As Int32
        Dim a31 As Int32
        Dim a32 As Int32
        Dim totalata3 As Int32
    End Structure

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
                cmd.CommandText = "select userid ,username  from userTBL where userid  not in (7144,7145,7146,7147,7148,7099,7180) and companyname  like 'ytl%' and role ='User' order by username"
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
    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Xml)>
    Public Shared Function GetRecentRemarks(ByVal plateno As String) As String
        Dim conn As SqlConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("sqlserverconnection"))
        Dim aa As New ArrayList()
        Dim a As ArrayList
        Try
            Dim cmd As New SqlCommand("select top 5 timestamp ,sourcename ,officeremark  from maintenance where plateno =@plateno and  status ='OSS' order by timestamp desc  ", conn)
            cmd.Parameters.AddWithValue("@plateno", plateno)
            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            Dim counter As Integer = 1
            While dr.Read()
                a = New ArrayList()
                a.Add(counter)
                a.Add(Convert.ToDateTime(dr("timestamp")).ToString("yyyy/MM/dd HH:mm:ss"))
                a.Add(dr("sourcename"))
                a.Add(dr("officeremark"))
                aa.Add(a)
                counter += 1
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
