using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IAuthorService
{
    Task<List<Author>> GetAllAuthorsAsync();
    Task<Author?> GetByIdAsync(Guid id);
    Task AddAsync(Author author);
    Task UpdateAsync(Author author);
    Task DeleteAsync(Guid id);
}