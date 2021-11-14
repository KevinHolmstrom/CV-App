using CV_App.Data;
using CV_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_App.Repository
{
    public class SectionRepo : ISectionRepo
    {
        private readonly ApplicationDbContext _context;

        public SectionRepo(ApplicationDbContext db)
        {
            _context = db;
        }

        public bool Create(Section entity)
        {
            _context.Add(entity);
            return Save();
        }

        public bool Delete(Section entity)
        {
            _context.Remove(entity);
            return Save();
        }

        public ICollection<Section> FindAll()
        {
            return _context.Sections.ToList();
        }


        public Section FindById(int id)
        {
            return _context.Sections.Find(id);
        }

        public bool isExists(int id)
        {
            return _context.Sections.Any(q => q.Id == id);
        }

        public bool Save()
        {
            var changes = _context.SaveChanges();
            if (changes > 0) { return true; }
            else { return false; }
        }

        public bool Update(Section entity)
        {
            _context.Update(entity);
            return Save();
        }
    }
}
