using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstraction;
using Application.Abstraction.ExternalServices;
using Contracts.User.Request;

namespace Infrastructure.ExternalServices;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public string Login(LoginUserRequest request)
    {
        var user = _userRepository.GetUserByEmailAndPassword(request);
        if (user == null)
        {
            return string.Empty;
        }

        var keyValue = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expireMinutes = int.TryParse(_configuration["Jwt:ExpireMinutes"], out var m) ? m : 60;

        if (string.IsNullOrEmpty(keyValue))
            throw new InvalidOperationException("Jwt:Key no está configurado. Verifica user-secrets o Key Vault.");
        if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            throw new InvalidOperationException("Jwt:Issuer o Jwt:Audience no están configurados.");

        var claims = new[]
        {
            new Claim("idUser", user.Id.ToString()),
            new Claim("userName", user.Name ?? string.Empty),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(expireMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
