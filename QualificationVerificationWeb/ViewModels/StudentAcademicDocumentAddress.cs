using System;

namespace QualificationVerificationWeb.ViewModels
{
    public  class StudentAcademicDocumentAddress
    {
        public int StudentID { get; set; }
        public int StudentAcademicDocumentID { get; set; }
        public int StudentAddressID { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }

        public virtual Student Student { get; set; }
        public virtual StudentAcademicDocument StudentAcademicDocument { get; set; }

    }
}
