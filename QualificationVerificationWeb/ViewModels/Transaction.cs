using System;

namespace QualificationVerificationWeb.ViewModels
{
    public class Transaction
    {
        public System.Guid TRANSACTION_ID { get; set; }
        public Nullable<System.DateTime> DATE { get; set; }
        public string PAY_REQUEST_ID { get; set; }
        public Nullable<int> AMOUNT { get; set; }
        public string REFERENCE { get; set; }
        public string TRANSACTION_STATUS { get; set; }
        public Nullable<int> RESULT_CODE { get; set; }
        public string RESULT_DESC { get; set; }
        public string CUSTOMER_EMAIL_ADDRESS { get; set; }
    }
}