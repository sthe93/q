using System;

namespace QualificationVerificationWeb.ViewModels
{
    public class StudentQualification
    {
        public int StudentQualificationID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public string QualificationName { get; set; }
        public string FacultyName { get; set; }
        public string FromYear { get; set; }
        public string ToYear { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public string CurrentlyResidingFaculty { get; set; }

        public virtual Student Student { get; set; }

    }
}
