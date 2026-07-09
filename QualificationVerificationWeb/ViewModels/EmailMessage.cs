using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class EmailMessage
    {
        public int EmailMessageID { get; set; }
        public string EmailMessageName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> StatusInd { get; set; }
        public string DocumentType_Key { get; set; }
    }
}