using System.ComponentModel.DataAnnotations;
namespace LibraryAPI.Models;

public class User
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public string PasswordHash { get; set; }
    public required string Email { get; set; }
    public required ICollection<UserRole> UserRoles { get; set; }
}