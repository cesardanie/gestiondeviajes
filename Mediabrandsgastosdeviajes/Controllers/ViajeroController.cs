using Microsoft.AspNetCore.Mvc;
using Mediabrandsgastosdeviajes.Models;
using System.Drawing.Text;
using Microsoft.EntityFrameworkCore;
using DinkToPdf;
using DinkToPdf.Contracts;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Linq;
using NPOI.XWPF.UserModel;
using System;
using NPOI.XWPF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.IO;
using System.Linq;
using NPOI.OpenXmlFormats.Wordprocessing;
using Xceed.Document.NET;
using Xceed.Words.NET;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Xceed.Document.NET;
using Xceed.Document.NET;
using DinkToPdf;
using DinkToPdf.Contracts;

using NPOI.XWPF.UserModel;

namespace Mediabrandsgastosdeviajes.Controllers
{
    public class ViajeroController : Controller
    {
        private readonly ViajesgastosContext _context;
        private readonly IConverter _pdfConverter;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ViajeroController(IConverter pdfConverter, ICompositeViewEngine viewEngine, ViajesgastosContext context, IWebHostEnvironment webHostEnvironment)
        {
            _pdfConverter = pdfConverter;
            _viewEngine = viewEngine;
            _context = context; // Inyecta el contexto de base de datos
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Viajero()
        {
            return View("Viajero"); // Muestra la vista "Viajero.cshtml"
        }

        public IActionResult Aprobador()
        {
            var viajeros = _context.Viajeros.ToList();
            var presupuestos = _context.Presupuestos.ToList();

            ViewBag.Presupuestos = presupuestos;

            return View(viajeros);
        }



        [HttpPost] // Esta acción manejará la solicitud POST del formulario
        public IActionResult ProcesarGastos(ViajeroForm model)
            {
            
            ViajeroDTO Viajero = new ViajeroDTO();
            model.TotalGastos = model.Alimentacion + model.Transportes+model.Hotel+model.Otros;
            Viajero.Transportes = model.Transportes;
            Viajero.Descripcion = model.Descripcion;
            Viajero.estado = "Pendiente";
            Viajero.Fecha = model.Fecha;
            Viajero.Responsable= model.Responsable;
            Viajero.Alimentacion=model.Alimentacion;
            Viajero.Hotel = model.Hotel;
            Viajero.Otros = model.Otros;
            Viajero.Unidad=model.Unidad;
            Viajero.TotalGastos=model.TotalGastos;
            Viajero.Responsable = model.Responsable;
            Viajero.Proveedor=model.Proveedor;
            Viajero.NombreCliente=model.NombreCliente;
            var Viajerodos= new Viajero{ 
                NombreCliente=Viajero.NombreCliente,
                Alimentacion=Viajero.Alimentacion,
                Descripcion=Viajero.Descripcion,
                Hotel=Viajero.Hotel,
                Estado=Viajero.estado,
                Otros=Viajero.Otros,
                Fecha=Viajero.Fecha,
                Proveedor=Viajero.Proveedor,
                TotalGastos=Viajero.TotalGastos,
                Transportes=Viajero.Transportes,
                Unidad=Viajero.Unidad,
                Agencia=Viajero.Responsable

            
            };
            _context.Viajeros.Add(Viajerodos);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Aceptar(int id)
        {
            var respuesta = "aprobado";
            var viajero = _context.Viajeros.FirstOrDefault(v => v.Id == id);
            var Presupuestosuno = _context.Presupuestos.FirstOrDefault(v => v.Id == 1);

            if (viajero == null)
            {
                // Manejar el caso en el que no se encontró ningún Viajero con el ID dado
                return NotFound(); // Puedes devolver un error 404, por ejemplo.
            }
            // Ahora, puedes acceder a los campos "TotalGastos" y "Estado" del viajero
       
            var saldo = Presupuestosuno.Saldo - viajero.TotalGastos;
            var Presupuestototal =Presupuestosuno.PresupuestoGeneral- saldo;
            viajero.Estado = respuesta;
            Presupuestosuno.PresupuestoGeneral=Presupuestototal;
            Presupuestosuno.Saldo = Presupuestototal;
            try
            {
                _context.Presupuestos.Update(Presupuestosuno);
                _context.SaveChanges();
                _context.Viajeros.Update(viajero);
                _context.SaveChanges();
                // Puedes manejar el caso de éxito aquí
                return RedirectToAction("Aprobador"); // Redirige a la vista "Aprobador" u otra página
            }
            catch (DbUpdateException ex)
            {
                // Manejar errores si no se pueden guardar los cambios
                // Puedes devolver una vista con un mensaje de error o realizar alguna otra acción apropiada
                return View("Error", ex);
            }
     
        }

        [HttpPost]
        public IActionResult Rechazar(int id)
        {
            
            var viajero = _context.Viajeros.FirstOrDefault(v => v.Id == id);
            // Lógica para rechazar aquí
            var respuesta = "Rechazado";
            viajero.Estado= respuesta;
            try
            {
                _context.Viajeros.Update(viajero);
                _context.SaveChanges();
                // Puedes manejar el caso de éxito aquí
                return RedirectToAction("Aprobador"); // Redirige a la vista "Aprobador" u otra página
            }
            catch (DbUpdateException ex)
            {
                // Manejar errores si no se pueden guardar los cambios
                // Puedes devolver una vista con un mensaje de error o realizar alguna otra acción apropiada
                return View("Error", ex);
            }

            // Puedes realizar las operaciones necesarias y, si es necesario, devolver una respuesta
            return Json(respuesta);
        }

        public IActionResult ExportToExcel()
        {
            // Obtener los datos que deseas exportar (por ejemplo, desde tu base de datos)
            var data = _context.Viajeros.ToList(); // Reemplaza 'TuTabla' con el nombre de tu tabla de datos
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Crear un nuevo archivo Excel
            using (var package = new ExcelPackage())
            {
                // Agregar una hoja de trabajo
                var worksheet = package.Workbook.Worksheets.Add("Datos");

                // Agregar encabezados
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Hotel";
                worksheet.Cells[1, 3].Value = "Alimentacion";
                worksheet.Cells[1, 4].Value = "Nombre de Cliente";
                worksheet.Cells[1, 5].Value = "Descripcion";
                worksheet.Cells[1, 6].Value = "Otros";
                worksheet.Cells[1, 7].Value = "Estado";
                worksheet.Cells[1, 8].Value = "Fecha";
                worksheet.Cells[1, 9].Value = "Agencia";
                worksheet.Cells[1, 10].Value = "Proveedor";
                worksheet.Cells[1, 11].Value = "Total Gastos";
                worksheet.Cells[1, 12].Value = "Transpoertes";
                worksheet.Cells[1, 13].Value = "Unidad";
                // ... Agregar más encabezados según tus datos

                // Llenar los datos
                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].Id; // Reemplaza 'Campo1' con el nombre del campo
                    worksheet.Cells[i + 2, 2].Value = data[i].Hotel; // Reemplaza 'Campo2' con el nombre del campo}
                    worksheet.Cells[i + 2, 3].Value = data[i].Alimentacion;
                    worksheet.Cells[i + 2, 4].Value = data[i].NombreCliente;
                    worksheet.Cells[i + 2, 5].Value = data[i].Descripcion;
                    worksheet.Cells[i + 2, 6].Value = data[i].Otros;
                    worksheet.Cells[i + 2, 7].Value = data[i].Estado;
                    worksheet.Cells[i + 2, 8].Value = data[i].Fecha;
                    worksheet.Cells[i + 2, 9].Value = data[i].Agencia;
                    worksheet.Cells[i + 2, 10].Value = data[i].Proveedor;
                    worksheet.Cells[i + 2, 11].Value = data[i].TotalGastos;
                    worksheet.Cells[i + 2, 12].Value = data[i].Transportes;
                    worksheet.Cells[i + 2, 13].Value = data[i].Unidad;

                    // ... Agregar más campos según tus datos
                }

                // Formatear el archivo Excel (opcional)
                worksheet.Cells[1, 1, data.Count + 1, data[0].GetType().GetProperties().Length].Style.Font.Bold = true;
                worksheet.Cells[1, 1, data.Count + 1, data[0].GetType().GetProperties().Length].AutoFitColumns();

                // Guardar el archivo Excel en un flujo de memoria
                var stream = new MemoryStream();
                package.SaveAs(stream);

                // Configurar la respuesta HTTP para descargar el archivo Excel
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Datos.xlsx");
            }
        }
        public IActionResult GenerateWordDocument()
        {
            List<Viajero> viajeros = _context.Viajeros.ToList();

            if (viajeros == null || viajeros.Count == 0)
            {
                return NotFound(); // Otra acción apropiada en caso de no encontrar registros
            }

            using (var doc = DocX.Create("viajeros.docx"))
            {
                doc.InsertParagraph("Lista de Viajeros")
                    .Bold()
                    .FontSize(16)
                    .Alignment = Alignment.center;

                foreach (var viajero in viajeros)
                {
                    doc.InsertParagraph($"ID: {viajero.Id}");
                    doc.InsertParagraph($"Agencia: {viajero.Agencia}");
                    doc.InsertParagraph($"Descripción: {viajero.Descripcion}");
                    doc.InsertParagraph($"Fecha: {viajero.Fecha}");
                    doc.InsertParagraph($"Alimentación: {viajero.Alimentacion}");
                    doc.InsertParagraph($"Transportes: {viajero.Transportes}");
                    doc.InsertParagraph($"Hotel: {viajero.Hotel}");
                    doc.InsertParagraph($"Otros: {viajero.Otros}");
                    doc.InsertParagraph($"Nombre del Cliente: {viajero.NombreCliente}");
                    doc.InsertParagraph($"Total de Gastos: {viajero.TotalGastos}");
                    doc.InsertParagraph($"Estado: {viajero.Estado}");
                    doc.InsertParagraph($"Unidad: {viajero.Unidad}");
                    doc.InsertParagraph($"Proveedor: {viajero.Proveedor}");
                    doc.InsertParagraph("");
                }

                doc.Save();
            }

            var memoryStream = new MemoryStream();
            using (var fileStream = new FileStream("viajeros.docx", FileMode.Open))
            {
                fileStream.CopyTo(memoryStream);
            }

            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "viajeros.docx");
        }
        public ActionResult GenerarPDF()
        {
            // Ruta completa del archivo en el servidor
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "pdfs", "Informe.pdf");




            using (var doc = DocX.Create(filePath))
            {
                doc.InsertParagraph("Este es un informe PDF generado con Xceed.Document.NET");
                doc.Save();
            }

            // Configurar la respuesta HTTP para descargar el archivo PDF
            var fileName = "Informe.pdf";
            var mimeType = "application/pdf";

            // Retorna el archivo con la ruta completa para que se descargue en la carpeta de descargas del navegador
            return File(filePath, mimeType, fileName);
        }

    }

}
