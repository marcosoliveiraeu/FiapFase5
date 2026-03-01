using System.Security.Claims;

namespace Identity.Api.Auth
{
    public static class ClaimsExtensions
    {
        public static Guid ObterUsuarioId(this ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(sub))
                throw new UnauthorizedAccessException("Token não contém claim 'sub'.");

            if (!Guid.TryParse(sub, out var usuarioId))
                throw new UnauthorizedAccessException("Claim 'sub' inválida.");

            return usuarioId;
        }
    }
}
