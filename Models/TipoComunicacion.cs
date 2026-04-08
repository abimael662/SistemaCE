using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class TipoComunicacion
{
    public int IdTipoComunicacion { get; set; }

    public string? Comunicacion { get; set; }

    public virtual ICollection<ContactoPersona> ContactoPersonas { get; set; } = new List<ContactoPersona>();
}
