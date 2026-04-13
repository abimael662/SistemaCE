using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Grupo
{
    public int IdGrupo { get; set; }

    public int? IdCarrera { get; set; }

    public string? Turno { get; set; }

    public int? IdGrupoBase { get; set; }

    public int? IdPeriodo { get; set; }

    public virtual ICollection<CalificacionAlumno> CalificacionAlumnos { get; set; } = new List<CalificacionAlumno>();

    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    public virtual ICollection<DocenteMateriaGrupo> DocenteMateriaGrupos { get; set; } = new List<DocenteMateriaGrupo>();

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    public virtual Carrera? IdCarreraNavigation { get; set; }

    public virtual GrupoBase? IdGrupoBaseNavigation { get; set; }

    public virtual Periodo? IdPeriodoNavigation { get; set; }

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();

    public virtual ICollection<SesionClase> SesionClases { get; set; } = new List<SesionClase>();
}
