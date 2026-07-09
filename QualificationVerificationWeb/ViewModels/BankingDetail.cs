using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class BankingDetail
    {
        public int BankingDetailsID { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }
        public string Reference { get; set; }
    }
}