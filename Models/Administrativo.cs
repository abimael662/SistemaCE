using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Administrativo
{
    public int IdAdministrativo { get; set; }

    public int? NumeroEmpleado { get; set; }

    public string? Departamento { get; set; }

    public string? Puesto { get; set; }

    public string? Rfc { get; set; }

    public decimal? Sueldo { get; set; }

    public virtual Persona IdAdministrativoNavigation { get; set; } = null!;

    public virtual Empleado? NumeroEmpleadoNavigation { get; set; }
}
