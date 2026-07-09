using System.Collections.Generic;

namespace QualificationVerificationWeb.ViewModels
{
    public  class DocumentType
    {
        public DocumentType()
        {
            this.Documents = new HashSet<Document>();
        }
    
        public int DocumentTypeID { get; set; }
        public string Description { get; set; }
        public string ApplicationInd { get; set; }
    
        public virtual ICollection<Document> Documents { get; set; }
    }
}
