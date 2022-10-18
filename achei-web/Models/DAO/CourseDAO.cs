using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using achei_web.Models.POJO;
using MySql.Data.MySqlClient;

namespace achei_web.Models.DAO
{
    public class CourseDAO
    {
        public List<Course> selectObject(String ord)
        {
            List<Course> listObj = new List<Course>();
            try
            {
                Connection objCon = new Connection();
                MySqlConnection Conn = new MySqlConnection();
                Conn = objCon.OpenConnection();

                MySqlCommand command = Conn.CreateCommand();
                command.CommandText = "select id, name, course_time from course " + ord;
                MySqlDataReader Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    Course obj = new Course();
                    obj.id = Reader.GetInt32(0);
                    obj.name = (!Reader.IsDBNull(1)) ? Reader.GetString(1) : string.Empty;
                    obj.course_time = (!Reader.IsDBNull(2)) ? Reader.GetString(2) : string.Empty;
                    listObj.Add(obj);
                }
                command.Dispose();
                objCon.CloseConnection();
            }
            catch(Exception ex)
            {
                String erro = ex.InnerException + ex.Message;
                erro += ex.StackTrace;
            }
            return listObj;
        }

    }
}
