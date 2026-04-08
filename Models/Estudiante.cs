using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SistemaCE.Models;

public partial class Estudiante
{
    public int IdEstudiante { get; set; }

    public int? IdGrupo { get; set; }

    public int? Matricula { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public int? Estatus { get; set; }

    public virtual ICollection<CalificacionAlumno> CalificacionAlumnos { get; set; } = new List<CalificacionAlumno>();

    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    // 🔥 IMPORTANTE: evitar validación en formularios
    [ValidateNever]
    public virtual Persona IdEstudianteNavigation { get; set; } = null!;

    // 🔥 también recomendado
    [ValidateNever]
    public virtual Grupo? IdGrupoNavigation { get; set; }

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();

    public virtual ICollection<Titulacion> Titulacions { get; set; } = new List<Titulacion>();
}