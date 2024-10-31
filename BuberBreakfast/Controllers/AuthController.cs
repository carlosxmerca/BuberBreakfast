using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuberBreakfast.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BuberBreakfast.Services.Users;

namespace BuberBreakfast.Controllers;

public class AuthController : ApiController
{
    private readonly IConfiguration configuration;
    private readonly IUserService _userService;

    public AuthController(IConfiguration configuration, IUserService userService)
    {
        this.configuration = configuration;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Auth([FromBody] LoginRequest user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest("Invalid username or password.");
        }

        var userResult = await _userService.GetUserAsync(user.UserName);
        if (userResult.IsError)
        {
            return Unauthorized();
        }

        var existingUser = userResult.Value;
        if (!_userService.VerifyPassword(user.Password, existingUser.Password))
        {
            return Unauthorized();
        }

        var jwtKey = configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new ArgumentNullException("Jwt:Key", "El valor de Jwt:Key no puede ser nulo o vacío.");
        }

        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        var key = Encoding.UTF8.GetBytes(jwtKey);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature
        );

        var subject = new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, existingUser.Id.ToString()),
            new Claim("userId", existingUser.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.UserName),
        });

        var expires = DateTime.UtcNow.AddMinutes(10);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return Ok(jwtToken);
    }
}
