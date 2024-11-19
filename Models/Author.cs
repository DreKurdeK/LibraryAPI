using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class Author
{
    public Guid Id { get; set; } // Primary Key
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}