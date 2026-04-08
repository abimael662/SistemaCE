using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class PersonaUsuario
{
    public int IdPersona { get; set; }

    public string? Usuario { get; set; }

    public string? Password { get; set; }

    public virtual Persona IdPersonaNavigation { get; set; } = null!;
}
