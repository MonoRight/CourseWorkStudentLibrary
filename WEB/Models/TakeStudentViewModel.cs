using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Models
{
    public class TakeStudentViewModel
    {
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GroupName { get; set; }
    }
}