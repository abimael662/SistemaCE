using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Grupo
{
    public int IdGrupo { get; set; }

    public int? IdCarrera { get; set; }

    public string? Grupo1 { get; set; }

    public int? Cuatrimestre { get; set; }

    public string? Turno { get; set; }

    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    public virtual Carrera? IdCarreraNavigation { get; set; }

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();
}
