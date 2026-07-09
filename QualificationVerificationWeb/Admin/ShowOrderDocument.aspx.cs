using QualificationVerificationWeb.Admin.Content;
using QualificationVerificationWeb.Helper;
using QualificationVerificationWeb.ViewModels;
using System;
using System.IO;
using System.Net.Http;
using System.Web;

namespace QualificationVerificationWeb.Admin
{
    public partial class ShowOrderDocument : BasePage
    {
        
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                    if (!string.IsNullOrEmpty((Request.QueryString["DocumetId"])))
                    {
                          
                           var documentID = Guid.Parse(Request.QueryString["DocumetId"]);

                            var endpoint = $"Document/GetDocumentByPropertyId?documentID={documentID}";


                            // Use ApiClient
                            var apiClient = new ApiClient();
                            var rest = await apiClient.GetAsync(endpoint);


                            if (rest.IsSuccessStatusCode)
                            {
                                var doc = rest.Content.ReadAsAsync<Document>().Result;


                                ViewFile1(doc.DocumentFile);

                                //else return field could not be found
                            }
                      
                    }
               
            }
        }
        public void ViewFile1(byte[] _selectedDocument)
        {
            bool IsImage = IsValidImage(_selectedDocument);
            Response.Clear();
            if (IsImage)
                Response.ContentType = "image/jpeg";
            else
                Response.ContentType = "application/pdf";


            Response.Clear();
            Response.Buffer = true;      
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(_selectedDocument);
            Response.End();
        }
    
        public static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                    System.Drawing.Image.FromStream(ms);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }


        public MemoryStream ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms;
            }
        }
    }
}