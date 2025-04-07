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
    public class CarroController : Controller
    {
        // GET: Carro
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar()
        {
            Carro.GerarLista(Session);
            return View(Session["ListaCarro"] as List<Carro>);
        }
        public ActionResult Exibir(int id)
        {
            return View((Session["ListaCarro"] as List<Carro>).ElementAt(id));
        }



        public ActionResult Delete(int id)
        {
            return View((Session["ListaCarro"] as List<Carro>).ElementAt(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Carro carro)
        {
            Carro.Procurar(Session, id)?.Excluir(Session); 

            return RedirectToAction("Listar");
        }


        public ActionResult Editar(int id)
        {

            return View(Carro.Procurar(Session, id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Carro carro)
        {
            carro.Editar(Session, id);

            return RedirectToAction("Listar");
        }

        public ActionResult Create()
        {
            return View(new Carro());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Carro carro)
        {
            carro.Adicionar(Session);

            return RedirectToAction("Listar");
        }

        public FileResult BaixarPdf()
        {
            var lista = Session["ListaCarro"] as List<Carro>;
            if (lista == null || lista.Count == 0)
            {
                return null; 
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Lista de Carros", FontFactory.GetFont("Arial", 18, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 1f , 1f});

                table.AddCell(new Phrase("Placa", FontFactory.GetFont("Arial", 12, Font.BOLD)));
                table.AddCell(new Phrase("Ano", FontFactory.GetFont("Arial", 12, Font.BOLD)));
                table.AddCell(new Phrase("Cor", FontFactory.GetFont("Arial", 12, Font.BOLD)));

                foreach (var carro in lista)
                {
                    table.AddCell(carro.Placa);
                    table.AddCell(carro.Ano.ToString());
                    table.AddCell(carro.Cor);
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "ListaCarros.pdf");
            }
        }
    }
}