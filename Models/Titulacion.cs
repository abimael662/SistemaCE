using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Titulacion
{
    public int IdTitulacion { get; set; }

    public int? IdNivel { get; set; }

    public int? IdEstudiante { get; set; }

    public DateOnly? FechaSolicitud { get; set; }

    public DateOnly? FechaAprobacion { get; set; }

    public DateOnly? FechaExpedicion { get; set; }

    public int? Estado { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }

    public virtual Nivel? IdNivelNavigation { get; set; }
}
