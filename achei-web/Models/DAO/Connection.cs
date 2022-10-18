using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.DAO
{
    public class Connection
    {
        private MySqlConnection Conn;

        public MySqlConnection OpenConnection()
        {
            Conn = new MySqlConnection("server=127.0.0.1;database=lostandfound;uid=root;pwd=admin;convert zero datetime=True");
            // convert zero datetime=True --> Usado para permitir receber o datetime do mysql no datetime do c#
            Conn.Open();
            return Conn;
        }
        public void CloseConnection()
        {
            Conn.Close();
        }
    }
}
