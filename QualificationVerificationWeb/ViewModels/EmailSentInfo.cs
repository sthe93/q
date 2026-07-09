using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class EmailSentInfo
    {
        public int EmailSentID { get; set; }
        public int StudentID { get; set; }
        public string EmailName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<byte> SentInd { get; set; }
        public Nullable<System.DateTime> SentDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}