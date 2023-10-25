using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mediabrandsgastosdeviajes.Models;

public partial class Presupuesto
{
    [Key]
    public long Id { get; set; }

    public decimal PresupuestoGeneral { get; set; }

    public decimal Saldo { get; set; }
}
