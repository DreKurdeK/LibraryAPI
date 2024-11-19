using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IAuthorRepository
{
    Task<PagedResult<Author>> GetAllAuthorsAsync(int pageNumber, int pageSize, string sortBy, 
        bool ascending);
    Task<Author?> GetByIdAsync(Guid id);
    Task AddAsync(AuthorDto authorDto);
    Task UpdateAsync(AuthorDto authorDto);
    Task DeleteAsync(Guid id);
}