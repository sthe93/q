using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class StudentParent
    {
        public StudentParent()
        {
            this.StudentParentDocuments = new HashSet<StudentParentDocument>();
        }

        public int StudentParentID { get; set; }
        public int StudentID { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public string ParentName { get; set; }
        public string ParentSurName { get; set; }
        public string ParentIDNumber { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<StudentParentDocument> StudentParentDocuments { get; set; }
    }
}