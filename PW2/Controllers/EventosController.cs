using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PW2.Models;

namespace PW2.Controllers
{
    public class EventosController : Controller
    {
        // GET: Eventos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar()
        {
            return View(Evento.GerarLista());
        }
        public ActionResult Exibir(int id)
        {
            return View(Evento.GerarLista().ElementAt(id));
        }
    }
}