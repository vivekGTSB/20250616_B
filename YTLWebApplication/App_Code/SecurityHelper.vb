Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Web.Security
Imports System.Security.Cryptography
Imports System.Text

Public Class SecurityHelper
    
    ' Input validation and sanitization
    Public Shared Function ValidateInput(input As String, inputType As String) As Boolean
        If String.IsNullOrEmpty(input) Then
            Return False
        End If
        
        Select Case inputType.ToLower()
            Case "email"
                Return Regex.IsMatch(input, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            Case "alphanumeric"
                Return Regex.IsMatch(input, "^[a-zA-Z0-9]+$")
            Case "numeric"
                Return Regex.IsMatch(input, "^[0-9]+$")
            Case "plateno"
                Return Regex.IsMatch(input, "^[A-Z0-9\-]+$") AndAlso input.Length <= 20
            Case "username"
                Return Regex.IsMatch(input, "^[a-zA-Z0-9_]+$") AndAlso input.Length <= 50
            Case "date"
                Dim dateValue As DateTime
                Return DateTime.TryParse(input, dateValue)
            Case Else
                Return False
        End Select
    End Function
    
    ' SQL injection prevention
    Public Shared Function SanitizeForSql(input As String) As String
        If String.IsNullOrEmpty(input) Then
            Return String.Empty
        End If
        
        ' Remove dangerous characters
        input = input.Replace("'", "''")
        input = input.Replace(";", "")
        input = input.Replace("--", "")
        input = input.Replace("/*", "")
        input = input.Replace("*/", "")
        input = input.Replace("xp_", "")
        input = input.Replace("sp_", "")
        
        Return input
    End Function
    
    ' XSS prevention
    Public Shared Function SanitizeForHtml(input As String) As String
        If String.IsNullOrEmpty(input) Then
            Return String.Empty
        End If
        
        input = HttpUtility.HtmlEncode(input)
        input = input.Replace("javascript:", "")
        input = input.Replace("vbscript:", "")
        input = input.Replace("onload=", "")
        input = input.Replace("onerror=", "")
        input = input.Replace("onclick=", "")
        
        Return input
    End Function
    
    ' Create parameterized SQL command
    Public Shared Function CreateSafeCommand(query As String, connection As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand(query, connection)
        cmd.CommandTimeout = 30
        Return cmd
    End Function
    
    ' Password hashing
    Public Shared Function HashPassword(password As String) As String
        Using sha256Hash As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password))
            Return Convert.ToBase64String(bytes)
        End Using
    End Function
    
    ' Session validation
    Public Shared Function ValidateSession() As Boolean
        If HttpContext.Current.Session Is Nothing OrElse 
           HttpContext.Current.Session("userid") Is Nothing Then
            Return False
        End If
        
        ' Check session timeout
        If HttpContext.Current.Session.Timeout < 30 Then
            HttpContext.Current.Session.Timeout = 30
        End If
        
        Return True
    End Function
    
    ' CSRF token generation and validation
    Public Shared Function GenerateCSRFToken() As String
        Dim token As String = Guid.NewGuid().ToString()
        HttpContext.Current.Session("CSRFToken") = token
        Return token
    End Function
    
    Public Shared Function ValidateCSRFToken(token As String) As Boolean
        If HttpContext.Current.Session("CSRFToken") Is Nothing Then
            Return False
        End If
        
        Return HttpContext.Current.Session("CSRFToken").ToString() = token
    End Function
    
    ' Rate limiting helper
    Public Shared Function CheckRateLimit(identifier As String, maxAttempts As Integer, timeWindow As TimeSpan) As Boolean
        Dim cacheKey As String = "RateLimit_" & identifier
        Dim attempts As Object = HttpContext.Current.Cache(cacheKey)
        
        If attempts Is Nothing Then
            HttpContext.Current.Cache.Insert(cacheKey, 1, Nothing, DateTime.Now.Add(timeWindow), TimeSpan.Zero)
            Return True
        End If
        
        Dim currentAttempts As Integer = CInt(attempts)
        If currentAttempts >= maxAttempts Then
            Return False
        End If
        
        HttpContext.Current.Cache(cacheKey) = currentAttempts + 1
        Return True
    End Function
    
    ' Log security events
    Public Shared Sub LogSecurityEvent(eventType As String, details As String, Optional userId As String = "")
        Try
            Dim logEntry As String = String.Format("{0} - {1} - User: {2} - {3}", 
                                                 DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 
                                                 eventType, 
                                                 userId, 
                                                 details)
            
            ' Log to file or database
            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/App_Data/security.log"), logEntry & Environment.NewLine)
        Catch ex As Exception
            ' Silent fail for logging
        End Try
    End Sub
    
End Class