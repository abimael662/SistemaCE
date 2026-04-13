using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
    
    [ValidateNever]
    public virtual Administrativo? Administrativo { get; set; }

    public virtual ICollection<ContactoPersona> ContactoPersonas { get; set; } = new List<ContactoPersona>();

    [ValidateNever]
    public virtual Docente? Docente { get; set; }

    [ValidateNever]
    public virtual Estudiante? Estudiante { get; set; }

    [ValidateNever]
    public virtual PersonaUsuario? PersonaUsuario { get; set; }
}
