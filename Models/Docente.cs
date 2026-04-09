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

    public virtual Empleado? NumeroEmpleadoNavigation { get; set; }

    public virtual ICollection<Especialidad> IdEspecialidads { get; set; } = new List<Especialidad>();

    //Checar le puse que puede ser nulo
    //public virtual Persona? IdDocenteNavigation { get; set; } = null!;
    public virtual ICollection<DocenteMateria> DocenteMaterias { get; set; } = new List<DocenteMateria>();
    public virtual Persona? IdDocenteNavigation { get; set; }
}
