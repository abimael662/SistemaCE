using SistemaCE.Models;
namespace SistemaCE.Helpers
{
    public class CalificacionHelper
    {
        public static string ObtenerLiteral(decimal? valor)
        {
            if (valor == null || valor == 0) return "SE";
            if (valor < 8) return "NA";
            if (valor < 9) return "SA";
            if (valor < 10) return "DE";
            return "AU";
        }
    }
}