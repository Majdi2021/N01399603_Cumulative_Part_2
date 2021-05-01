using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01399603_Cumulative_Part_2.Models
{
    public class Teacher
    {
        public Teacher() { }
        public int teacherid { get; set; }
        public string teacherfname { get; set; }
        public string teacherlname { get; set; }
        public string employeenumber { get; set; }
        public DateTime? hiredate { get; set; }
        public decimal? salary { get; set; }
    }
}