using Microsoft.AspNetCore.Mvc;
using Mediabrandsgastosdeviajes.Models;

namespace Mediabrandsgastosdeviajes.Controllers
{
    public class ViajeroController : Controller
    {
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

            return RedirectToAction("Index", "Home");
        }
    }
}
