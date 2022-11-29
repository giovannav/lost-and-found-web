using achei_web.Models.DAO;
using achei_web.Models.POJO;
using Microsoft.AspNetCore.Hosting;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using X.PagedList;
using X.PagedList.Mvc.Core;


namespace achei_web.Controllers
{
    public class ItenController : Controller
    {

        private IWebHostEnvironment _appEnvironment;

        public ItenController(IWebHostEnvironment env)
        {
            _appEnvironment = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Read/{id}")]
        public IActionResult Read(Iten iten)
        {
            ItenDAO itenDAO = new ItenDAO();

            try
            {
                List<Iten> objIten = new List<Iten>();
                objIten = itenDAO.selectObject("iten.id = " + iten.id + ";");

                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                int id = person.id;

                // Checar se já existe um room com esses usuários neste item e retornar o número do room
                Room room1 = new Room();
                ChatDAO chatDAO = new ChatDAO();
                room1.second_user_id = person.id;
                room1.first_user_id = itenDAO.selectItenOwner(iten.id);
                room1.id_room = Int32.Parse(chatDAO.insert(room1, iten.id).ToString());

                // Selecionar todos os rooms
                List<Room> listRooms = new List<Room>();

                //listRooms = <0>;
                listRooms = chatDAO.selectRooms(person.id, iten.id);

                var tuple = Tuple.Create(objIten[0], person, room1, listRooms);
                return View(tuple);
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }

        }

        [HttpGet]
        [Route("Create/{id}")]
        public IActionResult Create()
        {
            try
            {
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                int id = person.id;

                var tuple = Tuple.Create(person);
                return View(tuple);
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }
        }

        [HttpPost]
        [Route("Create/{id}")]
        public async Task<IActionResult> Create(Iten iten)
        {
            try
            {
                string wwwRootPath = _appEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(iten.ImageFile.FileName);
                string extension = Path.GetExtension(iten.ImageFile.FileName);

                // Save image to wwwroot
                iten.image_name = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                iten.image_path = "images/itens/" + iten.image_name;
                string image_name = Path.Combine(wwwRootPath + "/images/itens/", iten.image_name);

                using (var fileStream = new FileStream(image_name, FileMode.Create))
                {
                    await iten.ImageFile.CopyToAsync(fileStream);
                }

                ItenDAO itenDAO = new ItenDAO();
                itenDAO.insert(iten);
                return RedirectToAction("List");
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }
           
        }

        [HttpGet]
        [Route("List/Index")]
        public IActionResult List(int page=1)
        {
            StudentDAO objCDAO = new StudentDAO();
            ItenDAO itenDAO = new ItenDAO();
            NotificationDAO notificationDAO = new NotificationDAO();

            try
            {
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                int id = person.id;
                List<Notification> listNotification = new List<Notification>();
                listNotification = notificationDAO.selectObject(id);
                var tuple = Tuple.Create(objCDAO.selectObject("where id = " + id + ";"), person, itenDAO.selectObject("").ToPagedList(page, 6), listNotification);
                return View(tuple);
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }
        }

        // Listar todos os iten do usuário
        [HttpGet]
        [Route("List/Index/{user}")]
        public IActionResult List(int user, int page=1)
        {
            StudentDAO objCDAO = new StudentDAO();
            ItenDAO itenDAO = new ItenDAO();
            NotificationDAO notificationDAO = new NotificationDAO();

            try
            {
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                int id = person.id;
                List<Notification> listNotification = new List<Notification>();
                listNotification = notificationDAO.selectObject(id);
                var tuple = Tuple.Create(objCDAO.selectObject("where id = " + id + ";"), person, itenDAO.selectObject("iten.person_id = " + user + ";").ToPagedList(page, 6), listNotification);
                return View(tuple);
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }

        }

        [HttpPost]
        [Route("List/Index")]
        public IActionResult List(string date_start, string date_end, int page = 1)
        {
            StudentDAO objCDAO = new StudentDAO();
            ItenDAO itenDAO = new ItenDAO();
            string query = "";
            NotificationDAO notificationDAO = new NotificationDAO();

            try
            {
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                int id = person.id;

                if (date_start != null && date_end != null)
                {
                    query = "date_found >= '" + date_start + "' and date_found <= '" + date_end + "'";
                }
                else
                {
                    query = "";
                }
                List<Notification> listNotification = new List<Notification>();
                listNotification = notificationDAO.selectObject(id);
                var tuple = Tuple.Create(objCDAO.selectObject("where id = " + id + ";"), person, itenDAO.selectObject(query).ToPagedList(page, 6), listNotification);
                return View(tuple);
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }

        }

        [HttpGet]
        [Route("Update/{id}")]
        public IActionResult Update(int id)
        {
            ItenDAO itenDAO = new ItenDAO();

            try
            {
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                var tuple = Tuple.Create(person, itenDAO.selectObject("iten.id = " + id + ";")[0]);
                return View(tuple);
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }
        }

        [HttpPost]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(Iten iten)
        {
            try
            {
                try
                {
                    if (iten.ImageFile != null)
                    {
                        string wwwRootPath = _appEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(iten.ImageFile.FileName);
                        string extension = Path.GetExtension(iten.ImageFile.FileName);

                        // Save image to wwwroot
                        iten.image_name = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        iten.image_path = "images/itens/" + iten.image_name;
                        string image_name = Path.Combine(wwwRootPath + "/images/itens/", iten.image_name);

                        using (var fileStream = new FileStream(image_name, FileMode.Create))
                        {
                            await iten.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                ItenDAO itenDAO = new ItenDAO();
                itenDAO.update(iten);

                //// Get sessão
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                var tuple = Tuple.Create(person);
                return RedirectToAction("List", person.id);
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }

        }
    }
}
