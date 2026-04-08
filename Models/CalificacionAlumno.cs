using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class CalificacionAlumno
{
    public int IdCalificacion { get; set; }

    public int? IdDocente { get; set; }

    public int? IdEstudiante { get; set; }

    public int? IdMateria { get; set; }

    public decimal? Parcial1 { get; set; }

    public decimal? Parcial2 { get; set; }

    public decimal? Parcial3 { get; set; }

    public decimal? PromedioFinal { get; set; }

    public virtual Docente? IdDocenteNavigation { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }

    public virtual Materia? IdMateriaNavigation { get; set; }
}
