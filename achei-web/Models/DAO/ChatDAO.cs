using achei_web.Models.POJO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models.DAO
{
    public class ChatDAO
    {
        public object insert(Room room, int id)
        {

            int first_user_id = room.first_user_id;
            int second_user_id = room.second_user_id;
            int iten_id = id;
            object userNameObj = null;
            int last_insert_room = 0;
            Connection connection = new Connection();
            MySqlConnection Conn = new MySqlConnection();
            MySqlTransaction tran;
            Conn = connection.OpenConnection();
            tran = Conn.BeginTransaction();
            try
            {
                MySqlCommand command1 = Conn.CreateCommand();
                command1.CommandText = "SELECT id_room FROM room INNER JOIN iten_has_room ON room.id_room = iten_has_room.room_id"+
                                      " WHERE first_user_id = "+first_user_id+" and second_user_id = "+second_user_id+" and iten_id = "+iten_id+";";
                command1.Connection = Conn;
                command1.Transaction = tran;
                userNameObj = command1.ExecuteScalar();
                tran.Commit();
                connection.CloseConnection();

                if (userNameObj == null)
                {
                    Conn = connection.OpenConnection();
                    tran = Conn.BeginTransaction();
                    //insert into room (first_user_id, second_user_id) values (28, 29);
                    MySqlCommand command2 = Conn.CreateCommand();
                    command2.CommandText = "INSERT INTO ROOM (first_user_id, second_user_id) VALUES ("+first_user_id+", "+second_user_id+")";
                    command2.Connection = Conn;
                    command2.Transaction = tran;
                    command2.ExecuteNonQuery();

                    MySqlCommand command3 = Conn.CreateCommand();
                    command3.CommandText = "SELECT LAST_INSERT_ID();";
                    command3.Connection = Conn;
                    command3.Transaction = tran;
                    last_insert_room = Int32.Parse(command3.ExecuteScalar().ToString());

                    // insert into iten_has_room (iten_id, room_id) values (2, 1);
                    MySqlCommand command4 = Conn.CreateCommand();
                    command4.CommandText = "INSERT INTO ITEN_HAS_ROOM (iten_id, room_id) VALUES (" + iten_id + ", " + last_insert_room + ")";
                    command4.Connection = Conn;
                    command4.Transaction = tran;
                    command4.ExecuteNonQuery();

                    tran.Commit();
                    connection.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                String erro = ex.InnerException + ex.Message;
                erro += ex.StackTrace;
            }
            return userNameObj;
        }

        public int insertMessage(Message message)
        {
            int room_id = 0;
            Connection connection = new Connection();
            MySqlConnection Conn = new MySqlConnection();
            MySqlTransaction tran;
            Conn = connection.OpenConnection();
            tran = Conn.BeginTransaction();
            try
            {
                //insert into message(message, id_user, date_send, room_id) values 
                //('Bom dia', 29, current_timestamp(), 1);
                MySqlCommand command1 = Conn.CreateCommand();
                command1.CommandText = "INSERT INTO message (message, id_user, date_send, room_id) VALUES ('"+message.message+"', "+message.id_user+", current_timestamp(), "+message.room_id+");";
                command1.Connection = Conn;
                command1.Transaction = tran;
                command1.ExecuteNonQuery();

                MySqlCommand command2 = Conn.CreateCommand();
                command2.CommandText = "SELECT iten_id FROM iten_has_room WHERE room_id = "+message.room_id+";";
                command2.Connection = Conn;
                command2.Transaction = tran;
                room_id = Int32.Parse(command2.ExecuteScalar().ToString());

                tran.Commit();
                connection.CloseConnection();
            }
            catch (Exception ex)
            {
                String erro = ex.InnerException + ex.Message;
                erro += ex.StackTrace;
            }
            return room_id;
        }

        public List<Message> selectMessages(int room)
        {
            List<Message> listMessages = new List<Message>();

            try
            {
                Connection connection = new Connection();
                MySqlConnection Conn = new MySqlConnection();
                MySqlTransaction tran;
                Conn = connection.OpenConnection();
                tran = Conn.BeginTransaction();

                MySqlCommand command = Conn.CreateCommand();
                command.CommandText = "SELECT id_message, message, id_user, date_send, message.room_id FROM message "+
                                        "INNER JOIN iten_has_room ON message.room_id = iten_has_room.room_id "+
                                        "INNER JOIN room ON iten_has_room.room_id = room.id_room "+
                                        "WHERE room.id_room = "+room+" ORDER BY date_send;";
                command.Connection = Conn;
                command.Transaction = tran;
                MySqlDataReader Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    Message obj = new Message();
                    obj.id = Reader.GetInt32(0);
                    obj.message = (!Reader.IsDBNull(1)) ? Reader.GetString(1) : string.Empty;
                    obj.id_user = Reader.GetInt32(2);
                    obj.date_send = (!Reader.IsDBNull(3)) ? Reader.GetDateTime(3) : DateTime.Now;
                    obj.room_id = Reader.GetInt32(4);
                    listMessages.Add(obj);
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
            return listMessages;
        }

    }
}
