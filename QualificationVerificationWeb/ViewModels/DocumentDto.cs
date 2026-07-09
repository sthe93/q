using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace QualificationVerificationWeb.ViewModels
{
    [Serializable]
    public class DocumentDto
    {
        public int DocumentID { get; set; }
        public int? DocumentTypeID { get; set; }
        public string Document1 { get; set; }
        public string DocumentFile { get; set; }
        [ScriptIgnore]
        public byte[] DocumentFileBytes =>
            !string.IsNullOrEmpty(DocumentFile) ? Convert.FromBase64String(DocumentFile) : null;

        public DateTime? CreatedOnDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DocumentStatus { get; set; }
    }
}