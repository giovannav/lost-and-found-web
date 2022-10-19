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
        public IActionResult Read()
        {
            return View();
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
            PersonDAO objCDAO = new PersonDAO();
            ItenDAO itenDAO = new ItenDAO();

            // Get sessão
            var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
            var tuple = Tuple.Create(objCDAO.selectObject(""), person, itenDAO.selectObject(""));
            return View(tuple);
        }

    }
}
