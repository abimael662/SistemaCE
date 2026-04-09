using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Materia
{
    public int IdMateria { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<CalificacionAlumno> CalificacionAlumnos { get; set; } = new List<CalificacionAlumno>();

    //Agregado por mi @GG
    public virtual ICollection<DocenteMateria> DocenteMaterias { get; set; } = new List<DocenteMateria>();
}
