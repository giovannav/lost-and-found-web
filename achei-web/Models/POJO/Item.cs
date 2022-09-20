using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.POJO
{
    public class Item
    {
        public int id { get; set; } = 0;
        public string nome { get; set; } = string.Empty;
        public string descricao { get; set; } = string.Empty;
        public DateTime data_achado { get; set; } = DateTime.Now;
        public bool status { get; set; } = false;

        public IFormFile ImageFile { get; set; } = null;

        public string image_name { get; set; } = string.Empty;

        public string image_path { get; set; } = string.Empty;
    }
}
