using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.POJO
{
    public class Room
    {
        public int id_room { get; set; } = 0;
        public int first_user_id { get; set; } = 0;

        public int second_user_id { get; set; } = 0;
    }
}
