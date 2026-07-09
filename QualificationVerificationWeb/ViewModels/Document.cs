using System;
using System.Collections.Generic;

namespace QualificationVerificationWeb.ViewModels
{
    public class Document
    {
        public Document()
        {
            this.StudentDocuments = new HashSet<StudentDocument>();
            this.StudentParentDocuments = new HashSet<StudentParentDocument>();
        }

        public int DocumentID { get; set; }
        public Nullable<int> DocumentTypeID { get; set; }
        public string Document1 { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DocumentStatus { get; set; }
        public byte[] DocumentFile { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual ICollection<StudentDocument> StudentDocuments { get; set; }
        public virtual ICollection<StudentParentDocument> StudentParentDocuments { get; set; }
    }
}