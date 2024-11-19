using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IBookRepository
{
    Task<PagedResult<Book>> GetAllBooksAsync(int pageNumber, int pageSize, string sortBy = "Title", bool ascending = true);
    Task<Book?> GetByIdAsync(Guid id);
    Task AddAsync(BookDto bookDto);
    Task UpdateAsync(BookDto bookDto);
    Task DeleteAsync(Guid id);
}