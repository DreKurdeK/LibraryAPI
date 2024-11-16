using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Book?> GetByIdAsync(Guid id);
    Task AddAsync(BookDto book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Guid id);
}