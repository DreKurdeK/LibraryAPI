using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class Author
{
    public Guid Id { get; set; } // Primary Key
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}