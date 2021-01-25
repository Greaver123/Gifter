using System;
using System.Linq;
using System.Security.Claims;

namespace Gifter.Extensions
{
    public static class ClaimsExtensions
    {
        public static string SubjectId(this ClaimsPrincipal user) { return user?.Claims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value; }

    }
}
