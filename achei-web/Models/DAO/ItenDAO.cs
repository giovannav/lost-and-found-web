using achei_web.Models.POJO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.DAO
{
    public class ItenDAO
    {
        public void insert(Iten iten)
        {
            string name = iten.name;
            string description = iten.description;
            string date_found = iten.date_found.Year.ToString() + "-" + iten.date_found.Month.ToString() + "-" + iten.date_found.Day.ToString();
            string image_name = iten.image_name;
            string image_path = iten.image_path;
            int id = iten.id;
            int status = 0;

            Connection connection = new Connection();
            MySqlConnection Conn = new MySqlConnection();
            MySqlTransaction tran;
            Conn = connection.OpenConnection();
            tran = Conn.BeginTransaction();
            try
            {
                MySqlCommand command1 = Conn.CreateCommand();
                command1.CommandText = "SELECT student_record FROM student WHERE person_id = " + id + ";";
                command1.Connection = Conn;
                command1.Transaction = tran;
                string student_record = command1.ExecuteScalar().ToString();

                MySqlCommand command2 = Conn.CreateCommand();
                command2.CommandText = "INSERT INTO iten (name, description, date_found, image_name, image_path, status, student_record) VALUES " +
                    "('" + name + "', '" + description + "', '" + date_found + "', '" + image_name + "', '" + image_path + "', '" + status + "', '" + student_record + "');";
                command2.Connection = Conn;
                command2.Transaction = tran;
                command2.ExecuteNonQuery();

                tran.Commit();
                connection.CloseConnection();
            }
            catch (Exception ex)
            {
                String erro = ex.InnerException + ex.Message;
                erro += ex.StackTrace;
            }
        }

        public List<Iten> selectObject(String ord)
        {
            List<Iten> listItens = new List<Iten>();

            try
            {
                Connection connection = new Connection();
                MySqlConnection Conn = new MySqlConnection();
                MySqlTransaction tran;
                Conn = connection.OpenConnection();
                tran = Conn.BeginTransaction();

                MySqlCommand command = Conn.CreateCommand();
                command.CommandText = "SELECT id, name, description, date_found, image_name, image_path, status, student_record FROM iten; " + ord;
                command.Connection = Conn;
                command.Transaction = tran;
                MySqlDataReader Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    Iten obj = new Iten();
                    obj.id = Reader.GetInt32(0);
                    obj.name = (!Reader.IsDBNull(1)) ? Reader.GetString(1) : string.Empty;
                    obj.description = (!Reader.IsDBNull(2)) ? Reader.GetString(2) : string.Empty;
                    obj.date_found = (!Reader.IsDBNull(3)) ? Reader.GetDateTime(3) : DateTime.Now;
                    obj.image_name = (!Reader.IsDBNull(4)) ? Reader.GetString(4) : string.Empty;
                    obj.image_path = (!Reader.IsDBNull(5)) ? Reader.GetString(5) : string.Empty;
                    obj.status = (!Reader.IsDBNull(6)) ? Reader.GetBoolean(6) : false;
                    obj.student_record = (!Reader.IsDBNull(7)) ? Reader.GetString(7) : string.Empty;
                    listItens.Add(obj);
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
            return listItens;
        }
    }
}
