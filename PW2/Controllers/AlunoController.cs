using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PW2.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using OfficeOpenXml;

namespace PW2.Controllers
{
    public class AlunoController : Controller
    {
        // GET: Aluno
        static AlunoController()
        {
            // Defina a licença correta. Aqui é para uso comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
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
            if (ModelState.IsValid)
            {
                aluno.Editar(Session, id);

                return RedirectToAction("Listar");
            }
            return View(aluno);
        }

        public ActionResult Create()
        {

            return View(new Aluno());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                aluno.Adicionar(Session);

                return RedirectToAction("Listar");
            }
            return View(aluno);
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
                    table.AddCell(aluno.DataNasc.ToString("dd/MM/yyyy"));
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "ListaAlunos.pdf");
            }
        }

        public ActionResult ExportarExcel()
        {

            var alunos = (List<Aluno>)Session["ListaAluno"];

            if (alunos == null || alunos.Count == 0)
            {
                return Content("Não há alunos para exportar.");
            }

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Alunos");

                planilha.Cells[1, 1].Value = "Nome";
                planilha.Cells[1, 2].Value = "RA";
                planilha.Cells[1, 3].Value = "Data de Nascimento";

                for (int i = 0; i < alunos.Count; i++)
                {
                    planilha.Cells[i + 2, 1].Value = alunos[i].Nome;
                    planilha.Cells[i + 2, 2].Value = alunos[i].RA;
                    planilha.Cells[i + 2, 3].Value = alunos[i].DataNasc.ToString("dd/MM/yyyy");
                }

                var file = new MemoryStream(pacote.GetAsByteArray());

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Alunos.xlsx");
            }
        }
    }

}