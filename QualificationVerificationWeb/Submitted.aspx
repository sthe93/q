<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Submitted.aspx.cs" Inherits="QualificationVerificationWeb.Submitted" MasterPageFile="~/Master.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Content/bootstrap/jquery/jquery.js"></script>
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.min.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.css")%>" rel="stylesheet" />
    <script src="Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.js"></script>

   

    <style>
    .hrefUJholidays {
        color:black;
    }
</style>

<div class="tab-content">
<asp:UpdatePanel ID="pnlNoaccess" runat="server" UpdateMode="Always">
<ContentTemplate>
                
<div class="panel-body">
                    
<div class="panel-body">
<%--<div class="form-group">--%>
                               <H4>Confirmation of request</H4>
                                <div style="text-align: left">
                                   <b> <br />
                                    <p>Thank you for your request for a Academic Record/ Confirmation Letter/ Transcript Supplement. Please note that your payment is</p> 
                                    <p> currently being verified and an email confirming the validity status will be sent to you in due course.</p>
                                    <br />
                                    <br />
                                    We are busy processing your order.
                                    <br />
                                    <br />
                                    Please note: Working days excludes day of submission, weekends, public holidays and the <a href="https://www.uj.ac.za/about/Pages/Academic-Calendar-and-Regulations.aspx" target="_blank" id="hrefUJholidays">University of Johannesburg holidays</a>.
                                    <br />
                                     
                                    <br /></b>
                                    HOW LONG WILL IT TAKE TO RECEIVE MY DOCUMENTS?
                                    <br />
                                    Subsidised Programmes
                                    <br />
                                    <br />
                                    <p> Academic record:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3 working days</p>
                                     
                                    <p> Transcript supplement:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 20 working days</p>

                                    <p> Confirmation Letter:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3 working days</p>
                                    <br />
                                    <p>REQUESTS FROM STUDENTS PREVIOUSLY REGISTERED AT RAU AND TWR</p>
                                    
                                    <p> Academic Record:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  Minimum of 3 working days</p>
                                   
                                    <p> Transcript Supplement:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Minimum of 20 working days</p>

                                    <p>Confirmation Letter:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Minimum of 3 working days</p>
                                    <br />
                                    <p>Please note:</p>
                                    <p>Academic Records and Transcripts Supplements issued by RAU and TWR may take up-to 40 working days to be issued.</p>
                                    <p>Due to the historical nature of certain qualifications (from RAU/TWR) and Continuing Education Programmes, the University</p>
                                    <p>of Johannesburg may not be able to provide some of the records. In these instances, a letter will be issued.</p>
                                    <br />
                                    <p>Continuing Education Programmes (CEP)</p>

                                    <p>Academic Record:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Minimum of 3 working days</p>
                                    
                                    <p>Transcript Supplement:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Minimum of 20 working days</p>

                                    <p> Confirmation Letter:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Minimum of 3 working days</p>
                                    <br />
                                    <p>Certain requests may take longer to process.</p>
                                    
                                </div> 
                                 <label runat="server" id="lblMessageLinks"> For frequently asked questions and answers <b><a href="Faq.aspx" target="_blank">click here</a></b></label>
                                <br/>
                                <br/>
                            <a href="Default.aspx">Back</a> 
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="https://www.uj.ac.za/">Go to UJ Website</a>
                            <br/>
                        <%--</div>--%>
                    </div>
                </div>
             
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
