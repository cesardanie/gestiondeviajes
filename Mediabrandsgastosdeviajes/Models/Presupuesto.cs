using System;
using System.Collections.Generic;

namespace Mediabrandsgastosdeviajes.Models;

public partial class Presupuesto
{
    public long Id { get; set; }

    public decimal PresupuestoGeneral { get; set; }

    public decimal Saldo { get; set; }
}
