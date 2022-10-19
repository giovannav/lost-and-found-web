using achei_web.Models.DAO;
using achei_web.Models.POJO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Controllers
{
    [Route("student")]
    public class StudentController : Controller
    {

        private IWebHostEnvironment _appEnvironment;

        public StudentController(IWebHostEnvironment env)
        {
            _appEnvironment = env;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("Registration")]
        public IActionResult Registration()
        {
            CourseDAO objCourse = new CourseDAO();
            var tuple = Tuple.Create(objCourse.selectObject(""));
            return View(tuple);
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(Student student)
        
        {
            //Course course = new Course();
            //student.course.id = 1;
            StudentDAO studentDAO = new StudentDAO();
            studentDAO.insert(student);
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            PersonDAO objCDAO = new PersonDAO();

            // Get sessão
            var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
            var tuple = Tuple.Create(objCDAO.selectObject(""), person);
            return View(tuple);
            //return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Person person)
        {
            PersonDAO personDAO = new PersonDAO();
            Person authenticate = personDAO.authenticate(person);
            if (authenticate != null)
            {
                HttpContext.Session.SetString("StudentSession", JsonConvert.SerializeObject(authenticate));
                return this.RedirectToAction("List", "Iten");
                //return RedirectToAction("Index");
            }
            return View();
        }
    }
}
