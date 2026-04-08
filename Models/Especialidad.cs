using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Especialidad
{
    public int IdEspecialidad { get; set; }

    public string? Especialidad1 { get; set; }

    public virtual ICollection<Docente> IdDocentes { get; set; } = new List<Docente>();
}
