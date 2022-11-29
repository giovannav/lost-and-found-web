using achei_web.Models.DAO;
using achei_web.Models.POJO;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Models
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message, string room_id, string id_user)
        {
            //await Clients.User("NomedoUser").SendAsync("ReceiveMessage", user, message);
            await Clients.All.SendAsync("ReceiveMessage", user, id_user, message);
            ChatDAO chatDAO = new ChatDAO();
            Message message1 = new Message();
            message1.message = message;
            message1.room_id = Int32.Parse(room_id);
            message1.id_user = Int32.Parse(id_user);
            chatDAO.insertMessage(message1);
        }
    }
}
