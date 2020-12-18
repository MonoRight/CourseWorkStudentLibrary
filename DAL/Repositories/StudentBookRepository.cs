using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class StudentBookRepository : IRepository<StudentBook>
    {
        private LibraryContext db;
        private bool disposedValue = false;

        public StudentBookRepository(LibraryContext context)
        {
            this.db = context;
        }

        public IEnumerable<StudentBook> GetAll()
        {
            return db.StudentBooks;
        }

        public StudentBook Get(int id)
        {
            return db.StudentBooks.Find(id);
        }

        public void Create(StudentBook studentBook)
        {
            db.StudentBooks.Add(studentBook);
        }

        public void Update(StudentBook studentBook)
        {
            if (studentBook != null)
            {
                db.SaveChanges();
            }
        }

        public IEnumerable<StudentBook> Find(Func<StudentBook, Boolean> predicate)
        {
            return db.StudentBooks.Where(predicate).ToList();
        }
        public void Delete(int id)
        {
            StudentBook studentBook = db.StudentBooks.Find(id);
            if (studentBook != null)
                db.StudentBooks.Remove(studentBook);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
