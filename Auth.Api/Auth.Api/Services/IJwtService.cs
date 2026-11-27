using Auth.Api.Models;

namespace Auth.Api.Services;

public interface IJwtService
{
    string GenerarToken(AplicacionUser usuario, IList<string> roles);
}