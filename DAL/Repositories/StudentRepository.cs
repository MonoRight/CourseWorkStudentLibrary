using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Entities;

namespace DAL.Repositories
{
    public class StudentRepository :IRepository<Student>
    {
        private LibraryContext db;
        private bool disposedValue = false;

        public StudentRepository(LibraryContext context)
        {
            this.db = context;
        }

        public IEnumerable<Student> GetAll()
        {
            return db.Students;
        }

        public Student Get(int id)
        {
            return db.Students.Find(id);
        }

        public void Create(Student student)
        {
            db.Students.Add(student);
        }

        public void Update(Student student)
        {
            if (student != null)
            {
                db.SaveChanges();
            }
        }

        public IEnumerable<Student> Find(Func<Student, Boolean> predicate)
        {
            return db.Students.Where(predicate).ToList();
        }
        public void Delete(int id)
        {
            Student student = db.Students.Find(id);
            if (student != null)
                db.Students.Remove(student);
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
