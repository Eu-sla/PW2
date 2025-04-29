using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PW2.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using OfficeOpenXml;

namespace PW2.Controllers
{
    public class CelularController : Controller
    {
        static CelularController()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
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
            var celular = Celular.Procurar(Session, id);
            if (celular == null)
            {
                return HttpNotFound();
            }
            return View(celular);
        }



        public ActionResult Delete(int id)
        {
            var celular = Celular.Procurar(Session, id);
            if (celular == null)
            {
                return HttpNotFound();
            }
            return View(celular);
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

            var celular = Celular.Procurar(Session, id);
            if (celular == null)
            {
                return HttpNotFound();
            }
            return View(celular);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Celular celular)
        {
            if (ModelState.IsValid)
            {
                celular.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(celular);
        }

        public ActionResult Create()
        {
            return View(new Celular());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Celular celular)
        {
            if (ModelState.IsValid)
            {
                celular.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(celular);
        }

        public FileResult BaixarPdf()
        {
            var lista = Session["ListaCelular"] as List<Celular>;

            if (lista == null || lista.Count == 0)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var corTitulo = new BaseColor(227, 83, 122);
                var corCabecalho = new BaseColor(240, 240, 240); 
                var corCelular = new BaseColor(255, 255, 255); 

                var fonteTitulo = FontFactory.GetFont("Arial", 18, Font.BOLD, corTitulo);
                var fonteCabecalho = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK);
                var fonteNormal = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);

                var titulo = new Paragraph("Lista de Celulares", fonteTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph(" "));

                var table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 1f, 1f });

                var cellHeader1 = new PdfPCell(new Phrase("Número", fonteCabecalho));
                var cellHeader2 = new PdfPCell(new Phrase("Marca", fonteCabecalho));
                var cellHeader3 = new PdfPCell(new Phrase("Novo", fonteCabecalho));

                var corHeader = new BaseColor(173, 216, 230); 
                foreach (var c in new[] { cellHeader1, cellHeader2, cellHeader3 })
                {
                    c.BackgroundColor = corHeader;
                    c.HorizontalAlignment = Element.ALIGN_CENTER;
                    c.Padding = 5;
                }

                table.AddCell(cellHeader1);
                table.AddCell(cellHeader2);
                table.AddCell(cellHeader3);

                foreach (var celular in lista)
                {
                    var cellNumero = new PdfPCell(new Phrase(celular.Numero.ToString(), fonteNormal));
                    cellNumero.BackgroundColor = corCelular;
                    cellNumero.Padding = 5;

                    var cellMarca = new PdfPCell(new Phrase(celular.Marca, fonteNormal));
                    cellMarca.Padding = 5;

                    var cellNovo = new PdfPCell(new Phrase(celular.Novo ? "Sim" : "Não", fonteNormal));
                    cellNovo.Padding = 5;

                    table.AddCell(cellNumero);
                    table.AddCell(cellMarca);
                    table.AddCell(cellNovo);
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "ListaCelulares.pdf");
            }
        }


        public ActionResult ExportarExcel()
        {
            var celulares = Session["ListaCelular"] as List<Celular>;

            if (celulares == null || celulares.Count == 0)
            {
                return Content("Não há celulares para exportar.");
            }

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Celulares");

                planilha.Cells["A1"].Value = "Número";
                planilha.Cells["B1"].Value = "Marca";
                planilha.Cells["C1"].Value = "Novo";

                using (var range = planilha.Cells["A1:C1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#add8e6"));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < celulares.Count; i++)
                {
                    planilha.Cells[i + 2, 1].Value = celulares[i].Numero.ToString();
                    planilha.Cells[i + 2, 1].Style.Numberformat.Format = "@";

                    planilha.Cells[i + 2, 2].Value = celulares[i].Marca;
                    planilha.Cells[i + 2, 3].Value = celulares[i].Novo ? "Sim" : "Não";
                }

                planilha.Column(1).Width = 20;
                planilha.Column(2).Width = 20;
                planilha.Column(3).Width = 10;
                planilha.Cells[planilha.Dimension.Address].AutoFitColumns(0.9);

                var file = new MemoryStream(pacote.GetAsByteArray());

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Celulares.xlsx");
            }
        }


    }
}