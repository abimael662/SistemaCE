using SistemaCE.Models;

namespace SistemaCE.Helpers
{
    public class CalificacionHelper
    {
        public static TipoCalificacion ObtenerLiteral(decimal? valor)
        {
            if (valor == null || valor == 0) return TipoCalificacion.SE;
            if (valor < 8) return TipoCalificacion.NA;
            if (valor < 9) return TipoCalificacion.SA;
            if (valor < 10) return TipoCalificacion.DE;
            return TipoCalificacion.AU;
        }
    }
}