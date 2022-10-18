using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace achei_web.Models.POJO
{
    public class Person
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public DateTime birthday { get; set; } = DateTime.Now;
        public int type_user { get; set; } = 0;

        public string admin_cpf { get; set; } = string.Empty;
    }
}
