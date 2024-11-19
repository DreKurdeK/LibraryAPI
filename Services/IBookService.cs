using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Book?> GetByIdAsync(Guid id);
    Task AddAsync(BookDto book);
    Task UpdateAsync(BookDto bookDto);
    Task DeleteAsync(Guid id);
}