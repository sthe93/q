using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class ResponseDto
    {
        public string Id { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}