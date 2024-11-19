using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IBookService
{
    Task<PagedResult<Book>> GetAllBooksAsync(int pageNumber, int pageSize, string sortBy, bool ascending);
    Task<Book?> GetByIdAsync(Guid id);
    Task AddAsync(BookDto book);
    Task UpdateAsync(BookDto bookDto);
    Task DeleteAsync(Guid id);
}