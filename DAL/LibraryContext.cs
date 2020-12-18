using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public class LibraryContext : DbContext
    {
        public LibraryContext() : base("DBConnection")
        {
            //Database.Create(); //была ошибка связанна с созданием БД
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentBook> StudentBooks { get; set; }
    }
}
