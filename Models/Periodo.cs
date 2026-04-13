using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Periodo
{
    public int IdPeriodo { get; set; }

    public int Anio { get; set; }

    public string Periodo1 { get; set; } = null!;

    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}
