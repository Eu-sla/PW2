using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PW2.Models;

namespace PW2.Controllers
{
    public class AlunoController : Controller
    {
        // GET: Aluno
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar()
        {
            return View(Aluno.GerarLista());
        }
    }
}