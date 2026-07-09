using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class StudentQualificationData
    {
        public int StudentID { get; set; }
        public string QualificationName { get; set; }
        public string FacultyName { get; set; }
        public string FromYear { get; set; }
        public string ToYear { get; set; }
        public string CurrentlyResidingFaculty { get; set; }
    }

}