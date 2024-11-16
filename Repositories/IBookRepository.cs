using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Book?> GetByIdAsync(Guid id);
    Task AddAsync(BookDto bookDto);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Guid id);
}