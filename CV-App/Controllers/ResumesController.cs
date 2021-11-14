using CV_App.Data;
using CV_App.Models;
using CV_App.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CV_App.Repository;

namespace CV_App.Controllers
{
    public class ResumesController : Controller
    {
        private readonly IResumeRepo _resumeRepo;
        private readonly ISectionRepo _sectionRepo;
        private readonly ITextRowRepo _textRowRepo;
        private readonly IWebHostEnvironment _environment;


        public ResumesController(IWebHostEnvironment environment, IResumeRepo ResumeRepo, ISectionRepo SectionRepo, ITextRowRepo TextRowRepo)
        {
            _textRowRepo = TextRowRepo;               
            _resumeRepo = ResumeRepo;
            _sectionRepo = SectionRepo;
            _environment = environment;
        }

        //Views
        public IActionResult Index()
        {
            var resumes = _resumeRepo.FindAll();
            List<ResumeVM> model = new List<ResumeVM> { };
            foreach (Resume resume in resumes)
                {
                model.Add(ResumeToResumeVM(resume.Id));
                }

            return View(model);
        }

        public IActionResult Create()
        {
            var model = ResumeToResumeVM(CheckCookie().Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ResumeVM resumeVM)
        {
            Resume model = ResumeVMToResume(resumeVM);

            string wwwRootPath = _environment.WebRootPath;

            if(model.ProfilePicture == null) { } //No picture has been input. Nothing needs to be done
            else if(model.ProfilePath == null)   //No previous picture exists
                {
                AddPictureToWWWRoot(model);
                }
            else                                 //Delete Previous picture before adding new picture to wwwroot
            {
                DeletePictureAtPath(model.ProfilePath);
                AddPictureToWWWRoot(model);
            }

            bool isSuccess = _resumeRepo.Update(model);
            if (!isSuccess)
            {
                ModelState.AddModelError("", "Something Went Wrong, No Changes Made.");
                return View(model);
            }


            
            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteSection(int id)
        {
            var section = _sectionRepo.FindById(id);
            _sectionRepo.Delete(section);

            return RedirectToAction("Create");
        }

        public ActionResult ConstructSection(bool isInLeftSection)
        {
            Section section = CreateSection(isInLeftSection);
            SectionVM model = SectionToSectionVM(section.Id);
            return RedirectToAction("Section", new { id = model.Id });
        }

        public ActionResult Section(int id)
        {
            Section section = _sectionRepo.FindById(id);
            SectionVM sectionVM = SectionToSectionVM(section.Id);

            return View(sectionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Section(SectionVM VM)
        {
            Section section = SectionVMToSection(VM);
            bool isSuccess = _sectionRepo.Update(section);
            if (!isSuccess)
            {
                ModelState.AddModelError("", "Something Went Wrong, No Changes Made.");
                return View(section);
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }
        }
            
   

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTextSection(int Id, string text)
        {
            TextRow textrow = new TextRow
            {
                SectionId = Id,
                Text = text
            };
            _textRowRepo.Create(textrow);
             return RedirectToAction("Section", new { id = Id });
        }

            //Functions------------------------------------------------------------------------------------------------

        public ResumeVM ResumeToResumeVM(int ResumeId)
            {
            //Find Resume by Id and find all sections connected to that resume
            Resume resume = _resumeRepo.FindById(ResumeId);
            //List<Section> sections = _context.Sections.Where(q => q.ResumeId == ResumeId).ToList();
            List<Section> sections = _sectionRepo.FindAll().Where(q => q.ResumeId == resume.Id).ToList();
            List<SectionVM> sectionVMs = new List<SectionVM> { };

            //For each section in resume we find all textrows. Add all textrow value to section as list of string
            foreach (Section section in sections)
            {
                section.TextRows = _textRowRepo.FindAll().Where(q => q.SectionId == section.Id).ToList();
                List<string> TextRowsStrings = new List<string> { };
                foreach(TextRow textRow in section.TextRows)
                {
                    TextRowsStrings.Add(textRow.Text);
                }
                SectionVM sectionVM = new SectionVM
                {
                    Id = section.Id,
                    Label = section.Label,
                    IsInLeftSection = section.IsInLeftSection,
                    TextRows = TextRowsStrings
                };
                sectionVMs.Add(sectionVM);
            }

            //Build resumeVM using sections and return resumeVM
            ResumeVM resumeVM = new ResumeVM
            {
                Id = resume.Id,
                UserCookieId = resume.UserCookieId,
                Title = resume.Title,
                ProfilePath = resume.ProfilePath,
                ProfilePicture = resume.ProfilePicture,   
                Sections = sectionVMs
            };

            return resumeVM;
            }

        public Resume ResumeVMToResume(ResumeVM VM)
        {
            List<Section> sections = new List<Section> { };

            if (VM.Sections != null)
            {
                foreach (var sectionVM in VM.Sections)
                {
                    Section section = new Section
                    {
                        Id = sectionVM.Id,
                        Label = sectionVM.Label,
                        IsInLeftSection = sectionVM.IsInLeftSection
                    };
                    foreach (string TextRow in sectionVM.TextRows)
                    {
                        section.TextRows.Add(new TextRow { Text = TextRow });
                    };

                    sections.Add(section);
                }
            }

            Resume resume = new Resume
            {
                Id = VM.Id,
                ProfilePath = VM.ProfilePath,
                ProfilePicture = VM.ProfilePicture,
                Title = VM.Title,
                UserCookieId = VM.UserCookieId       
            };

            return resume;
        }

        public Section SectionVMToSection(SectionVM VM)
        {
            List<TextRow> textRows = new List<TextRow> { };
            if(VM.TextRows != null) {
                foreach (string textRow in VM.TextRows)
                {
                    TextRow row = new TextRow
                    {
                    SectionId = VM.Id,
                    Text = textRow
                    };
                    textRows.Add(row);
                }
            }

            Section section = new Section
            {
                Id = VM.Id,
                ResumeId = VM.ResumeId,
                Label = VM.Label,
                IsInLeftSection = VM.IsInLeftSection,
                TextRows = textRows
            };

            return section;
        }

        public SectionVM SectionToSectionVM(int sectionId)
        {
            Section section = _sectionRepo.FindById(sectionId);

             section.TextRows = _textRowRepo.FindAll().Where(q => q.SectionId == section.Id).ToList();

            List<string> textRows = new List<string> { };
            if (section.TextRows != null)
            {
                foreach (TextRow row in section.TextRows)
                {
                    textRows.Add(row.Text);
                }
            }

            SectionVM ViewModel = new SectionVM
            {
                Id = section.Id,
                Label = section.Label,
                IsInLeftSection = section.IsInLeftSection,
                ResumeId = section.ResumeId,
                TextRows = textRows
            };

            return ViewModel;

        }

        public Resume CheckCookie()
        {
            //Get Access Key
            var Cookie = Request.Cookies["AccessKey"];

            //If no Cookie exists, create one, connect it to a new resume, save resume to DB and return that resume
            if (Cookie == null)
            {
                string guid = Guid.NewGuid().ToString();
                Response.Cookies.Append("AccessKey", guid, new CookieOptions { Expires = new DateTimeOffset(2030, 1, 1, 0, 0, 0, TimeSpan.FromHours(0)) }); ;
                Resume resume = new Resume
                {
                    UserCookieId = guid
                };
                _resumeRepo.Create(resume);
                return resume;
            }
            
            //If Cookie exists find and return resume connected to cookieId
            else
            {
                Resume resume = _resumeRepo.FindByCookie(Cookie);
                if (resume != null)
                {
                    return resume;
                }
                else
                {
                    Resume resume1 = new Resume
                    {
                        UserCookieId = Cookie
                    };
                    _resumeRepo.Create(resume1);
                    return resume1;
                }
            }
        }

        public Section CreateSection(bool isInLeftSection)
        {
            //Get resume in which to connect section to
            Resume resume = _resumeRepo.FindByCookie(Request.Cookies["AccessKey"]);
            //Create new section and add it to database
            Section section = new Section
            {
                ResumeId = resume.Id,
                IsInLeftSection = isInLeftSection
            };

            if (_sectionRepo.Create(section)) 
            {
            return section;
            }
            else
            {
                return null;
            }
        }

        public void AddPictureToWWWRoot(Resume model)
        {
            string wwwRootPath = _environment.WebRootPath;

            string fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
            string extension = Path.GetExtension(model.ProfilePicture.FileName);
            string fileNamewithExtension = fileName + DateTime.UtcNow.ToString("yymmssfff") + extension;
            model.ProfilePath = fileNamewithExtension;
            string pathToFile = Path.Combine(wwwRootPath + "/Profiles/", fileNamewithExtension);
            using (var fileStream = new FileStream(pathToFile, FileMode.Create))
            {
                model.ProfilePicture.CopyTo(fileStream);
            }
        }

        public void DeletePictureAtPath(string ProfilePath)
        {
            string FullPath = Path.Combine(_environment.WebRootPath, "Profiles", ProfilePath);
            if (System.IO.File.Exists(FullPath))
            {
                System.IO.File.Delete(FullPath);
            }
        }

    }
}
