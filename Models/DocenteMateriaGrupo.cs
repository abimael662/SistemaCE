using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class DocenteMateriaGrupo
{
    public int IdDocenteMateriaGrupo { get; set; }

    public int IdDocente { get; set; }

    public int IdMateria { get; set; }

    public int IdGrupo { get; set; }

    public virtual Docente IdDocenteNavigation { get; set; } = null!;

    public virtual Grupo IdGrupoNavigation { get; set; } = null!;

    public virtual Materia IdMateriaNavigation { get; set; } = null!;
}
