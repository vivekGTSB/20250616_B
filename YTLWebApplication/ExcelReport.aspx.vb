Imports System.Text
Imports System.Data


Namespace AVLS

    Partial Class ExcelReport
        Inherits System.Web.UI.Page

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
        Public sbrHTML As StringBuilder
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load

            'If Session("login") = Nothing Then
            '    Response.Redirect("Login.aspx")
            'End If

            Dim columncount As Byte = 8

            Dim title As String = Request.QueryString("title")
            Dim plateno As String = Request.QueryString("plateno")
            Dim reportperiod As String = Request.QueryString("rperoid")
            Dim username As String = Request.QueryString("username")
            Dim reporttype As String = Request.QueryString("reporttype")
            Dim otrack As String = Request.QueryString("otrack")
            Dim internal As String = Request.QueryString("internal")
            Dim external As String = Request.QueryString("external")
            Dim trips As String = Request.QueryString("trips")
            Dim tonnage As String = Request.QueryString("tonnage")

            If Not Session("exceltable") Is Nothing Then

                Dim table As New DataTable
                table = Session("exceltable")

                columncount = table.Columns.Count
                If title = "Vehicle Log Report" Then
                    columncount = columncount - 1
                ElseIf title = "Vehicle Idling Report" Then
                    columncount = columncount - 1
                ElseIf title = "Vehicle Speed Report" Then
                    columncount = columncount - 1
                ElseIf title = "Vehicle_Speed_Report" Then
                    columncount = columncount - 1
                ElseIf title = "Vehicle Harsh Breaking Report" Then
                    columncount = columncount - 1
                ElseIf title = "Vehicle Geofence Report" Then
                    columncount = columncount - 1
                ElseIf title = "Vehicle_Power_Cut_Events_Report" Or title = "Vehicle_All_Events_Report" Or title = "Vehicle_Panic_Events_Report" Or title = "Vehicle_Geofence_Events_Report" Or title = "Vehicle_Immobilizer_Events_Report" Then
                    columncount = columncount - 1
                End If
                Response.Write("<table>")
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'>" & title & "</b></td></tr>")
                Response.Write("<tr><td colspan='" & columncount & "'></td></tr>")
                If username <> Nothing Then
                    Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>UserName : </b>" & username & "</td></tr>")
                End If
                If plateno <> Nothing Then
                    If title = "Vehicles Violation Weekly Summary Report" Then
                        Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Report Period  : </b>" & plateno & "</td></tr>")
                    Else
                        Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Vehicle Plate Number : </b>" & plateno & "</td></tr>")
                    End If
                End If
                If reporttype = "1" Then
                    Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Report Period  : </b>" & reportperiod & "</td></tr>")
                End If

                If Not Request.QueryString("internal") Is Nothing Then
                    Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Trips Loaded/ Total Order Trip  : </b>" & trips & "</td></tr>")
                    Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Tonnage Loaded/Order Tonnage  : </b>" & tonnage & "</td></tr>")
                    Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Hours to Dateline  : </b>" & otrack & "</td></tr>")
                End If
                If Not Session("internaltruck") Is Nothing Then
                    Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Internal Trucks  : </b>" & Session("internaltruck") & "</td></tr>")
                End If
                If Not Session("externaltruck") Is Nothing Then
                    Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>External Trucks  : </b>" & Session("externaltruck") & "</td></tr>")
                End If
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Report Date : </b>" & DateTime.Now & "</td></tr>")
                Response.Write("<tr><td colspan='" & columncount & "'></td></tr>")
                If Not Session("OVMonthlyAllAng") Is Nothing Then
                    Response.Write("<tr><td colspan='" & columncount & "' align='left' style='color: #FF0000'><b>" & Session("OVMonthlyAllAng") & "</b></td></tr>")
                    Response.Write("<tr><td colspan='" & columncount & "'></td></tr>")
                    Session("OVMonthlyAllAng") = ""
                End If

                Response.Write("<tr>")
                For j As Int32 = 0 To columncount - 1

                    If table.Columns(j).Caption = "Start Location1" Then
                        Response.Write("<th style='background-color: #465AE8; color: #FFFFFF';border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid>" & " Start Location" & "</th>")
                    ElseIf table.Columns(j).Caption = "End Location1" Then
                        Response.Write("<th style='background-color: #465AE8; color: #FFFFFF';border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid>" & "End Location" & "</th>")

                    Else
                        Response.Write("<th style='background-color: #465AE8; color: #FFFFFF';border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid>" & table.Columns(j).Caption & "</th>")

                    End If
                Next
                Response.Write("</tr>")

                Dim totalRow As Boolean = False

                For j As Int32 = 0 To table.Rows.Count - 1
                    Response.Write("<tr>")

                    For i As Int32 = 0 To columncount - 1

                        If table.Rows(j).Item(i).ToString() = "TOTAL" Or table.Rows(j).Item(i).ToString() = "" Then
                            totalRow = True
                        End If
                        If totalRow = True Then
                            Response.Write("<td style='background-color:#FFA280;border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid'>" & table.Rows(j).Item((i)).ToString() & "</td>")
                        Else
                            Response.Write("<td style='border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid'>" & table.Rows(j).Item((i)).ToString() & "</td>") 'background-color: #FFFFE1;
                        End If
                    Next
                    totalRow = False
                    Response.Write("</tr>")
                Next

                'Response.Write("<tr><td colspan='" & columncount & "'></td></tr>")

                'Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Total Number of Records : " & table.Rows.Count & "</b></td></tr>")
                Response.Write("</table>")
            End If


            If Not Session("exceltable2") Is Nothing Then
                Dim table As New DataTable
                table = Session("exceltable2")

                columncount = table.Columns.Count

                Response.Write("<br/>")
                Response.Write("<br/>")
                Response.Write("<table border='1'>")

                Response.Write("<tr>")
                For j As Int32 = 0 To table.Columns.Count - 1
                    If table.Columns(j).Caption = "Location Name" Then
                        Response.Write("<th style='background-color: #465AE8; color: #FFFFFF'>" & "Address" & "</th>")
                    Else
                        Response.Write("<th style='background-color: #465AE8; color: #FFFFFF'>" & table.Columns(j).Caption & "</th>")
                    End If

                Next
                Response.Write("</tr>")


                For j As Int32 = 0 To table.Rows.Count - 1
                    Response.Write("<tr>")

                    For i As Int32 = 0 To table.Columns.Count - 1
                        Response.Write("<td >" & table.Rows(j).Item((i)).ToString() & "</td>") 'style='background-color: #FFFFE1;'


                    Next

                    Response.Write("</tr>")
                Next
                Response.Write("</table>")
            End If

            If Not Session("exceltable3") Is Nothing Then
                Dim table As New DataTable
                table = Session("exceltable3")

                columncount = table.Columns.Count

                Response.Write("<br/>")
                Response.Write("<br/>")
                Response.Write("<table border='1'>")

                Response.Write("<tr>")
                For j As Int32 = 0 To table.Columns.Count - 1
                    Response.Write("<th style='background-color: #465AE8; color: #FFFFFF'>" & table.Columns(j).Caption & "</th>")
                Next
                Response.Write("</tr>")


                For j As Int32 = 0 To table.Rows.Count - 1
                    Response.Write("<tr>")

                    For i As Int32 = 0 To table.Columns.Count - 1
                        Response.Write("<td >" & table.Rows(j).Item((i)).ToString() & "</td>") 'style='background-color: #FFFFE1;'
                    Next

                    Response.Write("</tr>")
                Next
                Response.Write("</table>")
            End If


            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & CStr(title & "_" & plateno & "_" & Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy/MM/dd")).Replace(" ", "_") & ".xls;")

            Session.Remove("exceltable")
            Session.Remove("exceltable2")
            Session.Remove("exceltable3")

        End Sub
    End Class
End Namespace
