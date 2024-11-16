using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class Publisher
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    [MaxLength(200, ErrorMessage = "Name cannot be longer than 200 characters")]
    public string Name { get; set; }
    
    [StringLength(500, ErrorMessage = "Address cannot be longer than 500 characters")]
    public string Address { get; set; }
    
    [Required(ErrorMessage = "Year is required")]
    [Range(1800, 2024, ErrorMessage = "Year must be between 1900 and 2019")]
    public int FoundedYear { get; set; }
}