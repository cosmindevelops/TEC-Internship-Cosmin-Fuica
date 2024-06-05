using Internship.Model;

namespace ApiApp.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user, List<string> roles);
}