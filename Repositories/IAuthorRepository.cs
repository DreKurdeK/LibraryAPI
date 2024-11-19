using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IAuthorRepository
{
    Task<List<Author>> GetAllAuthorsAsync();
    Task<Author?> GetByIdAsync(Guid id);
    Task AddAsync(AuthorDto authorDto);
    Task UpdateAsync(AuthorDto authorDto);
    Task DeleteAsync(Guid id);
}