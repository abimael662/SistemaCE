using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class SesionClase
{
    public int IdSesion { get; set; }

    public int? IdDocente { get; set; }

    public int? IdMateria { get; set; }

    public int? IdGrupo { get; set; }

    public DateOnly? Fecha { get; set; }

    public TimeOnly? HoraInicio { get; set; }

    public TimeOnly? HoraFin { get; set; }

    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    public virtual Docente? IdDocenteNavigation { get; set; }

    public virtual Grupo? IdGrupoNavigation { get; set; }

    public virtual Materia? IdMateriaNavigation { get; set; }
}
