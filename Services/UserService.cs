using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.Services;

public class UserService(IConfiguration configuration) : IUserService
{
    private readonly PasswordHasher<User> _passwordHasher = new();
    private readonly IConfiguration _configuration = configuration;
    private readonly List<User> _users = new();


    public Task<User> RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<string> LoginAsync(LoginRequest request)
    {
        var user = _users.FirstOrDefault(u => u.Username == request.Username);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        var passwordVerificationResult =
            _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        return Task.FromResult(GenerateJwtToken(user));
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? string.Empty));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresIn"])),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}