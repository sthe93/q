<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Calculate.aspx.cs" Inherits="QualificationVerificationWeb.Calculate" MaintainScrollPositionOnPostback="true" %>
<%@ Import Namespace="QualificationVerificationWeb.Helper" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="<%= ResolveUrl("~/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.min.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.css")%>" rel="stylesheet" />
    <script src="Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.js"></script>

    <style>
    .btnFeesGrant { background-color: #E7540E !important; color: #fff !important; }
.btnCancel { margin-left: 30px; }
.deliveryGridview { margin-left: 15px; width: 782px; }
                  td, th { padding: 4px 5px 4px 4px !important; }
#divAddSecond { margin-bottom: 0px; }
#ContentPlaceHolder1_gvAddress_ddlNumberofAddress_0,
#ContentPlaceHolder1_gvAddress_ddlSendToMultipleAddress_0 {
                         background-color: #fdfdfd; border-color: #fdfdfd; color: #fdfdfd;
}
</style>

    <script type="text/javascript">
                 function pageLoad(sender, args) {
                 $('[data-toggle="tooltip"]').tooltip();
                 }

                          $(document).ready(function () {
                                                         // Initialize multiselect dropdowns
                          $('[id*=lstAcademicDocument], [id*=lstDocumentType]').multiselect({
                              includeSelectAllOption: false
                          });

                                                         // Hide specific dropdowns
                          $('#ContentPlaceHolder1_gvAddress_ddlNumberofAddress_0, #ContentPlaceHolder1_gvAddress_ddlSendToMultipleAddress_0, #ContentPlaceHolder1_gvAddress_ddlNumberofAddress_1, #ContentPlaceHolder1_gvAddress_ddlSendToMultipleAddress_1').hide();
                          });

    function CancelAndClose() {
    $("#myModal").removeClass('fade').modal('hide');
        return false;
    }

    function EnableTextBoxValidation(objDDL) {
        var splitObj = objDDL.split("_");
        var objTextID = splitObj[0] + "_" + splitObj[1] + "_ddlNumberofAddress_" + splitObj[3];
        var objTextSample = document.getElementById(objTextID);
        var ddlDOM = document.getElementById(objDDL);

        objTextSample.disabled = ddlDOM.options[ddlDOM.selectedIndex].value !== "1";
        if (objTextSample.disabled) {
            objTextSample.options[objTextSample.selectedIndex].value = 0;
        }
    }

    function checkButtonClicked(buttonSelected) {
        document.getElementById("ContentPlaceHolder1_ButtonClicked").value = buttonSelected;
    }
    </script>

    <div class="tab-content">
    <asp:UpdatePanel ID="pnlCalcutate" runat="server" UpdateMode="Always">
    <ContentTemplate>
    <div class="panel-body">
    <div id="AppliedFeedback" runat="server" style="text-align: start;">
    <br />
    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Style="text-align: left" Text="" Width="800px"></asp:Label>
    <br />
    <div style="text-align: left">
    <div id="Div1" runat="server" style="border-style: groove; border-color: inherit; border-width: 1px; text-align: start; width: 72%; margin-left: 170px">
    <br />
    <div id="divDocumentMultiType" runat="server" visible="true">
    <label class="col-md-2 control-label" style="text-align: left; top: 3px; left: 0px; width: 1200px;">
    <b>Payment calculator</b> - Please note that payment is per document type requested. Each document type outlines your entire academic journey at UJ.
</label>
    <br />
    <br />
    <asp:UpdatePanel ID="addresspanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:GridView ID="gvAddress" runat="server" AutoGenerateColumns="false" BorderWidth="0px" GridLines="None" Width="90%" Style="margin-left: 70px; margin-right: 90px; margin-top: 60px;">
    <Columns>
    <asp:TemplateField HeaderText="" ItemStyle-Width="1%" ItemStyle-Font-Size="Small">
    <ItemTemplate>
    <asp:HiddenField runat="server" ID="DeliveryInd" Value='<%# Eval("DeliveryInd")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Academic Record" ItemStyle-Width="5%" HeaderStyle-Width="10%" ItemStyle-Font-Size="Small">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAcademicRecord" runat="server" Text='<%# Eval("Academic Record") %>' Visible="false" />
                                                            <asp:DropDownList ID="ddlNumberofAcademicRecord" runat="server" Height="45px" Width="80px" AppendDataBoundItems="true" AutoPostBack="true" SelectedValue='<%# Eval("Academic Record") %>' OnSelectedIndexChanged="ddlNumberofAcademicRecord_SelectedIndexChanged">
                                                                <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Transcript Supplement" ItemStyle-Width="5%" HeaderStyle-Width="10%" ItemStyle-Font-Size="Small">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranscriptSupplement" runat="server" Text='<%# Eval("Transcript Supplement") %>' Visible="false" />
                                                            <asp:DropDownList ID="ddlNumberofTranscriptSupplement" runat="server" Height="45px" Width="80px" AppendDataBoundItems="true" AutoPostBack="true" SelectedValue='<%# Eval("Transcript Supplement") %>' OnSelectedIndexChanged="ddlNumberofTranscriptSupplement_SelectedIndexChanged">
                                                                <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Confirmation Letter" ItemStyle-Width="5%" HeaderStyle-Width="10%" ItemStyle-Font-Size="Small">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblConfirmationLetter" runat="server" Text='<%# Eval("Confirmation Letter") %>' Visible="false" />
                                                            <asp:DropDownList ID="ddlNumberofConfirmationLetter" runat="server" Height="45px" Width="80px" AppendDataBoundItems="true" AutoPostBack="true" SelectedValue='<%# Eval("Confirmation Letter") %>' OnSelectedIndexChanged="ddlNumberofConfirmationLetter_SelectedIndexChanged">
                                                                <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Forms For Official Bodies" ItemStyle-Width="5%" HeaderStyle-Width="15%" ItemStyle-Font-Size="Small">
                                                        <HeaderTemplate>
                                                            <span style="color: red">*</span> Forms For Official Bodies
                                                            <a href="#" data-toggle="tooltip" title="e.g. WES, CORU, CGFNS" style="padding-left: 5px">
                                                                <span class="fa fa-question-circle" onclick="return false"></span>
                                                            </a>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFormsForOfficialBodies" runat="server" Text='<%# Eval("Forms For Official Bodies") %>' Visible="false" />
                                                            <asp:DropDownList ID="ddlNumberofFormsForOfficialBodies" runat="server" Height="45px" Width="80px" AppendDataBoundItems="true" AutoPostBack="true" SelectedValue='<%# Eval("Forms For Official Bodies") %>' OnSelectedIndexChanged="ddlNumberofFormsForOfficialBodies_SelectedIndexChanged">
                                                                <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Send to multiple address" ItemStyle-Width="5%" HeaderStyle-Width="10%" ItemStyle-Font-Size="Small" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSendToMultipleAddress" runat="server" Text='<%# Eval("Send To Multiple Address") %>' Visible="false" />
                                                            <asp:DropDownList ID="ddlSendToMultipleAddress" runat="server" onchange="EnableTextBoxValidation(this.id)" Height="45px" Width="80px" AppendDataBoundItems="true" SelectedValue='<%# Eval("Send To Multiple Address") %>'>
                                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Number of Address" ItemStyle-Width="15%" HeaderStyle-Width="10%" ItemStyle-Font-Size="Small" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNumberOfAddress" runat="server" Text='<%# Eval("Number of Address") %>' Visible="false" />
                                                            <asp:DropDownList ID="ddlNumberofAddress" runat="server" Height="45px" Width="80px" AppendDataBoundItems="true" Enabled="false" SelectedValue='<%# Eval("Number of Address") %>' OnSelectedIndexChanged="ddlNumberofAddress_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <br />
                                <div id="Div2" class="form-inline" runat="server" style="margin-left: 300px">
                                    <asp:Button ID="btnCalculate" CssClass="btn btn-primary" runat="server" Text="Calculate" CausesValidation="false" Width="105px" OnClick="btnCalculate_Click" />
                                    <asp:Button ID="btnClear" CssClass="btn btn-primary" runat="server" Text="Clear" CausesValidation="false" OnClick="btnClear_Click" />
                                    <asp:Button ID="btnClose" CssClass="btn btn-primary" runat="server" Text="Close" CausesValidation="false" OnClick="btnClose_Click" />
                                </div>
                                <br />
                                <div id="Div10" runat="server" style="margin-left: 200px" visible="false">
                                    <div class="form-inline">
                                        <p>Total Amount Payable: <b><asp:Label ID="lblTotalAmount" Style="text-align: center" runat="server" Text=""></asp:Label></b></p>
                                    </div>
                                </div>
                                <br />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>