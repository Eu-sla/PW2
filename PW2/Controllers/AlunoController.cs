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
            var aluno = Aluno.Procurar(Session, id);
            if (aluno == null)
            {
                return HttpNotFound();
            }
            return View(aluno);
        }



        public ActionResult Delete(int id)
        {
            var aluno = Aluno.Procurar(Session, id);
            if (aluno == null)
            {
                return HttpNotFound();
            }
            return View(aluno);
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

                var corTitulo = new BaseColor(214, 92, 116);
                var corCabecalho = new BaseColor(245, 138, 110);
                var corComplementar = new BaseColor(0, 0, 0); 

                var fonteTitulo = FontFactory.GetFont("Arial", 18, Font.BOLD, corTitulo);
                var fonteCabecalho = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK);
                var fonteNormal = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);

                var titulo = new Paragraph("Lista de Alunos", fonteTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 1f, 1.5f });

                var cabecalho1 = new PdfPCell(new Phrase("Nome", fonteCabecalho));
                var cabecalho2 = new PdfPCell(new Phrase("RA", fonteCabecalho));
                var cabecalho3 = new PdfPCell(new Phrase("Data de Nascimento", fonteCabecalho));

                foreach (var cell in new[] { cabecalho1, cabecalho2, cabecalho3 })
                {
                    cell.BackgroundColor = corCabecalho;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                }

                table.AddCell(cabecalho1);
                table.AddCell(cabecalho2);
                table.AddCell(cabecalho3);

                foreach (var aluno in lista)
                {
                    var cellNome = new PdfPCell(new Phrase(aluno.Nome, fonteNormal));
                    cellNome.Padding = 5;

                    var cellRA = new PdfPCell(new Phrase(aluno.RA, fonteNormal));
                    cellRA.Padding = 5;

                    var cellDataNasc = new PdfPCell(new Phrase(aluno.DataNasc.ToString("dd/MM/yyyy"), fonteNormal));
                    cellDataNasc.Padding = 5;

                    table.AddCell(cellNome);
                    table.AddCell(cellRA);
                    table.AddCell(cellDataNasc);
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

                using (var range = planilha.Cells["A1:C1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#f58a6e")); // Amarelo claro
                    range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < alunos.Count; i++)
                {
                    planilha.Cells[i + 2, 1].Value = alunos[i].Nome;
                    planilha.Cells[i + 2, 2].Value = alunos[i].RA;
                    planilha.Cells[i + 2, 3].Value = alunos[i].DataNasc.ToString("dd/MM/yyyy");
                }

                planilha.Cells[planilha.Dimension.Address].AutoFitColumns(0.9);

                var file = new MemoryStream(pacote.GetAsByteArray());

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Alunos.xlsx");
            }
        }


    }

}