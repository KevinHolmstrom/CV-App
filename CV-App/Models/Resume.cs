using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CV_App.Models
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ProfilePath { get; set; }
        [NotMapped]
        public IFormFile ProfilePicture { get; set; }
        [NotMapped]
        public ICollection<Section> Sections { get; set; }

        public string UserCookieId { get; set; }
    }
}
