<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternalServerError.aspx.cs" Inherits="QualificationVerificationWeb.InternalServerError" MasterPageFile="~/Master.Master"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Content/bootstrap/jquery/jquery.js"></script>
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.min.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.css")%>" rel="stylesheet" />
    <script src="Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.js"></script>
  
    <div class="tab-content">
        <asp:UpdatePanel ID="pnlError" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="form-group">
                    <div class="panel-body">
                         <div id="ErrorInternal" runat="server" style="text-align: start;">
                            <br />
                            <br />
                            <br />
                            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Style="text-align: left" Text="" Width="700px"></asp:Label>
                              <div class="form-group" style="text-align: left">
                                    <br />
                                    <h2><b>Error while processing the request.</b></h2>
                                    <br /> 
                                     <h5>An unknown error has occured!</h5>
                                    <br />
                                    <br />
                                    <a href="https://www.uj.ac.za/">Go to UJ Website.</a>
                                    <br />
                               </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
