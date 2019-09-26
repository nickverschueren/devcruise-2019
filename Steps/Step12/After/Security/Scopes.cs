using System.Linq;
using System.Security.Claims;

namespace DevCruise.Security
{
    public static class Scopes
    {
        public const string ReadAccess = "readAccess";
        public const string WriteAccess = "writeAccess";

        public const string AadScopePrefix = "api://devCruiseApi/";
        public const string AadReadAccess = AadScopePrefix + ReadAccess;
        public const string AadWriteAccess = AadScopePrefix + WriteAccess;

        public const string ScopeClaimType = "http://schemas.microsoft.com/identity/claims/scope";

        public static bool HasScope(this ClaimsPrincipal user, string scopeName)
            => user.FindFirst(Scopes.ScopeClaimType)?.Value.Split(' ').Contains(scopeName) ?? false;
    }
}