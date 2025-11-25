using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _config;

    public JwtTokenGenerator(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Secret"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("TenantId", user.TenantId.ToString()),
            new Claim("Email", user.Email),
            new Claim("IsAdmin", user.IsAdmin.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
