using System;

namespace QualificationVerificationWeb.ViewModels
{
    public partial class StudentParentDocument
    {
        public int StudentParentID { get; set; }
        public int DocumentID { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
    
        public virtual Document Document { get; set; }
        public virtual StudentParent StudentParent { get; set; }
    }
}
