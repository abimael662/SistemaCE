using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class DocenteMateriaGrupo
{
    public int IdDocenteMateriaGrupo { get; set; }

    public int IdDocente { get; set; }

    public int IdMateria { get; set; }

    public int IdGrupo { get; set; }

    [ValidateNever]
    public virtual Docente IdDocenteNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Grupo IdGrupoNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Materia IdMateriaNavigation { get; set; } = null!;
}
