using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class Certificado
{
    public int IdCertificado { get; set; }

    public int? IdGrupo { get; set; }

    public int? IdEstudiante { get; set; }

    public string? TipoCertificado { get; set; }

    public DateOnly? FechaEmision { get; set; }

    public string? Descripcion { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }

    public virtual Grupo? IdGrupoNavigation { get; set; }
}
