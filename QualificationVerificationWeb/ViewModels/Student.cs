using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace QualificationVerificationWeb.ViewModels
{
    [Serializable]
    public class Student
    {
        
        public byte[] IdPassportCopy { get; set; }
        public string IdPassportFileName { get; set; }

        public byte[] SpecialInstructionCopy { get; set; }
        public string SpecialInstructionFileName { get; set; }

        public Student()
        {
            this.EmailSents = new HashSet<EmailSent>();
            this.StudentAcademicDocuments = new HashSet<StudentAcademicDocument>();
            this.StudentAcademicDocumentAddresses = new HashSet<StudentAcademicDocumentAddress>();
            this.StudentAddresses = new HashSet<StudentAddress>();
            this.StudentDocuments = new HashSet<StudentDocument>();
            this.StudentParents = new HashSet<StudentParent>();
            this.StudentQualifications = new HashSet<StudentQualification>();
        }

            public int StudentID { get; set; }
            public string StudentNumber { get; set; }
            public string StudentIDNumber { get; set; }
            public Nullable<System.DateTime> CreatedOnDate { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> LastUpdated { get; set; }
            public string LastUpdatedBy { get; set; }
            public string ApplicationYear { get; set; }
            public Nullable<bool> AcceptedTerms { get; set; }
            public int StudentStatus { get; set; }
            public Nullable<int> ReasonID { get; set; }
            public string Surname { get; set; }
            public string FullName { get; set; }
            public string MaidenSurname { get; set; }
            public string EmailAddress { get; set; }
            public string PhoneNumber { get; set; }
            public string CourierInstructions { get; set; }
            public string ReferenceNumber { get; set; }
            public Nullable<bool> ConfirmDuplicateOrder { get; set; }
            public Nullable<System.DateTime> ConfirmDuplicateOrderDt { get; set; }
            public Nullable<int> DuplicateOfStudentID { get; set; }
            public Nullable<bool> ThirdParty_or_Student { get; set; }
            public string PaymentMethod { get; set; }
            public Nullable<int> OnlinePaymentApproval { get; set; }
            public bool IsDraft { get; set; }
            [ScriptIgnore]
             public virtual ICollection<EmailSent> EmailSents { get; set; }
             [ScriptIgnore]
        public virtual Reason Reason { get; set; }
        [ScriptIgnore]
        public virtual ICollection<StudentAcademicDocument> StudentAcademicDocuments { get; set; }
        [ScriptIgnore]
        public virtual ICollection<StudentAcademicDocumentAddress> StudentAcademicDocumentAddresses { get; set; }
        [ScriptIgnore]
        public virtual ICollection<StudentAddress> StudentAddresses { get; set; }
        [ScriptIgnore]
        public virtual ICollection<StudentDocument> StudentDocuments { get; set; }
        [ScriptIgnore]
        public virtual ICollection<StudentParent> StudentParents { get; set; }
        [ScriptIgnore]
        public virtual ICollection<StudentQualification> StudentQualifications { get; set; }
      
    }
}