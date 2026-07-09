using System;

namespace QualificationVerificationWeb.Admin.Models
{
    public class ApplicationViewModel
    {
        public int AppId { get; set; }
        public Guid ApplicationGuid { get; set; }
        public string AppName { get; set; }
        public string AppDescription { get; set; }
        public string AppIcon { get; set; }
        public string AppUrl { get; set; }
        public string ConnectionString { get; set; }
        public string Roles { get; set; }
    }
}
