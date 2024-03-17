using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interface;

namespace Services.Extensions;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateTokenForAccount(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, user.Role.ToString()),
            new(ClaimTypes.Email, user.Email),
            new("userId", user.Id.ToString())
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["jwt:secret"]));

        var credential = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            _configuration["jwt:issuer"],
            _configuration["jwt:audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credential);

        return tokenHandler.WriteToken(token);
    }
}