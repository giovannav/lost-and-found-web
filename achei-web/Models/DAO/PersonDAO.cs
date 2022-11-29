using achei_web.Models.POJO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.DAO
{
    public class PersonDAO
    {
        public Int32 insert(Student student)
        {
            string name = student.name;
            string email = student.email;
            string password = student.password;
            string phone = student.phone;
            int type_user = student.type_user;
            string admin_cpf = student.admin_cpf;
            string birthday = student.birthday.Year.ToString() + "-" + student.birthday.Month.ToString() + "-" + student.birthday.Day.ToString();
            int id = 0;

            //BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            // ----------------------------------------------------------

            Connection connection = new Connection();
            MySqlConnection Conn = new MySqlConnection();
            MySqlTransaction tran;
            Conn = connection.OpenConnection();
            tran = Conn.BeginTransaction();
            try
            {
                MySqlCommand command1 = Conn.CreateCommand();
                command1.CommandText = "INSERT INTO person (name, email, password, phone, type_user, birthday, admin_cpf) VALUES ('"+name+"', '"+email+ "', '" + passwordHash + "','" + phone + "'," + type_user + ", '" + birthday + "', '" + admin_cpf + "');";
                command1.Connection = Conn;
                command1.Transaction = tran;
                command1.ExecuteNonQuery();

                MySqlCommand command2 = Conn.CreateCommand();
                command2.CommandText = "SELECT LAST_INSERT_ID();";
                command2.Connection = Conn;
                command2.Transaction = tran;
                id = Int32.Parse(command2.ExecuteScalar().ToString());
               
                tran.Commit();
                connection.CloseConnection();
            }
            catch (Exception ex)
            {
                String erro = ex.InnerException + ex.Message;
                erro += ex.StackTrace;
            }
            return id;
        }
    }
}
