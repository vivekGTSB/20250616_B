Partial Public Class ErrorPage
    Inherits System.Web.UI.Page
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Log the error details
        Dim ex As Exception = Server.GetLastError()
        If ex IsNot Nothing Then
            SecurityHelper.LogSecurityEvent("ERROR_PAGE", "Error page accessed: " & ex.Message)
        End If
        
        ' Clear the error
        Server.ClearError()
    End Sub
    
End Class