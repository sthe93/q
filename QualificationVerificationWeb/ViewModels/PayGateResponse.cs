using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class PayGateResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}