using Microsoft.AspNetCore.Mvc;
using Mediabrandsgastosdeviajes.Models;
using System.Drawing.Text;
using Microsoft.EntityFrameworkCore;

namespace Mediabrandsgastosdeviajes.Controllers
{
    public class ViajeroController : Controller
    {
        private readonly ViajesgastosContext _context;

        public ViajeroController(ViajesgastosContext context)
        {
            _context = context;
        }


        public IActionResult Viajero()
        {
            return View("Viajero"); // Muestra la vista "Viajero.cshtml"
        }

        public IActionResult Aprobador()
        {
            return View("Aprobador"); // Muestra la vista "Aprobador.cshtml" (si existe)
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
    }
}
