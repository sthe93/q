using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class UpatedStatus
    {
        public int StudentID { get; set; }
        public string ChangedBy { get; set; }
        public int StudentStatus { get; set; }
        public int ReasonID { get; set; }
        public int DocumentID { get; set; }
        public int DocumentStatus { get; set; }
        public int IDorPassportDocumentID { get; set; }
        public int IDorPassportDocumentStatus { get; set; }
    }
}