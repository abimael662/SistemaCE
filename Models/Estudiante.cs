using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Estudiante
{
    public int IdEstudiante { get; set; }

    public int? IdGrupo { get; set; }

    public int? Matricula { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    //public int? Estatus { get; set; }

    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    public virtual ICollection<CalificacionAlumno> CalificacionAlumnos { get; set; } = new List<CalificacionAlumno>();

    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    public virtual Persona IdEstudianteNavigation { get; set; } = null!;

    public virtual Grupo? IdGrupoNavigation { get; set; }

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();

    public virtual ICollection<Titulacion> Titulacions { get; set; } = new List<Titulacion>();
    public EstatusEstudiante? Estatus { get; set; }
}

public enum EstatusEstudiante
{
    Activo = 1,
    Baja = 2,
    Egresado = 3
}