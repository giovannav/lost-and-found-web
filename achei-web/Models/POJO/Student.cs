using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.POJO
{
    public class Student : Person
    {
        public string student_record { get; set; } = string.Empty;
        public List<Iten> itens { get; set; } = null;
        public Course course { get; set; } = new Course();
    }
}
