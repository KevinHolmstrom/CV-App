using CV_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_App.Repository
{
    public interface IResumeRepo : IRepoBase<Resume>
    {
        Resume FindByCookie(string id);
    }
}
