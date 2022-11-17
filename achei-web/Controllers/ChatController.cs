using achei_web.Models.DAO;
using achei_web.Models.POJO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Controllers
{
    public class ChatController : Controller
    {
        [HttpGet]
        [Route("Index/{id}")]
        public IActionResult Index(int id)
        {
            Room room1 = new Room();
            List<Message> objMessage = new List<Message>();
            ItenDAO itenDAO = new ItenDAO();

            var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
            room1.second_user_id = person.id;
            room1.first_user_id = itenDAO.selectItenOwner(id);

            // Checar se já existe um room com esses usuários neste item e retornar o número do room
            ChatDAO chatDAO = new ChatDAO();
            room1.id_room = Int32.Parse(chatDAO.insert(room1, id).ToString());
            objMessage = chatDAO.selectMessages(room1.id_room);
            var tuple = Tuple.Create(room1, person, objMessage);
            return View(tuple);
        }

        [HttpPost]
        public IActionResult Insert(Message messageobj)
        {
            ChatDAO chatDAO = new ChatDAO();
            var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
            int iten_id = chatDAO.insertMessage(messageobj);
            //Inserir mensagem

            return RedirectToAction("Index", new { @id = iten_id });
        }
    }
}
