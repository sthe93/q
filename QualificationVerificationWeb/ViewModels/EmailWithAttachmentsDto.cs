using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
 
    public class EmailWithAttachmentsDto
    {
        public string From { get; set; }
        public string[] TO { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<EmailAttachmentDto> Attachments { get; set; }
    }
}