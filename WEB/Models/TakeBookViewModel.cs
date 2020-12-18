using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Models
{
    public class TakeBookViewModel
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
    }
}