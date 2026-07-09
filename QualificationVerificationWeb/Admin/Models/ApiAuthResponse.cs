using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.Admin.Models
{
    public class ApiAuthResponse
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Roles { get; set; }
        public int ApplicationId { get; set; }
        public string TokenExpiry { get; set; }
    }

    public class StudentApiAuthResponse
    {
        public string Token { get; set; }
        public string StudentNum { get; set; }
        public string StudentIdNum { get; set; }
        public string Roles { get; set; }
        public string TokenExpiry { get; set; }
    }
}