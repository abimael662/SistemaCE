using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Asistencium
{
    public int IdAsistencia { get; set; }

    public int? IdSesion { get; set; }

    public int? IdEstudiante { get; set; }

    public string? Observacion { get; set; }

    public int? IdEstado { get; set; }

    [ValidateNever]
    public virtual EstadoAsistencium? IdEstadoNavigation { get; set; }

    [ValidateNever]
    public virtual Estudiante? IdEstudianteNavigation { get; set; }

    [ValidateNever]
    public virtual SesionClase? IdSesionNavigation { get; set; }
}
