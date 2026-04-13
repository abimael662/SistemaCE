using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class GrupoBase
{
    public int IdGrupoBase { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}
