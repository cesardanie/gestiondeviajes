namespace Mediabrandsgastosdeviajes.Models
{
    public class ViajeroForm
    {

            public string Responsable { get; set; } // Agencia o Cliente
            public string Unidad { get; set; } // BPN, INI, UM, OTRA
            public DateTime Fecha { get; set; }
            public string Descripcion { get; set; }
            public string Proveedor { get; set; }
            public decimal Alimentacion { get; set; }
            public decimal Transportes { get; set; }
            public decimal Hotel { get; set; }
            public decimal Otros { get; set; }
            public string NombreCliente { get; set; }
            public decimal TotalGastos { get; set; }
        
    }
}
