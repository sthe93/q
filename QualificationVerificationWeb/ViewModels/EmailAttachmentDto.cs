using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class EmailAttachmentDto
    {
        public string AttachmentName { get; set; }
        public byte[] Attachment { get; set; }
        public string Extension { get; set; }
    }
}