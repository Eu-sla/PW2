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
    public class EventosController : Controller
    {
        static EventosController()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
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
            var evento = Evento.Procurar(Session, id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }



        public ActionResult Delete(int id)
        {
            var evento = Evento.Procurar(Session, id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
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

            var evento = Evento.Procurar(Session, id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(evento);
        }

        public ActionResult Create()
        {
            return View(new Evento());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(evento);
        }

        public FileResult BaixarPdf()
        {
            var lista = Session["ListaEvento"] as List<Evento>;
            if (lista == null || lista.Count == 0)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4, 25, 25, 30, 30);
                var writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Título
                var fonteTitulo = FontFactory.GetFont("Arial", 18, Font.BOLD, BaseColor.BLACK);
                var titulo = new Paragraph("Lista de Eventos", fonteTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                doc.Add(titulo);

                // Tabela
                var table = new PdfPTable(2)
                {
                    WidthPercentage = 100
                };
                table.SetWidths(new float[] { 2f, 1f });

                var fonteCabecalho = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.WHITE);
                var fonteCelula = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK);

                // Cabeçalhos com fundo colorido
                var headerBackground = new BaseColor(227, 83, 122); // rosinha da paleta

                PdfPCell cellLocal = new PdfPCell(new Phrase("Local", fonteCabecalho))
                {
                    BackgroundColor = headerBackground,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                };
                PdfPCell cellData = new PdfPCell(new Phrase("Data", fonteCabecalho))
                {
                    BackgroundColor = headerBackground,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                };

                table.AddCell(cellLocal);
                table.AddCell(cellData);

                // Dados
                foreach (var evento in lista)
                {
                    table.AddCell(new PdfPCell(new Phrase(evento.Local, fonteCelula)) { Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase(evento.Data.ToString("dd/MM/yyyy"), fonteCelula)) { Padding = 5 });
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "ListaEventos.pdf");
            }
        }
        public ActionResult ExportarExcel()
        {
            var eventos = Session["ListaEvento"] as List<Evento>;

            if (eventos == null || eventos.Count == 0)
            {
                return Content("Não há eventos para exportar.");
            }

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Eventos");

                planilha.Cells["A1"].Value = "Local";
                planilha.Cells["B1"].Value = "Data";

                using (var range = planilha.Cells["A1:B1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#e3537a"));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < eventos.Count; i++)
                {
                    planilha.Cells[i + 2, 1].Value = eventos[i].Local;
                    planilha.Cells[i + 2, 2].Value = eventos[i].Data.ToString();
                }

                planilha.Column(1).Width = 30; 
                planilha.Column(2).Width = 18; 

                planilha.Cells[planilha.Dimension.Address].AutoFitColumns(1.5); 

                var file = new MemoryStream(pacote.GetAsByteArray());

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Eventos.xlsx");
            }
        }
    }
}