using System.Security.Claims;
using LibraryAPI.Models;
using LibraryAPI.Services;
using LibraryAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var user = await _userService.RegisterAsync(registerRequest);
        return Ok(new { user.Id, user.Username,user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var token = await _userService.LoginAsync(loginRequest);
        return Ok(new { Token = token });
    }
    
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var validator = new ChangePasswordRequestValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Something went wrong."));
        var result = await _userService.ChangePasswordAsync(userId, request);

        return Ok(result);
    }
}