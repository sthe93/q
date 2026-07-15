<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Login.aspx.cs" Inherits="QualificationVerificationWeb.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UJ Qualification Verification - Login</title>
    <link rel="icon" href="https://www.uj.ac.za/favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Admin/Content/bootstrap/font-awesome/css/font-awesome.css")%>" />

    <style>
        body {
            margin: 0;
            padding: 0;
            background-image: url('<%= ResolveUrl("~/Admin/Content/images/UserStoreAdmin_background.jpg") %>');
            background-size: cover;
            background-repeat: no-repeat;
            background-position: center center;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .header {
            background-color: #ff6f00;
            height: 80px;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 0 20px;
            box-sizing: border-box;
        }

        .header h2 {
            margin: 0;
            font-size: 24px;
            font-weight: bold;
        }

        .logo-container {
            text-align: center;
            margin-top: 20px;
            margin-bottom: 20px;
        }

        .logo-container img {
            height: 180px;
        }

        .login-container {
            width: 400px;
            margin: 0 auto;
            padding: 30px;
            background-color: rgba(255, 255, 255, 0.95);
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.3);
        }

        .login-container h2 {
            margin-bottom: 20px;
            color: #333;
            text-align: center;
        }

        .form-group {
            margin-bottom: 20px;
        }

        label {
            font-weight: bold;
            display: block;
            margin-bottom: 5px;
        }

        .password-wrapper input[type="password"],
        .password-wrapper input[type="text"] {
            width: 100%;
            padding: 10px 40px 10px 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
            transition: 0.3s ease;
        }

        input[type="text"]:focus,
        .password-wrapper input:focus {
            border-color: #ff6f00;
            box-shadow: 0 0 5px rgba(255, 111, 0, 0.6);
            outline: none;
        }

        .btn-login {
            width: 100%;
            padding: 10px;
            background-color: #ff6f00;
            color: white;
            font-weight: bold;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .btn-login:hover {
            background-color: #e65c00;
        }

        .alert {
            color: red;
            text-align: center;
            margin-bottom: 10px;
        }

        .footer {
            text-align: center;
            margin-top: 30px;
            color: gray;
        }

        .password-wrapper {
            position: relative;
            display: flex;
            flex-direction: column;
            width: 100%;
        }

        .password-wrapper .form-control {
            padding-right: 40px;
        }

        .toggle-password {
            position: absolute;
            right: 10px;
            top: 70%;
            transform: translateY(-50%);
            cursor: pointer;
            color: #888;
            font-size: 18px;
        }

        .toggle-password:hover {
            color: #ff6f00;
        }
    </style>
</head>
<body>
    <!-- Orange Header -->
    <div class="header">
        <h2>Qualification Verification System</h2>
    </div>

    <!-- UJ Logo centered below orange bar -->
    <div class="logo-container">
        <img src='<%= ResolveUrl("~/Admin/Images/ujlogo.jpg") %>' alt="UJ Logo" />
    </div>

    <!-- Login Form -->
    <form id="form1" runat="server">
        <asp:HiddenField ID="__RequestVerificationToken" runat="server" />
        <div class="login-container">
            <h2>Login</h2>

            <asp:Panel ID="loginAlertMessage" runat="server" CssClass="alert" Visible="false">
                <asp:Label ID="lblLoginMessage" runat="server"></asp:Label>
            </asp:Panel>
            
            <div class="form-group password-wrapper">
               
                <label for="txtUsername">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter username" autocomplete="off" />
            </div>

            <div class="form-group password-wrapper">
                <label for="txtPassword">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter password" autocomplete="off"/>
                <span class="toggle-password" onclick="togglePassword()">
                    <i id="eyeIcon" runat="server" class="fa-solid fa-eye"></i>
                </span>
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-login" OnClick="btnLogin_Click" />
            
            <!-- Cookie disclaimer (essential cookies for session/state) -->
            <div class="row" id="cookieDiv">
             <br />
                <label class="col-form-label  px-3">
                    <strong>Cookie notice:</strong> this system uses
                    <em>essential cookies</em> to maintain your session and keep you signed in.
                    These cookies are required for security and core functionality.
                </label>
            </div>
                <!-- Footer -->
  <div class="footer">
      &copy; University of Johannesburg - All rights reserved
  </div>  </div>

    </form>

  

    <script type="text/javascript">
        function togglePassword() {
            var passwordInput = document.getElementById('<%= txtPassword.ClientID %>');
            var eyeIcon = document.getElementById('<%= eyeIcon.ClientID %>');
            if (passwordInput.type === "password") {
                passwordInput.type = "text";
                eyeIcon.classList.remove("fa-eye");
                eyeIcon.classList.add("fa-eye-slash");
            } else {
                passwordInput.type = "password";
                eyeIcon.classList.remove("fa-eye-slash");
                eyeIcon.classList.add("fa-eye");
            }
        }
    </script>
</body>
</html>
