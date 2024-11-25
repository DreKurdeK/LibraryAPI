using LibraryAPI.Models;
using LibraryAPI.Services;
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
}