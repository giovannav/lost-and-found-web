using achei_web.Models.POJO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.DAO
{
    public class StudentDAO
    {
        public void insert(Student student)
        {
            PersonDAO personDAO = new PersonDAO();
            int id = personDAO.insert(student);
            string student_record = student.student_record;
            int curso = student.course.id;

            Connection connection = new Connection();
            MySqlConnection Conn = new MySqlConnection();
            MySqlTransaction tran;
            Conn = connection.OpenConnection();
            tran = Conn.BeginTransaction();
            try
            {
                MySqlCommand command1 = Conn.CreateCommand();
                command1.CommandText = "INSERT INTO student (student_record, person_id) VALUES ('" + student_record + "', "+id+");";
                command1.Connection = Conn;
                command1.Transaction = tran;
                command1.ExecuteNonQuery();

                MySqlCommand command2 = Conn.CreateCommand();
                command2.CommandText = "INSERT INTO student_has_course (student_record, course_id) VALUES ('" + student_record + "', " + curso + ");";
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
    }
}
