using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Nivel
{
    public int IdNivel { get; set; }

    public string? Nombre { get; set; }

    public string? Nomenclatura { get; set; }

    public virtual ICollection<Carrera> Carreras { get; set; } = new List<Carrera>();

    public virtual ICollection<Titulacion> Titulacions { get; set; } = new List<Titulacion>();
}
