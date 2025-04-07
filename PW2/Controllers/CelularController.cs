using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PW2.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace PW2.Controllers
{
    public class CelularController : Controller
    {
        // GET: Celular
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar()
        {
            Celular.GerarLista(Session);
            return View(Session["ListaCelular"] as List<Celular>);
        }
        public ActionResult Exibir(int id)
        {
            return View((Session["ListaCelular"] as List<Celular>).ElementAt(id));
        }



        public ActionResult Delete(int id)
        {
            return View((Session["ListaCelular"] as List<Celular>).ElementAt(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Celular celular)
        {
            Celular.Procurar(Session, id)?.Excluir(Session); 

            return RedirectToAction("Listar");
        }


        public ActionResult Editar(int id)
        {

            return View(Celular.Procurar(Session, id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Celular celular)
        {
            celular.Editar(Session, id);

            return RedirectToAction("Listar");
        }

        public ActionResult Create()
        {
            return View(new Celular());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Celular celular)
        {
            celular.Adicionar(Session);

            return RedirectToAction("Listar");
        }

        public FileResult BaixarPdf()
        {
            var lista = Session["ListaCelular"] as List<Celular>;
            if (lista == null || lista.Count == 0)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Lista de Celulares", FontFactory.GetFont("Arial", 18, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 1f, 1f });

                table.AddCell(new Phrase("Número", FontFactory.GetFont("Arial", 12, Font.BOLD)));
                table.AddCell(new Phrase("Marca", FontFactory.GetFont("Arial", 12, Font.BOLD)));
                table.AddCell(new Phrase("Novo", FontFactory.GetFont("Arial", 12, Font.BOLD)));

                foreach (var celular in lista)
                {
                    table.AddCell(celular.Numero.ToString());
                    table.AddCell(celular.Marca);
                    table.AddCell(celular.Novo.ToString());
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "ListaCelulares.pdf");
            }
        }
    }
}