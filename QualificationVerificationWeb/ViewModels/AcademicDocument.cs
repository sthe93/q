using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class AcademicDocument
    {
        public AcademicDocument()
        {
            this.StudentAcademicDocuments = new HashSet<StudentAcademicDocument>();
        }

        public int AcademicDocumentID { get; set; }
        public string Description { get; set; }
        public string DocumentType { get; set; }
        public Nullable<decimal> DocumentFee { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public string DeliveryType { get; set; }
        public string DeliveryInd { get; set; }
        public string TurnAroundTime { get; set; }
        public Nullable<int> IsActive { get; set; }

        public virtual ICollection<StudentAcademicDocument> StudentAcademicDocuments { get; set; }
    }
}