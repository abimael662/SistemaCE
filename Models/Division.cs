using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Division
{
    public int IdDivision { get; set; }

    public string? Nombre { get; set; }

    public string? Nomenclatura { get; set; }

    public virtual ICollection<Carrera> Carreras { get; set; } = new List<Carrera>();
}
