using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class GetAcademicDocumentTypePrice
    {
        public int Id { get; set; }
        public string DocumentType { get; set; }
        public string Collect { get; set; }
        public string Courier___South_Africa { get; set; }
        public string Courier___International { get; set; }
        public string TurnAroundTime { get; set; }
    }
}