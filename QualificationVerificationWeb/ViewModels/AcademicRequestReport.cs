using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class AcademicRequestReport
    {
        public int StudentID { get; set; }
        public string CreatedOnDate { get; set; }
        public string DocumentType { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string StudentNumber { get; set; }
        public string StudentIDNumber { get; set; }
        public string LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public int StudentStatus { get; set; }
        public string DeliveryInd { get; set; }
        public string DeliveryType { get; set; }
        public string EmailAddress { get; set; }
        public string StreetAddress { get; set; }
        public int DocumentID { get; set; }
        public Guid DocumentGuidID { get; set; }
        public decimal TotalAmount { get; set; }
        public string ComplexAddress { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string ProofOfPayment { get; set; }
        public string RequestStatus { get; set; }
        public string Reason { get; set; }
        public string MaidenSurname { get; set; }
        public string CourierInstructions { get; set; }
        public string NumberOfDays { get; set; }
        public string DocumentTypeInd { get; set; }
        public int SpecialinstructionsDocumentID { get; set; }
        public Guid SpecialinstructionsDocumentGuidId { get; set; }
        public string DuplicateOf { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime Orderdate { get; set; }
        public string ThirdParty_or_Student { get; set; }
        public int IDorPassportDocumentID { get; set; }
        public Guid IDorPassportDocumentGuidID { get; set; }
        public string IDorPassportStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string PaygateReference { get; set; }
        public string TransactionDate { get; set; }
        public string CountryIndication { get; set; }
    }
}