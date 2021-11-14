using CV_App.Data;
using CV_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CV_App.Repository
{
    public class TextRowRepo : ITextRowRepo
    {
        private readonly ApplicationDbContext _context;

        public TextRowRepo(ApplicationDbContext db)
        {
            _context = db;
        }

        public bool Create(TextRow entity)
        {
            _context.Add(entity);
            return Save();
        }

        public bool Delete(TextRow entity)
        {
            _context.Remove(entity);
            return Save();
        }

        public ICollection<TextRow> FindAll()
        {
            return _context.TextRows.ToList();
        }


        public TextRow FindById(int id)
        {
            return _context.TextRows.Find(id);
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

        public bool Update(TextRow entity)
        {
            _context.Update(entity);
            return Save();
        }
    }
}
