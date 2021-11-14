using CV_App.Data;
using CV_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_App.Repository
{
    public class ResumeRepo : IResumeRepo
    {
        private readonly ApplicationDbContext _context;

        public ResumeRepo(ApplicationDbContext db)
        {
            _context = db;
        }

        public bool Create(Resume entity)
        {
            _context.Add(entity);
            return Save();
        }

        public bool Delete(Resume entity)
        {
            _context.Remove(entity);
            return Save();
        }

        public ICollection<Resume> FindAll()
        {
            return _context.Resumes.ToList();
        }

        public Resume FindByCookie(string Cookie)
        {
            return FindAll().FirstOrDefault(q => q.UserCookieId == Cookie);
        }

        public Resume FindById(int id)
        {
            return _context.Resumes.Find(id);
        }

        public bool isExists(int id)
        {
            return _context.Resumes.Any(q => q.Id == id);
        }

        public bool Save()
        {
            var changes = _context.SaveChanges();
            if (changes > 0) { return true; }
            else { return false; }
        }

        public bool Update(Resume entity)
        {
            _context.Update(entity);
            return Save();
        }
    }
}
