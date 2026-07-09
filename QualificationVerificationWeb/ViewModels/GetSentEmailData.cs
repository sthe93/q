using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class GetSentEmailData
    {
        public int EmailSentID { get; set; }
        public int StudentID { get; set; }
        public string EmailName { get; set; }
        public string Message { get; set; }
        public Nullable<byte> SentInd { get; set; }
        public string SentDate { get; set; }
        public string CreatedDate { get; set; }
    }
}