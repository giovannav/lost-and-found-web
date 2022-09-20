using achei_web.Models.POJO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace achei_web.Controllers
{
    [Route("aluno")]
    public class AlunoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Cadastro")]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [Route("Cadastro")]
        public IActionResult Cadastro(Aluno aluno)
        {
            return View();
        }

    }
}
