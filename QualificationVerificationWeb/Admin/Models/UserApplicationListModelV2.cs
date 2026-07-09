using System;
using System.Collections.Generic;
using QualificationVerificationWeb.Admin.Content;

namespace QualificationVerificationWeb.Admin.Models
{
    public class UserApplicationModel
    {
        public int AppId { get; set; }
        public Guid ApplicationGuid { get; set; }
        public string AppName { get; set; }
        public string AppDescription { get; set; }
        public string AppIcon { get; set; }
        public string AppUrl { get; set; }
        public string ConnectionString { get; set; }
        public string Roles { get; set; }
        public bool IsActive { get; set; }
        public int FacultyId { get; set; }
        public string Faculty { get; set; }
    }

    public class UserApplicationListModelV2
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UserApplicationModel> Applications { get; set; }
    }



}