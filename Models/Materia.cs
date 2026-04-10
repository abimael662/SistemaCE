using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Materia
{
    public int IdMateria { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<CalificacionAlumno> CalificacionAlumnos { get; set; } = new List<CalificacionAlumno>();

    public virtual ICollection<DocenteMateriaGrupo> DocenteMateriaGrupos { get; set; } = new List<DocenteMateriaGrupo>();

    public virtual ICollection<SesionClase> SesionClases { get; set; } = new List<SesionClase>();
}
