using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Persona
{
    public int IdPersona { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public string? Curp { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public virtual Administrativo? Administrativo { get; set; }

    public virtual ICollection<ContactoPersona> ContactoPersonas { get; set; } = new List<ContactoPersona>();

    public virtual Docente? Docente { get; set; }

    public virtual Estudiante? Estudiante { get; set; }

    public virtual PersonaUsuario? PersonaUsuario { get; set; }
}
