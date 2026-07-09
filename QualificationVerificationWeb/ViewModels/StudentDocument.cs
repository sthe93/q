using System;

namespace QualificationVerificationWeb.ViewModels
{
    public  class StudentDocument
    {
        public int StudentID { get; set; }
        public int DocumentID { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
    
        public virtual Document Document { get; set; }
        public virtual Student Student { get; set; }
    }
}
