using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using PW2.Models;
using OfficeOpenXml;

namespace PW2.Controllers
{
    public class CarroController : Controller
    {
        static CarroController()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
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
            var carro = Carro.Procurar(Session, id);
            if (carro == null)
                return HttpNotFound();

            return View(carro);
        }



        public ActionResult Delete(int id)
        {
            var carro = Carro.Procurar(Session, id);
            if (carro == null)
                return HttpNotFound();

            return View(carro);
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

            var carro = Carro.Procurar(Session, id);
            if (carro == null)
                return HttpNotFound();

            return View(carro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Carro carro)
        {
            if (ModelState.IsValid)
            {
                carro.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(carro);
        }

        public ActionResult Create()
        {
            return View(new Carro());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Carro carro)
        {
            if (ModelState.IsValid)
            {
                carro.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(carro);
        }
        public FileResult BaixarPdf()
        {
            var lista = Session["ListaCarro"] as List<Carro>;
            if (lista == null || lista.Count == 0)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var corTitulo = new BaseColor(255, 140, 0); 
                var corCabecalho = new BaseColor(255, 165, 0);
                var corCelulaPlaca = new BaseColor(255, 255, 255); 

                var fonteTitulo = FontFactory.GetFont("Arial", 18, Font.BOLD, corTitulo);
                var fonteCabecalho = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.WHITE);
                var fonteNormal = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);

                var titulo = new Paragraph("Lista de Carros", fonteTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph(" "));

                var table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 1f, 1f });

                var cellHeader1 = new PdfPCell(new Phrase("Placa", fonteCabecalho));
                var cellHeader2 = new PdfPCell(new Phrase("Ano", fonteCabecalho));
                var cellHeader3 = new PdfPCell(new Phrase("Cor", fonteCabecalho));

                foreach (var c in new[] { cellHeader1, cellHeader2, cellHeader3 })
                {
                    c.BackgroundColor = corCabecalho;
                    c.HorizontalAlignment = Element.ALIGN_CENTER;
                    c.Padding = 5;
                }

                table.AddCell(cellHeader1);
                table.AddCell(cellHeader2);
                table.AddCell(cellHeader3);

                foreach (var carro in lista)
                {
                    var cellPlaca = new PdfPCell(new Phrase(carro.Placa, fonteNormal));
                    cellPlaca.BackgroundColor = corCelulaPlaca;
                    cellPlaca.Padding = 5;

                    var cellAno = new PdfPCell(new Phrase(carro.Ano.ToString(), fonteNormal));
                    cellAno.Padding = 5;

                    var cellCor = new PdfPCell(new Phrase(carro.Cor, fonteNormal));
                    cellCor.Padding = 5;

                    table.AddCell(cellPlaca);
                    table.AddCell(cellAno);
                    table.AddCell(cellCor);
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "ListaCarros.pdf");
            }
        }
        public ActionResult ExportarExcel()
        {
            var carros = Session["ListaCarro"] as List<Carro>;

            if (carros == null || carros.Count == 0)
            {
                return Content("Não há carros para exportar.");
            }

            using (var pacote = new OfficeOpenXml.ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Carros");

                planilha.Cells["A1"].Value = "Placa";
                planilha.Cells["B1"].Value = "Ano";
                planilha.Cells["C1"].Value = "Cor";

                using (var range = planilha.Cells["A1:C1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFA500")); // laranja
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < carros.Count; i++)
                {
                    planilha.Cells[i + 2, 1].Value = carros[i].Placa;
                    planilha.Cells[i + 2, 1].Style.Numberformat.Format = "@";

                    planilha.Cells[i + 2, 2].Value = carros[i].Ano;
                    planilha.Cells[i + 2, 3].Value = carros[i].Cor;
                }

                planilha.Column(1).Width = 20;
                planilha.Column(2).Width = 15;
                planilha.Column(3).Width = 20;
                planilha.Cells[planilha.Dimension.Address].AutoFitColumns(0.9);

                var file = new MemoryStream(pacote.GetAsByteArray());

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Carros.xlsx");
            }
        }

    }
}