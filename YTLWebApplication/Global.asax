<%@ Application Language="VB" %>

<script runat="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        SecurityHelper.LogSecurityEvent("APPLICATION_START", "Application started")
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
        SecurityHelper.LogSecurityEvent("APPLICATION_END", "Application ended")
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
        Dim ex As Exception = Server.GetLastError()
        If ex IsNot Nothing Then
            SecurityHelper.LogSecurityEvent("APPLICATION_ERROR", "Unhandled exception: " & ex.Message)
        End If
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        Session.Timeout = 30
        SecurityHelper.LogSecurityEvent("SESSION_START", "New session started")
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends
        SecurityHelper.LogSecurityEvent("SESSION_END", "Session ended")
    End Sub
    
    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Security headers and request validation
        Response.Headers.Remove("Server")
        
        ' Validate request size
        If Request.ContentLength > 4194304 Then ' 4MB limit
            Response.StatusCode = 413
            Response.End()
        End If
        
        ' Block suspicious requests
        Dim userAgent As String = Request.UserAgent
        If String.IsNullOrEmpty(userAgent) OrElse 
           userAgent.ToLower().Contains("sqlmap") OrElse
           userAgent.ToLower().Contains("nikto") OrElse
           userAgent.ToLower().Contains("nessus") Then
            SecurityHelper.LogSecurityEvent("SUSPICIOUS_REQUEST", "Blocked suspicious user agent: " & userAgent)
            Response.StatusCode = 403
            Response.End()
        End If
    End Sub
    
    Sub Application_PreSendRequestHeaders(ByVal sender As Object, ByVal e As EventArgs)
        ' Remove server information
        Response.Headers.Remove("Server")
    End Sub

</script>