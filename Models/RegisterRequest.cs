namespace LibraryAPI.Models;

public class RegisterRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
    public required string Email { get; set; }
}
