using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Gifter.Extensions
{
    public static class ClaimsExtensions
    {
        public static string SubjectId(this ClaimsPrincipal user)
        {
            var userId = user?.Claims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;
            return userId != null ? Regex.Replace(userId, "auth0\\|", string.Empty) : null ;
        }
    }
}
