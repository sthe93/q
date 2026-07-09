<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PagGateRedirect.aspx.cs" Inherits="QualificationVerificationWeb.PagGateTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="Content/bootstrap/jquery/jquery.js"></script>

    <script type="text/javascript">
        $(function () {

            var paygateId = localStorage.getItem("PAYGATE_ID");
            var requestId = localStorage.getItem("PAY_REQUEST_ID");
            var reference = localStorage.getItem("REFERENCE");
            var checksum = localStorage.getItem("CHECKSUM");

            // Safety check – don't submit empty form
            if (!paygateId || !requestId || !reference || !checksum) {
                console.error("PayGate values missing");
                return;
            }

            $("#PAYGATE_ID").val(paygateId);
            $("#PAY_REQUEST_ID").val(requestId);
            $("#REFERENCE").val(reference);
            $("#CHECKSUM").val(checksum);

            localStorage.clear();

            $("#payGForm").submit();

        });
    </script>
</head>

<body>


<form id="payGForm" action="https://secure.paygate.co.za/payweb3/process.trans" method="POST">

    <input type="hidden" id="PAYGATE_ID" name="PAYGATE_ID" />
    <input type="hidden" id="PAY_REQUEST_ID" name="PAY_REQUEST_ID" />
    <input type="hidden" id="REFERENCE" name="REFERENCE" />
    <input type="hidden" id="CHECKSUM" name="CHECKSUM" />

</form>

</body>
</html>