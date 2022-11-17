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

            List<Iten> objIten = new List<Iten>();
            objIten = itenDAO.selectObject("where id = " + iten.id + ";");

            var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
            int id = person.id;

            var tuple = Tuple.Create(objIten[0], person);
            return View(tuple);
        }

        [HttpGet]
        [Route("Create/{id}")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create/{id}")]
        public async Task<IActionResult> Create(Iten iten)
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

        [HttpGet]
        public IActionResult List(Iten iten)
        {
            StudentDAO objCDAO = new StudentDAO();
            ItenDAO itenDAO = new ItenDAO();

            // Get sessão
            var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
            int id = person.id;
            //TempData["StudentSession"] = HttpContext.Session.Id;

            var tuple = Tuple.Create(objCDAO.selectObject("where id = " + id + ";"), person, itenDAO.selectObject(""));
            return View(tuple);
        }

        [HttpGet]
        [Route("Update/{id}")]
        public IActionResult Update(int id)
        {
            //StudentDAO objCDAO = new StudentDAO();
            ItenDAO itenDAO = new ItenDAO();

            // Get sessão
            var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
            // int id = person.id;
            //TempData["StudentSession"] = HttpContext.Session.Id;

            var tuple = Tuple.Create(person, itenDAO.selectObject("where id = " + id + ";")[0]);
            return View(tuple);
        }

        [HttpPost]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(Iten iten)
        {
            try {
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
            catch(Exception)
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


        //[HttpGet]
        //public IActionResult Chat(int id)
        //{
        //    ItenDAO itenDAO = new ItenDAO();

        //    var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
        //    int owner_id = itenDAO.selectItenOwner(id);

        //    // Checar se já existe um room com esses usuários neste item e retornar o número do room
        //    ChatDAO chatDAO = new ChatDAO();
        //    int room = Int32.Parse(chatDAO.insert(owner_id, person.id, id).ToString());

        //    //var tuple = Tuple.Create(person);
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Chat(Message messageObj)
        //{
        //    ItenDAO itenDAO = new ItenDAO();

        //    var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
        //   // int owner_id = itenDAO.selectItenOwner(id);
        //    return View();
        //}
    }
}
