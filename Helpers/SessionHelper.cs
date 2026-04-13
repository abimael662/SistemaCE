using SistemaCE.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace SistemaCE.Helpers
{
    public class SessionHelper
    {
        public static string GetValue(IPrincipal user, string property)
        {
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
                return "";

            var identity = ((ClaimsIdentity)user.Identity).FindFirst(property);
            return identity == null ? "" : identity.Value;
        }

        public static string GetNameIdentifier(IPrincipal user)
        {
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
                return "";

            var identity = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier);
            return identity == null ? "" : identity.Value;
        }

        public static string GetEmail(IPrincipal user)
        {
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
                return "";

            var identity = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Email);
            return identity == null ? "" : identity.Value;
        }

        public static string GetName(IPrincipal user)
        {
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
                return "";

            var identity = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Name);
            return identity == null ? "" : identity.Value;
        }

        public static string GetClaim(IPrincipal user, string type)
        {
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
                return "";

            var identity = (ClaimsIdentity)user.Identity;
            return identity.FindFirst(type)?.Value ?? "";
        }
    }
}
