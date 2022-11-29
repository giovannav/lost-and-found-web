using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.POJO
{
    public class Message
    {
        public int id { get; set; } = 0;
        public string message { get; set; } = string.Empty;
        public int id_user { get; set; } = 0;
        public DateTime date_send { get; set; } = DateTime.Now;
        public int room_id { get; set; } = 0;

        public Person person { get; set; } = new Person();

    }
}
