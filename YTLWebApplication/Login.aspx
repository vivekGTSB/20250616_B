<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="YTLWebApplication.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>YTL Fleet Management - Login</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="robots" content="noindex, nofollow" />
    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }
        .login-container {
            max-width: 400px;
            margin: 100px auto;
            background: white;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        .login-header {
            text-align: center;
            margin-bottom: 30px;
            color: #333;
        }
        .form-group {
            margin-bottom: 20px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
            color: #555;
        }
        .form-group input {
            width: 100%;
            padding: 12px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
            box-sizing: border-box;
        }
        .form-group input:focus {
            outline: none;
            border-color: #465ae8;
            box-shadow: 0 0 5px rgba(70, 90, 232, 0.3);
        }
        .login-button {
            width: 100%;
            padding: 12px;
            background-color: #465ae8;
            color: white;
            border: none;
            border-radius: 4px;
            font-size: 16px;
            cursor: pointer;
            transition: background-color 0.3s;
        }
        .login-button:hover {
            background-color: #3a4bc8;
        }
        .error-message {
            color: #d32f2f;
            margin-top: 10px;
            text-align: center;
            font-size: 14px;
        }
        .security-notice {
            font-size: 12px;
            color: #666;
            text-align: center;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-header">
                <h2>YTL Fleet Management</h2>
                <p>Please sign in to continue</p>
            </div>
            
            <asp:HiddenField ID="hdnCSRFToken" runat="server" />
            
            <div class="form-group">
                <label for="txtUsername">Username:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" 
                           MaxLength="50" autocomplete="username" required="true" />
            </div>
            
            <div class="form-group">
                <label for="txtPassword">Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" 
                           CssClass="form-control" MaxLength="100" autocomplete="current-password" required="true" />
            </div>
            
            <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="login-button" OnClick="btnLogin_Click" />
            
            <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
            
            <div class="security-notice">
                This is a secure system. All activities are logged and monitored.
            </div>
        </div>
    </form>
    
    <script type="text/javascript">
        // Prevent form resubmission
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }
        
        // Clear password field on page load
        window.onload = function() {
            document.getElementById('<%= txtPassword.ClientID %>').value = '';
        };
    </script>
</body>
</html>