using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class StudentAcademicDocumentAddressDto
    {
        public int StudentID { get; set; }
        public int AcademicDocumentID { get; set; }
        public int NumberCopies { get; set; }
        public string ComplexAddress { get; set; }
        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string CountryCode { get; set; }
    }
}