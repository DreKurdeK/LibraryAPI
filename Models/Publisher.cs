using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class Publisher
{
    public Guid Id { get; set; } // Primary Key
    public string Name { get; set; }
    public string Address { get; set; }
    public int FoundedYear { get; set; }
}