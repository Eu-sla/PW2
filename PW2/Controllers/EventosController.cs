using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
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
            Evento.GerarLista(Session);
            return View(Session["ListaEvento"] as List<Evento>);
        }
        public ActionResult Exibir(int id)
        {
            return View((Session["ListaEvento"] as List<Evento>).ElementAt(id));
        }



        public ActionResult Delete(int id)
        {
            return View((Session["ListaEvento"] as List<Evento>).ElementAt(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Evento evento)
        {
            Evento.Procurar(Session, id)?.Excluir(Session); 

            return RedirectToAction("Listar");
        }


        public ActionResult Editar(int id)
        {

            return View(Evento.Procurar(Session, id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Evento evento)
        {
            evento.Editar(Session, id);

            return RedirectToAction("Listar");
        }

        public ActionResult Create()
        {
            return View(new Evento());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento evento)
        {
            evento.Adicionar(Session);

            return RedirectToAction("Listar");
        }

        public FileResult BaixarPdf()
        {
            var lista = Session["ListaEvento"] as List<Evento>;
            if (lista == null || lista.Count == 0)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Lista de Eventos", FontFactory.GetFont("Arial", 18, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 1f });

                table.AddCell(new Phrase("Local", FontFactory.GetFont("Arial", 12, Font.BOLD)));
                table.AddCell(new Phrase("Data", FontFactory.GetFont("Arial", 12, Font.BOLD)));
                

                foreach (var evento in lista)
                {
                    table.AddCell(evento.Local);
                    table.AddCell(evento.Data.ToString());
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "ListaCelulares.pdf");
            }
        }
    }
}