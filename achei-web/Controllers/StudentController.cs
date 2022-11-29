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

        [HttpGet]
        [Route("Registration")]
        public IActionResult Registration(int message)
        {
            CourseDAO objCourse = new CourseDAO();
            if (message == 1)
            {
                ViewBag.Email = "Email ou prontuário já cadastrados!";
            }
            else
            {
                ViewBag.Email = "";
            }
            var tuple = Tuple.Create(objCourse.selectObject(""));
            return View(tuple);
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(Student student)
        
        {
            CourseDAO objCourse = new CourseDAO();
            StudentDAO studentDAO = new StudentDAO();

            bool email_exists = studentDAO.selectStudentEmail(student);

            var tuple = Tuple.Create(objCourse.selectObject(""));

            if (email_exists == true)
            {
                //string message = "Email já cadastrado!";
                return this.RedirectToAction("Registration", "Student", new { message = 1 });
            }

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
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("StudentSession");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Index()
        {
            StudentDAO objCDAO = new StudentDAO();
            try
            {
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                TempData["StudentSession"] = HttpContext.Session.Id;
                var tuple = Tuple.Create(objCDAO.selectObject(""), person);
                return View(tuple);
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Student student)
        {
            StudentDAO studentDAO = new StudentDAO();
            Person authenticate = studentDAO.authenticate(student);

            try
            {
                if (authenticate != null)
                {
                    HttpContext.Session.SetString("StudentSession", JsonConvert.SerializeObject(authenticate));
                    return this.RedirectToAction("List", "Iten");
                }
                return View();
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }
        }

        [HttpGet]
        [Route("Profile/{id}")]
        public IActionResult Profile(int id)
        {
            StudentDAO studentDAO = new StudentDAO();

            try
            {
                Student student = new Student();
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                student = studentDAO.selectStudent(id);
                var tuple = Tuple.Create(student, person);
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
            StudentDAO studentDAO = new StudentDAO();
            CourseDAO objCourse = new CourseDAO();
            try
            {
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                var tuple = Tuple.Create(person, studentDAO.selectStudent(id), objCourse.selectObject(""));
                return View(tuple);
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }
        }

        [HttpPost]
        [Route("Update/{id}")]
        public IActionResult Update(Student student)
        {
            StudentDAO studentDAO = new StudentDAO();
            CourseDAO objCourse = new CourseDAO();

            try
            {
                var person = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("StudentSession"));
                student.id = person.id;
                studentDAO.update(student);
                return RedirectToAction("Profile", new { id = person.id });
            }
            catch
            {
                return this.RedirectToAction("Login", "Student");
            }
        }
    }
}
