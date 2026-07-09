using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class StudentQualificationAPIData
    {
        public string STUDENT_NUMBER { get; set; }
        public string SURNAME { get; set; }
        public string INITIALS { get; set; }
        public string FIRST_NAMES { get; set; }
        public string TITLE { get; set; }
        public string NI_PI { get; set; }
        public string STUDENT_NAME { get; set; }
        public string QUALIFICATION_CODE { get; set; }
        public string QUALIFICATION_NAME { get; set; }
        public string ALTERNATE_QUALIFICATION_NAME { get; set; }
        public string EXTERNAL_CODE { get; set; }
        public string STARTYEAR { get; set; }
        public string ENDYEAR { get; set; }
        public string FACULTY_SCHOOL_NAME { get; set; }
        public string NUMBER_OF_QUAL_REGISTRATIONS { get; set; }
        public string APPROVED_QUALIFICATION_NAME { get; set; }
        public string CurrentlyResidingFaculty { get; set; }
    }
}