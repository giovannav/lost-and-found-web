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

            Connection connection = new Connection();
            MySqlConnection Conn = new MySqlConnection();
            MySqlTransaction tran;
            Conn = connection.OpenConnection();
            tran = Conn.BeginTransaction();
            try
            {
                MySqlCommand command1 = Conn.CreateCommand();
                command1.CommandText = "INSERT INTO person (name, email, password, phone, type_user, birthday, admin_cpf) VALUES ('"+name+"', '"+email+ "', '" + password + "','" + phone + "'," + type_user + ", '" + birthday + "', '" + admin_cpf + "');";
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

        public List<Person> selectObject(String ord)
        {
            List<Person> listPerson = new List<Person>();

            try
            {
                Connection connection = new Connection();
                MySqlConnection Conn = new MySqlConnection();
                MySqlTransaction tran;
                Conn = connection.OpenConnection();
                tran = Conn.BeginTransaction();

                MySqlCommand command = Conn.CreateCommand();
                command.CommandText = "SELECT id, name, email, password FROM person "+ord;
                command.Connection = Conn;
                command.Transaction = tran;
                MySqlDataReader Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    Person obj = new Person();
                    obj.id = Reader.GetInt32(0);
                    obj.name = (!Reader.IsDBNull(1)) ? Reader.GetString(1) : string.Empty;
                    obj.email = (!Reader.IsDBNull(2)) ? Reader.GetString(2) : string.Empty;
                    obj.password = (!Reader.IsDBNull(3)) ? Reader.GetString(3) : string.Empty;

                    listPerson.Add(obj);
                }
                command.Dispose();
                tran.Commit();
                connection.CloseConnection();
            }
            catch (Exception ex)
            {
                String erro = ex.InnerException + ex.Message;
                erro += ex.StackTrace;
            }
            return listPerson;
        }

        public bool authenticate(Person person)
        {
            bool email_correct = false;
            bool password_correct = false;
            List<Person> listObj = new List<Person>();
            listObj = selectObject("");

            foreach (var user in listObj)
            {
                if (person.email.Equals(user.email))// (objC.email == user.email)
                {
                    email_correct = true;
                }

                if (email_correct)
                {
                    if (person.password.Equals(user.password))
                    {
                        password_correct = true;
                        break;
                    }
                }
            }
            if (email_correct == true && password_correct == true)
            {
                return true;
            }
            return false;
        }

    }
}
