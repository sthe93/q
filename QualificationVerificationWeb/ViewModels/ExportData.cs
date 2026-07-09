using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class ExportData
    {

        public string CreatedOnDate { get; set; }
        public string CreatedOnTime { get; set; }
        public string Surname { get; set; }
        public string MaidenSurname { get; set; }
        public string FullName { get; set; }
        public string StudentNumber { get; set; }
        public string StudentIDNumber { get; set; }
        public string QualificationName { get; set; }
        public string FacultyName { get; set; }
        public string FromYear { get; set; }
        public string ToYear { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string RequestStatus { get; set; }
        public string DocumentType { get; set; }
        public string DeliveryType { get; set; }

        #region new fields
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaygateReference { get; set; }
        #endregion
        public string ComplexAddress { get; set; }
        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdated { get; set; }
    }
}