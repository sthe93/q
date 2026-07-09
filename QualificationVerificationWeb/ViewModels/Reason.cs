using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QualificationVerificationWeb.ViewModels
{
    public class Reason
    {
        public Reason()
        {
            this.Students = new HashSet<Student>();
        }

        public int ReasonID { get; set; }
        [Column("Reason")]
        public string Reason1 { get; set; }
     
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public string AppInd { get; set; }
        public bool IsActive { get; set; }   
        public string EmailInstruction { get; set; }
        //[JsonIgnore]
        public virtual ICollection<Student> Students { get; set; }

    }
}
