using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Docente
{
    public int IdDocente { get; set; }

    public int? NumeroEmpleado { get; set; }

    public string? CedulaProfesional { get; set; }

    public string? Rfc { get; set; }

    public decimal? Sueldo { get; set; }

    public virtual ICollection<CalificacionAlumno> CalificacionAlumnos { get; set; } = new List<CalificacionAlumno>();

    public virtual ICollection<DocenteMateriaGrupo> DocenteMateriaGrupos { get; set; } = new List<DocenteMateriaGrupo>();

    public virtual Persona IdDocenteNavigation { get; set; } = null!;

    public virtual Empleado? NumeroEmpleadoNavigation { get; set; }

    public virtual ICollection<SesionClase> SesionClases { get; set; } = new List<SesionClase>();

    public virtual ICollection<Especialidad> IdEspecialidads { get; set; } = new List<Especialidad>();
}
