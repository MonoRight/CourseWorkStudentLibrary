using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;

namespace DAL
{
    public class UnitOfWork :IUnitOfWork
    {
        private LibraryContext db;
        private BookRepository bookRepository;
        private StudentRepository studentRepository;
        private StudentBookRepository studentBookRepository;
        private bool disposed = false;

        public UnitOfWork()
        {
            db = new LibraryContext();
        }
        public IRepository<Book> Books
        {
            get
            {
                if (bookRepository == null)
                {
                    bookRepository = new BookRepository(db);
                }
                return bookRepository;
            }
        }

        public IRepository<Student> Students
        {
            get
            {
                if (studentRepository == null)
                {
                    studentRepository = new StudentRepository(db);
                }
                return studentRepository;
            }
        }

        public IRepository<StudentBook> StudentBooks
        {
            get
            {
                if (studentBookRepository == null)
                {
                    studentBookRepository = new StudentBookRepository(db);
                }
                return studentBookRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
