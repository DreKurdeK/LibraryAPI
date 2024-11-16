using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Book?> GetByIdAsync(Guid id);
    Task AddAsync(Book book);
    Task UpdateAsync(Guid id, Book book);
    Task DeleteAsync(Guid id);
}