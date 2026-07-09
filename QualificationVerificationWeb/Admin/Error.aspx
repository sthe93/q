<%@ Page Language="C#" ValidateRequest="false" EnableViewState="false" %>

<!DOCTYPE html>
<html>
<head>
    <title>Error</title>
    <meta charset="utf-8" />
    
    <!-- Minimal safe styling -->
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f6f9;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 600px;
            margin: 80px auto;
            background: #ffffff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            text-align: center;
        }

        h2 {
            color: #d9534f;
        }

        p {
            color: #333;
            line-height: 1.6;
        }

        a {
            color: #007bff;
            text-decoration: none;
        }

        a:hover {
            text-decoration: underline;
        }

        .footer {
            margin-top: 20px;
            font-size: 14px;
            color: #777;
        }
    </style>
</head>
<body>

    <div class="container">
        <h2>Our Apologies</h2>

        <p>
            An unexpected error has occurred, we apologise for the inconvenience.
        </p>

        <p>
            Please try again:
          
        </p>


        <div class="footer">
         <a href="<%= ResolveUrl("~/Admin/Login.aspx") %>">Login</a>
        </div>
    </div>


</body>
</html>
