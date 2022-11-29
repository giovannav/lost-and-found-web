using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.POJO
{
    public class Notification
    {
        public int id { get; set; } = 0;
        public int id_to_user { get; set; } = 0;
        public int id_from_user { get; set; } = 0;

        public int id_room { get; set; } = 0;
        public int id_iten { get; set; } = 0;
        public int is_read { get; set; } = 0;

        public DateTime date_send { get; set; } = DateTime.Now;

        public Person person { get; set; } = new Person();
        public Iten iten { get; set; } = new Iten();

    }
}
