using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Inscripcion
{
    public int IdInscripcion { get; set; }

    public int? IdGrupo { get; set; }

    public int? IdEstudiante { get; set; }

    public DateOnly? FechaInscripcion { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }

    public virtual Grupo? IdGrupoNavigation { get; set; }
}
