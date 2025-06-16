Imports System.Data.SqlClient

Partial Public Class Login
    Inherits System.Web.UI.Page
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ' Generate CSRF token
            hdnCSRFToken.Value = SecurityHelper.GenerateCSRFToken()
            
            ' Clear any existing authentication
            AuthenticationHelper.LogoutUser()
            
            ' Check if user is already authenticated
            If AuthenticationHelper.IsUserAuthenticated() Then
                Response.Redirect("~/Dashboard.aspx")
            End If
        End If
    End Sub
    
    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Try
            ' Validate CSRF token
            If Not SecurityHelper.ValidateCSRFToken(hdnCSRFToken.Value) Then
                ShowError("Invalid request. Please try again.")
                SecurityHelper.LogSecurityEvent("CSRF_VIOLATION", "Invalid CSRF token", txtUsername.Text)
                Return
            End If
            
            ' Validate input
            Dim username As String = txtUsername.Text.Trim()
            Dim password As String = txtPassword.Text
            
            If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
                ShowError("Please enter both username and password.")
                Return
            End If
            
            ' Additional input validation
            If Not SecurityHelper.ValidateInput(username, "username") Then
                ShowError("Invalid username format.")
                SecurityHelper.LogSecurityEvent("INVALID_USERNAME_FORMAT", "Username contains invalid characters", username)
                Return
            End If
            
            If password.Length < 6 OrElse password.Length > 100 Then
                ShowError("Invalid password length.")
                Return
            End If
            
            ' Attempt authentication
            If AuthenticationHelper.AuthenticateUser(username, password) Then
                ' Regenerate session ID for security
                Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))
                
                ' Redirect to dashboard
                Response.Redirect("~/Dashboard.aspx", False)
            Else
                ShowError("Invalid username or password.")
                
                ' Clear password field
                txtPassword.Text = ""
                
                ' Add delay to prevent brute force attacks
                System.Threading.Thread.Sleep(2000)
            End If
            
        Catch ex As Exception
            SecurityHelper.LogSecurityEvent("LOGIN_EXCEPTION", "Exception during login: " & ex.Message, txtUsername.Text)
            ShowError("An error occurred during login. Please try again.")
        Finally
            ' Always clear password from memory
            txtPassword.Text = ""
        End Try
    End Sub
    
    Private Sub ShowError(message As String)
        lblError.Text = SecurityHelper.SanitizeForHtml(message)
        lblError.Visible = True
    End Sub
    
End Class