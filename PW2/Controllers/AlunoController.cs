using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PW2.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

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
            Aluno.GerarLista(Session);
            return View(Session["ListaAluno"] as List<Aluno>);
        }
        public ActionResult Exibir(int id)
        {
            return View((Session["ListaAluno"] as List<Aluno>).ElementAt(id));
        }



        public ActionResult Delete(int id)
        {
            return View((Session["ListaAluno"] as List<Aluno>).ElementAt(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Aluno aluno)
        {
            Aluno.Procurar(Session, id)?.Excluir(Session); //? -> verifica se nao é nulo

            return RedirectToAction("Listar");
        }


        public ActionResult Editar(int id)
        {
            
            return View(Aluno.Procurar(Session, id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Aluno aluno)
        {
            aluno.Editar(Session, id);

            return RedirectToAction("Listar");
        }

        public ActionResult Create()
        {
            return View(new Aluno());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aluno aluno)
        {
            aluno.Adicionar(Session);

            return RedirectToAction("Listar");
        }


        public FileResult BaixarPdf()
        {
            var lista = Session["ListaAluno"] as List<Aluno>;
            if (lista == null || lista.Count == 0)
            {
                return null; 
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Lista de Alunos", FontFactory.GetFont("Arial", 18, Font.BOLD));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph(" ")); 

                PdfPTable table = new PdfPTable(3); 
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 1f, 1f });

                table.AddCell(new Phrase("Nome", FontFactory.GetFont("Arial", 12, Font.BOLD)));
                table.AddCell(new Phrase("RA", FontFactory.GetFont("Arial", 12, Font.BOLD)));
                table.AddCell(new Phrase("Data de Nascimento", FontFactory.GetFont("Arial", 12, Font.BOLD)));

                foreach (var aluno in lista)
                {
                    table.AddCell(aluno.Nome);
                    table.AddCell(aluno.RA);
                    table.AddCell(aluno.DataNasc.ToString());
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "ListaAlunos.pdf");
            }
        }
    }

}