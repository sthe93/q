using System;

namespace QualificationVerificationWeb.ViewModels
{
    public class EmailSent
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
        public virtual Student Student { get; set; }
    }
}

