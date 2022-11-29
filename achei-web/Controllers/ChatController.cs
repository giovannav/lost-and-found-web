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
        public IActionResult Index(int id) //room id
        {
            Room room1 = new Room();
            List<Message> objMessage = new List<Message>();
            ItenDAO itenDAO = new ItenDAO();
            try
            {
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                ChatDAO chatDAO = new ChatDAO();
                objMessage = chatDAO.selectMessages(id);
                room1.id_room = id;

                var tuple = Tuple.Create(person, objMessage, room1);
                return View(tuple);
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }
        }

        [HttpPost]
        public IActionResult Insert(Message messageobj)
        {
            ChatDAO chatDAO = new ChatDAO();

            try
            {
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                return new EmptyResult();
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }
        }
    }
}
