using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class RequestSummaryInfoData
    {
        public int StudentAcademicDocumentID { get; set; }
        public int StudentID { get; set; }
        public string DeliveryType { get; set; }
        public string DocumentType { get; set; }
        public int NumberCopies { get; set; }
        public string Address { get; set; }
    }
}