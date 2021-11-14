using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CV_App.ViewModels
{
    public class ResumeVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ProfilePath { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public List<SectionVM>  Sections { get; set; }

        public string UserCookieId { get; set; }
    }
}
