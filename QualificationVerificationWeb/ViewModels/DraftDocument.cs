using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class DraftDocument
    {
        public int DraftDocumentId { get; set; }
        public int StudentId { get; set; }
        public int AcademicRecordCount { get; set; }
        public int TranscriptSupplementCount { get; set; }
        public int ConfirmationLetterCount { get; set; }
        public int FormsForOfficialBodiesCount { get; set; }
        public string DeliveryMethod { get; set; }
        public string CountryCode { get; set; }
        public string ComplexAddress { get; set; }
        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public virtual Student Student { get; set; }
    }
}