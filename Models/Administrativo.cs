using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCE.Models;

public partial class Administrativo
{
    public int IdAdministrativo { get; set; }

    public int? NumeroEmpleado { get; set; }

    public string? Departamento { get; set; }

    public string? Puesto { get; set; }

    public string? Rfc { get; set; }

    public decimal? Sueldo { get; set; }

    [ValidateNever]
    public virtual Persona IdAdministrativoNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Empleado? NumeroEmpleadoNavigation { get; set; }
}
