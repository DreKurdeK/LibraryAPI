using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IAuthorService
{
    Task<List<Author>> GetAllAuthorsAsync();
    Task<Author?> GetByIdAsync(Guid id);
    Task AddAsync(AuthorDto authorDto);
    Task UpdateAsync(Author author);
    Task DeleteAsync(Guid id);
}