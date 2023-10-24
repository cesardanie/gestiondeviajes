using System;
using System.Collections.Generic;

namespace Mediabrandsgastosdeviajes.Models;

public partial class Viajero
{
    public long Id { get; set; }

    public string Agencia { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public decimal Alimentacion { get; set; }

    public decimal Transportes { get; set; }

    public decimal Hotel { get; set; }

    public decimal Otros { get; set; }

    public string NombreCliente { get; set; } = null!;

    public decimal TotalGastos { get; set; }

    public string Estado { get; set; } = null!;

    public string Unidad { get; set; } = null!;

    public string Proveedor { get; set; } = null!;
}
