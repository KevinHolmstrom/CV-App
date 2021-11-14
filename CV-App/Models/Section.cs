using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CV_App.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
        [NotMapped]
        public ICollection<TextRow> TextRows { get; set; }
        public bool IsInLeftSection { get; set; }

        //Navigation Properties
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
    }
}
