using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Models
{
    public class TakeStudentBookViewModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public string StudentName { get; set; }
        public string Surname { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
    }
}