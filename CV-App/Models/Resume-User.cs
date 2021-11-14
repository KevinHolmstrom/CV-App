using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_App.Models
{
    public class Resume_User
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int ResumeId { get; set; }
    }
}
