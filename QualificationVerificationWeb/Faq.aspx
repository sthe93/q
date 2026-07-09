<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Faq.aspx.cs" Inherits="QualificationVerificationWeb.Faq" MasterPageFile="~/Master.Master"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Content/bootstrap/jquery/jquery.js"></script>
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.min.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.css")%>" rel="stylesheet" />
    <script src="Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.js"></script>
 
    <div class="tab-content">
        <asp:UpdatePanel ID="pnlFaq" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="panel-body">
                    
                    <div class="panel-body">
                     <div id="inline-editor-5158" data-pk="5158" data-languagecode="EN-GB" class="inline-editor ">
<br />
                         <br />
<h3><b><u>Frequently Asked Questions</u></b></h3>
<p>&nbsp;</p>
<h4>I AM NOT ABLE TO LOGIN?</h4>

<p>If you can't log in it might be because of a few reasons.</p>

<ul>
	<li>Make sure that your Identity number/Passport number is correct.</li>
	<li>If you have used your student number to login in, ensure that this is the correct student number.</li>
	<li>If you have fees outstanding this will also block you from logging onto the system.</li>
    <li>Accounts can also be blocked by the University if any suspicious activity is recognised.</li>
</ul>

<hr>
<h4>WHAT IS AN ACADEMIC RECORD?</h4>

<ul>
	<li>It is a record that contains student’s registration information, which includes student name, student number, identity number, when the qualification(s) was issued (if obtained).</li>
	<li>It also contains all modules/subjects registered and cancelled as well as marks for all modules passed and failed.</li>
	<li>A code of conduct (alumni’s/students’ behavior during the period of study) is included on the academic record. Please note that a separate letter is not issued for the code of conduct.</li>
</ul>

<hr>
<h4>WHAT IS A TRANSCRIPT SUPPLEMENT?</h4>

<ul>
	<li>It is a document that contains the details of all modules passed, as well as a short description of each module completed including NQF level, Credits if applicable. Failed modules are excluded.</li>
	<li>The supplement will also include the Academic Record.</li>
</ul>

<hr>
<h4>WHAT IS A CONFIRMATION LETTER?</h4>

<ul>
	<li>This document provides confirmation of qualification(s) registered as well as an indication of whether it was obtained or not.</li>
	<li>The medium of instruction (English) will be included in the confirmation letter for UJ and TWR alumni.</li>
    <li>For RAU alumni the verifications team would be required to verify if the qualification was offered in English for a particular year.</li>
</ul>

<hr>
<h4>WHAT IS A SUBSIDISED ACADEMIC PROGRAMME?</h4>

<p>A subsidised academic programme means an externally approved, DHET funded, registered and accredited, structured academic programme at the University that, upon successful completion, will lead to the award of a formal qualification such as a certificate, diploma or a degree.</p>

<hr>
<h4>WHAT IS A CONTINUING EDUCATION PROGRAMME (CEP)?</h4>

<p>CEP means an institution-approved short learning programme or institution-approved whole programme (the latter accredited by HEQC and registered by SAQA). These programmes receive no state funding and upon successful completion, will lead to the award of a qualification in the case of a whole programme or a UJ certificate in case of a SLP.</p>

<hr>
<h4>HOW MUCH WILL IT COST?</h4>

<p>Please note:</p>

<p>&nbsp;</p>
<ul>
	<li>Each request will include both electronic and original hard copies of the record.</li>
	<li>THE PRICES BELOW ARE FOR COLLECTION ONLY AND INCLUDES VAT.</li>
	<li>Courier services are at an additional cost to the price of the Academic Record and Transcript Supplement.</li>
</ul>

<p>&nbsp;</p>

<p>Academic Record: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; R250.00</p>

<p>Transcript Supplement:&nbsp;&nbsp;&nbsp; R400.00</p>

<p>&nbsp;</p>

<p><strong>COURIER SERVICES</strong></p>

<p>R300.00 - South Africa</p>

<p>R600.00 - International</p>

<p>&nbsp;</p>

<p>Please note that the prices indicated above are 2019 rates and are subject to change.</p>

<hr>
<h4>HOW LONG WILL IT TAKE TO RECEIVE MY DOCUMENTS?</h4>

<p><strong>Subsidised Programmes</strong></p>

<p>Academic record:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3 working days</p>

<p>Transcript supplement:&nbsp;&nbsp;&nbsp;&nbsp; 20 working days</p>

<hr>
<h4>REQUESTS FROM STUDENTS PREVIOUSLY REGISTERED AT RAU AND TWR</h4>

<p>Academic Record:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A minimum of 3 working days*</p>

<p>Transcript Supplement:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A minimum of 20 working days*</p>

<p>&nbsp;</p>

<p><em>Please note:</em></p>

<p>*Academic Records and Transcript Supplements issued by RAU/TWR may take up-to 40 working days to be issued. Due to the historical nature of certain qualifications from RAU/TWR and Continuing Education Programmes, the University of Johannesburg may not be able to provide some of the records. In these instances, a letter will be issued.</p>
<hr>
<p>&nbsp;</p>

<p><strong>Continuing Education Programmes (CEP)</strong></p>

<p>Academic Record:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A minimum of 3 working days*</p>

<p>Transcript Supplement:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A minimum of 20 working days*</p>

<p>&nbsp;</p>

<p><em>Please note:</em></p>

<p>*Academic Records and Transcript Supplements issued by RAU/TWR may take up-to 40 working days to be issued. Due to the historical nature of certain qualifications from RAU/TWR and Continuing Education Programmes, the University of Johannesburg may not be able to provide some of the records. In these instances, a letter will be issued.</p>


<hr>
<h4>HOW WILL I RECEIVE MY DOCUMENTS?</h4>

<ul>
	<li>Electronic copies will be submitted via email.</li>
	<li>Hard copies may be collected or delivered by courier services at an additional fee (see various costs for national and international).</li>
</ul>

<hr>
<h4>HOW ARE REQUESTS FROM STUDENTS OF VISTA UNIVERSITY AND LYCEUM COLLEGE HANDLED?</h4>

<ul>
	<li>UJ is not in possession of records for studies completed at Vista University or Lyceum College. These records are to be requested from UNISA and Lyceum College respectively.</li>
</ul>

<hr>
<h4>WHAT ARE SPECIAL INSTRUCTIONS?</h4>

<p>These requests may entail the following:</p>

<ul>
	<li>Students applying to study or work abroad are usually required to complete additional documents/forms such as WES, CGFNS, NMC, Embassies. These forms can be uploaded under this section together with the instructions.</li>
	<li>Alumni and non-graduated students who require information pertaining to practical work completed to be included in the Transcript Supplement.</li>
	<li>Requests to the University of Johannesburg to submit Academic Records/Transcripts Supplement to a third party.</li>
</ul>

<hr>
                            <div id="Div2" class="form-inline" runat="server" style="margin-left: 300px">
                              <label class="col-md-2 control-label "></label> 
                                <asp:Button ID="btnClose" CssClass="btn btn-primary " runat="server" Text="Close" CausesValidation="false" OnClick="btnClose_Click" onchange="test()" />
                          </div>
<p>&nbsp;</p>

    </div>
                  
                    </div>
                </div>
             
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

