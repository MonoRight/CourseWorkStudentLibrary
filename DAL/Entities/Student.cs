using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GroupName { get; set; }
        public ICollection<StudentBook> StudentBooks { get; set; }
        public Student()
        {
            StudentBooks = new List<StudentBook>();
        }
    }
}
