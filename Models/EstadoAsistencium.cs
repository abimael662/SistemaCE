using System;
using System.Collections.Generic;

namespace SistemaCE.Models;

public partial class EstadoAsistencium
{
    public int IdEstado { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();
}
