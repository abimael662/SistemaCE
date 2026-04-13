using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string? TipoEmpleado { get; set; }

    [ValidateNever]
    public virtual Administrativo? Administrativo { get; set; }

    [ValidateNever]
    public virtual Docente? Docente { get; set; }
}
