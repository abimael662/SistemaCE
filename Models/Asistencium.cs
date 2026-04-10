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

    public virtual EstadoAsistencium? IdEstadoNavigation { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }

    public virtual SesionClase? IdSesionNavigation { get; set; }
}
