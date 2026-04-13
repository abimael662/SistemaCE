using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class ContactoPersona
{
    public int IdContacto { get; set; }

    public int? IdPersona { get; set; }

    public int? TipoComunicacion { get; set; }

    public string? Dato { get; set; }
    
    [ValidateNever]
    public virtual Persona? IdPersonaNavigation { get; set; }

    [ValidateNever]
    public virtual TipoComunicacion? TipoComunicacionNavigation { get; set; }
}
