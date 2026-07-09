using System;

namespace QualificationVerificationWeb.ViewModels
{
    public class StudentAddress
    {
        public int StudentAddressID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public string ComplexAddress { get; set; }
        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string CountryCode { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }

        public virtual Student Student { get; set; }
    }
}
