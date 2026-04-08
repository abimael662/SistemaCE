using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Carrera
{
    public int IdCarrera { get; set; }

    public string? Nombre { get; set; }

    public int? IdNivel { get; set; }

    public int? IdDivision { get; set; }

    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();

    public virtual Division? IdDivisionNavigation { get; set; }

    public virtual Nivel? IdNivelNavigation { get; set; }
}
