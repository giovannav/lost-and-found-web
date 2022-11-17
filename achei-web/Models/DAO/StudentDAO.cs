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

        public List<Student> selectObject(String ord)
        {
            List<Student> listStudent = new List<Student>();

            try
            {
                Connection connection = new Connection();
                MySqlConnection Conn = new MySqlConnection();
                MySqlTransaction tran;
                Conn = connection.OpenConnection();
                tran = Conn.BeginTransaction();

                MySqlCommand command = Conn.CreateCommand();
                command.CommandText = "SELECT id, name, email, password, phone, type_user, birthday, admin_cpf, student_record FROM person" +
                    " INNER JOIN student ON person.id = student.person_id " + ord;
                command.Connection = Conn;
                command.Transaction = tran;
                MySqlDataReader Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    Student obj = new Student();
                    obj.id = Reader.GetInt32(0);
                    obj.name = (!Reader.IsDBNull(1)) ? Reader.GetString(1) : string.Empty;
                    obj.email = (!Reader.IsDBNull(2)) ? Reader.GetString(2) : string.Empty;
                    obj.password = (!Reader.IsDBNull(3)) ? Reader.GetString(3) : string.Empty;
                    obj.phone = (!Reader.IsDBNull(4)) ? Reader.GetString(4) : string.Empty;
                    obj.type_user = Reader.GetInt32(5);
                    obj.birthday = (!Reader.IsDBNull(6)) ? Reader.GetDateTime(6) : DateTime.Now;
                    obj.admin_cpf = (!Reader.IsDBNull(7)) ? Reader.GetString(7) : string.Empty;
                    obj.student_record = (!Reader.IsDBNull(8)) ? Reader.GetString(8) : string.Empty;

                    listStudent.Add(obj);
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
            return listStudent;
        }

        public Student authenticate(Student student)
        {
            bool email_correct = false;
            bool password_correct = false;
            List<Student> listObj = new List<Student>();
            listObj = selectObject("");
            int index = 0;
            foreach (var user in listObj)
            {
                index++;
                if (student.email.Equals(user.email))// (objC.email == user.email)
                {
                    email_correct = true;
                }

                if (email_correct)
                {
                    if (student.password.Equals(user.password))
                    {
                        password_correct = true;
                        break;
                    }
                }
            }
            if (email_correct == true && password_correct == true)
            {
                return listObj[index - 1];
            }
            return null;
        }
    }
}
