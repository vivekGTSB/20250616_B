Imports System.Text
Imports System.Data


Namespace AVLS

    Partial Class ExcelReport_new
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
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'If Session("login") = Nothing Then
            '    Response.Redirect("Login.aspx")
            'End If

            '' This excel special for vehicle management page.

            Dim columncount As Byte = 8

            Dim title As String = Request.QueryString("title")
            Dim plateno As String = Request.QueryString("plateno")
            Dim Type As String = Request.QueryString("Type")

            Dim Totaltank1 As String = Request.QueryString("Totaltank1")
            Dim Totaltank2 As String = Request.QueryString("Totaltank2")
            Dim TotalWithOutTank As String = Request.QueryString("TotalWithOutTank")
            Dim TotalPTO As String = Request.QueryString("TotalPTO")
            Dim TotalVehicle As String = Request.QueryString("TotalVehicle")

            Dim totalExistingVehicle As String = Request.QueryString("totalExistingVehicle")
            Dim totalNewInstallation As String = Request.QueryString("totalNewInstallation")
            Dim totalUnbill As String = Request.QueryString("totalUnbill")
            Dim comName As String = Request.QueryString("comName")

            'And Not Session("exceltable2") Is Nothing

            Dim temp As New DataTable
            temp.Columns.Add(New DataColumn("chk"))
            temp.Columns.Add(New DataColumn("no"))
            temp.Columns.Add(New DataColumn("username"))
            temp.Columns.Add(New DataColumn("plateno"))
            temp.Columns.Add(New DataColumn("plate no"))
            temp.Columns.Add(New DataColumn("unitid"))
            temp.Columns.Add(New DataColumn("type"))
            temp.Columns.Add(New DataColumn("color"))
            temp.Columns.Add(New DataColumn("model"))
            temp.Columns.Add(New DataColumn("brand"))
            temp.Columns.Add(New DataColumn("groupname"))
            temp.Columns.Add(New DataColumn("speed"))
            temp.Columns.Add(New DataColumn("tank1"))
            temp.Columns.Add(New DataColumn("tank2"))
            temp.Columns.Add(New DataColumn("portno"))
            temp.Columns.Add(New DataColumn("Immobilizer"))
            temp.Columns.Add(New DataColumn("installdate"))
            temp.Columns.Add(New DataColumn("Tank of Level Sensor"))
            temp.Columns.Add(New DataColumn("PTO"))
            temp.Columns.Add(New DataColumn("WeightSensor"))
            temp.Columns.Add(New DataColumn("Billing Type"))
            temp.Columns.Add(New DataColumn("installdate2"))
            'If Not Session("exceltable") Is Nothing Then

            Dim table As New DataTable
            If Type = 1 Then
                temp = Session("exceltable")
                Try
                    temp.Columns.Remove("chk")
                    temp.Columns.Remove("plateno")
                    temp.Columns.Remove("Tank of Level Sensor")
                    temp.Columns.Remove("PTO")
                    temp.Columns.Remove("installdate2")
                Catch ex As Exception

                End Try

                table = temp
            ElseIf Type = 2 Then
                temp = Session("exceltable2")
                Try
                    temp.Columns.Remove("chk")
                    temp.Columns.Remove("plateno")
                    temp.Columns.Remove("unitid")
                    temp.Columns.Remove("type")
                    temp.Columns.Remove("color")
                    temp.Columns.Remove("model")
                    temp.Columns.Remove("brand")
                    temp.Columns.Remove("speed")
                    temp.Columns.Remove("tank1")
                    temp.Columns.Remove("tank2")
                    temp.Columns.Remove("portno")
                    temp.Columns.Remove("Immobilizer")
                    temp.Columns.Remove("WeightSensor")
                    temp.Columns.Remove("installdate2")
                Catch ex As Exception

                End Try

                table = temp
            End If


            columncount = table.Columns.Count
            If title = "Vehicle Log Report" Then
                columncount = columncount - 1
            ElseIf title = "Vehicle Idling Report" Then
                columncount = columncount - 1
            End If

            Response.Write("<table>")

            If Type = 2 Then
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'> Company Name : " & comName & "</b></td></tr>")
            End If

            Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'>" & title & "</b></td></tr>")

            If Type = 2 Then
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'>" & "Total Vehicle :" & TotalVehicle.Replace(",", "") & "</b></td></tr>")
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'>" & "Total Tank 1: " & Totaltank1 & "</b></td></tr>")
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'>" & "Total Tank 2: " & Totaltank2 & "</b></td></tr>")
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'>" & "Total Without Tank: " & TotalWithOutTank & "</b></td></tr>")
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'>" & "Total PTO: " & TotalPTO & "</b></td></tr>")
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'>" & "Total Existing Vehicle: " & totalExistingVehicle & "</b></td></tr>")
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'>" & "Total New Installation: " & totalNewInstallation & "</b></td></tr>")
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b style='font-family:Verdana;font-size:20px;color:#465AE8;'>" & "Total Unbill Vehicle: " & totalUnbill & "</b></td></tr>")
            End If

            Response.Write("<tr><td colspan='" & columncount & "'></td></tr>")
            If plateno <> Nothing Then
                Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Vehicle Plate Number : </b>" & plateno & "</td></tr>")
            End If
            Response.Write("<tr><td colspan='" & columncount & "' align='left'><b>Report Date : </b> '" & DateTime.Now.ToString("yyyy/MM/dd H:mm:ss tt") & "</td></tr>")

            Response.Write("<tr><td colspan='" & columncount & "'></td></tr>")


            Response.Write("<tr>")
            For j As Int32 = 0 To columncount - 1
                Response.Write("<th style='background-color: #465AE8; color: #FFFFFF';border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid>" & table.Columns(j).Caption & "</th>")
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
                        Response.Write("<td style='border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid'>" & table.Rows(j).Item((i)).ToString() & "</td>")
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
            'End If

            'If Not Session("exceltable2") Is Nothing And Type = 2 Then
            '    Dim table As New DataTable
            '    table = Session("exceltable2")

            '    columncount = table.Columns.Count

            '    Response.Write("<br/>")
            '    Response.Write("<br/>")
            '    Response.Write("<table border='1'>")

            '    Response.Write("<tr>")
            '    For j As Int32 = 0 To table.Columns.Count - 1
            '        Response.Write("<th style='background-color: #465AE8; color: #FFFFFF'>" & table.Columns(j).Caption & "</th>")
            '    Next
            '    Response.Write("</tr>")


            '    For j As Int32 = 0 To table.Rows.Count - 1
            '        Response.Write("<tr>")

            '        For i As Int32 = 0 To table.Columns.Count - 1
            '            Response.Write("<td >" & table.Rows(j).Item((i)).ToString() & "</td>") 'style='background-color: #FFFFE1;'
            '        Next

            '        Response.Write("</tr>")
            '    Next
            '    Response.Write("</table>")
            'End If

            'If Not Session("exceltable3") Is Nothing Then
            '    Dim table As New DataTable
            '    table = Session("exceltable3")

            '    columncount = table.Columns.Count

            '    Response.Write("<br/>")
            '    Response.Write("<br/>")
            '    Response.Write("<table border='1'>")

            '    Response.Write("<tr>")
            '    For j As Int32 = 0 To table.Columns.Count - 1
            '        Response.Write("<th style='background-color: #465AE8; color: #FFFFFF'>" & table.Columns(j).Caption & "</th>")
            '    Next
            '    Response.Write("</tr>")


            '    For j As Int32 = 0 To table.Rows.Count - 1
            '        Response.Write("<tr>")

            '        For i As Int32 = 0 To table.Columns.Count - 1
            '            Response.Write("<td >" & table.Rows(j).Item((i)).ToString() & "</td>") 'style='background-color: #FFFFE1;'
            '        Next

            '        Response.Write("</tr>")
            '    Next
            '    Response.Write("</table>")
            'End If

            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & title & ".xls;")

        End Sub
    End Class
End Namespace
