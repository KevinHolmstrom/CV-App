using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_App.ViewModels
{
    public class SectionVM
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public List<string> TextRows { get; set; }
        public int ResumeId { get; set; }
        public bool IsInLeftSection { get; set; }

    }
}
