using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.POJO
{
    public class Iten
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public DateTime date_found { get; set; } = DateTime.Now;
        public bool status { get; set; } = false;

        public string student_record { get; set; } = string.Empty;
        public IFormFile ImageFile { get; set; } = null;

        public string image_name { get; set; } = string.Empty;

        public string image_path { get; set; } = string.Empty;
    }
}
