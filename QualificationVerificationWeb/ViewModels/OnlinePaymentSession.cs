using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class OnlinePaymentSession
    {
        public int OnlinePaymentSessionID { get; set; }
        public int StudentID { get; set; }
        public string TotalAmount { get; set; }
        public int TryAgain { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string Token { get; set; }
    }
}