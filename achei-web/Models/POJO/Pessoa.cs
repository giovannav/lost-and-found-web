using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace achei_web.Models.POJO
{
    public class Pessoa
    {
        public int id { get; set; } = 0;
        public string nome { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string senha { get; set; } = string.Empty;
        public string telefone { get; set; } = string.Empty;
        public DateTime data_nascimento { get; set; } = DateTime.Now;

        public int tipo_usuario { get; set; } = 0;

    }
}
