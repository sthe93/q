using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{

    public class OrderInfoData
    {
        public int StudentID { get; set; }
        public string CreatedOnDate { get; set; }
        public string DocumentType { get; set; }
        public Nullable<int> NumberCopies { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string StudentNumber { get; set; }
        public string StudentIDNumber { get; set; }
        public string LastUpdated { get; set; }
        public int StudentStatus { get; set; }
        public string DeliveryInd { get; set; }
        public string DeliveryType { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ComplexAddress { get; set; }
        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public int DocumentID { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string LastUpdatedBy { get; set; }
        public string QualificationName { get; set; }
        public string FacultyName { get; set; }
        public string FromYear { get; set; }
        public string ToYear { get; set; }
        public string IsPaid { get; set; }
        public string MaidenSurname { get; set; }
        public string CourierInstructions { get; set; }
        public string ReferenceNumber { get; set; }
        public string CurrentlyResidingFaculty { get; set; }
    }
}