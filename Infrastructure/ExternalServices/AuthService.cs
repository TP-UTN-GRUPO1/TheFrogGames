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
        var claims = new[]
        {
                new Claim("idUser", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        Console.WriteLine(_configuration["Jwt:Key"]);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
          issuer: _configuration["Jwt:Issuer"],
          audience: _configuration["Jwt:Audience"],
          claims: claims,
          expires: DateTime.Now.AddHours(1),
          signingCredentials: creds);


        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;


    }



}
