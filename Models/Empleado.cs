using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string? TipoEmpleado { get; set; }

    public virtual Administrativo? Administrativo { get; set; }

    public virtual Docente? Docente { get; set; }
}
