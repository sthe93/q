using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class AcademicDocumentData
    {
        public int AcademicDocumentID { get; set; }
        public string AcademicDocumentDesc { get; set; }
        public string DocumentType { get; set; }
        public string DocumentFee { get; set; }
        public string DeliveryType { get; set; }
        public string DeliveryInd { get; set; }
        public decimal? D_DocumentFee { get; set; }
        public string DocumentType_Key { get; set; }
    }
}