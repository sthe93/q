using System;
using System.Collections.Generic;

namespace QualificationVerificationWeb.ViewModels
{
    public class StudentAcademicDocument
    {
        public StudentAcademicDocument()
        {
            this.StudentAcademicDocumentAddresses = new HashSet<StudentAcademicDocumentAddress>();
        }

        public int StudentAcademicDocumentID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> AcademicDocumentID { get; set; }
        public Nullable<int> NumberCopies { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }

        public virtual AcademicDocument AcademicDocument { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<StudentAcademicDocumentAddress> StudentAcademicDocumentAddresses { get; set; }

    }
}
