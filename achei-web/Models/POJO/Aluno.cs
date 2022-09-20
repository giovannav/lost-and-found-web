using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.POJO
{
    public class Aluno : Pessoa
    {
        public string prontuario { get; set; } = string.Empty;

        public List<Item> items = null;
    }
}
