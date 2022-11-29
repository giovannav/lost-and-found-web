using achei_web.Models.POJO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.DAO
{
    public class NotificationDAO
    {
        public List<Notification> selectObject(int touser)
        {
            List<Notification> listNotification = new List<Notification>();

            try
            {
                Connection connection = new Connection();
                MySqlConnection Conn = new MySqlConnection();
                MySqlTransaction tran;
                Conn = connection.OpenConnection();
                tran = Conn.BeginTransaction();

                MySqlCommand command = Conn.CreateCommand();
                command.CommandText = "SELECT count(notification.id), notification.id_to_user, notification.id_from_user, " +
                            "notification.id_room, notification.id_iten, notification.is_read, iten.name, person.name, notification.date_send FROM notification " +
                            "INNER JOIN iten ON notification.id_iten = iten.id " +
                            "INNER JOIN person ON notification.id_from_user = person.id " +
                            "WHERE id_to_user = " + touser + " " +
                            "GROUP BY notification.id_from_user, notification.id_room, notification.id_iten " +
                            "ORDER BY date_send DESC;";
                command.Connection = Conn;
                command.Transaction = tran;
                MySqlDataReader Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    Notification obj = new Notification();
                    obj.id = Reader.GetInt32(0);
                    obj.id_to_user = Reader.GetInt32(1);
                    obj.id_from_user = Reader.GetInt32(2);
                    obj.id_room = Reader.GetInt32(3);
                    obj.id_iten = Reader.GetInt32(4);
                    obj.is_read = Reader.GetInt32(5);
                    obj.iten.name = (!Reader.IsDBNull(6)) ? Reader.GetString(6) : string.Empty;
                    obj.person.name = (!Reader.IsDBNull(7)) ? Reader.GetString(7) : string.Empty;
                    obj.date_send = (!Reader.IsDBNull(8)) ? Reader.GetDateTime(8) : DateTime.Now;

                    listNotification.Add(obj);
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
            return listNotification;
        }

    }
}
