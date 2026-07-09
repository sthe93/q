using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class StudentAcademicDocumentOrderDto
    {
        public int StudentID { get; set; }
        public string ComplexAddress { get; set; }
        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Address { get; set; }
        public string DeliveryMethod { get; set; } 
        public string DeliveryInd { get; set; }     
        public string AcademicRecord { get; set; }
        public string TranscriptSupplement { get; set; }
        public string ConfirmationLetter { get; set; }
        public string FormsForOfficialBodies { get; set; }


    }
}