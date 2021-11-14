using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CV_App.Models
{
    public class TextRow
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }

        //Navigation properties
        public int SectionId { get; set; }
        [NotMapped]
        public Section Section { get; set; }
    }
}
