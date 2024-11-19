using AutoMapper;
using FluentValidation;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class BookService(
    IBookRepository bookRepository,
    ILogger<BookService> logger) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly ILogger<BookService> _logger = logger;

    public async Task<PagedResult<Book>> GetAllBooksAsync(int pageNumber, int pageSize, string sortBy, bool ascending)
    {
        try
        {
            _logger.LogInformation("Fetching books from repository.");
            var result = await _bookRepository.GetAllBooksAsync(pageNumber, pageSize, sortBy, ascending);

            _logger.LogInformation($"Fetched {result.Items.Count()} books.");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching books.");
            throw new Exception("An error occurred while fetching books.", ex);
        }
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Fetching book with ID: {id}");
            var book = await _bookRepository.GetByIdAsync(id);
            return book;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching book.");
            throw new Exception("An error occurred while fetching book.", ex);
        }
    }

    public async Task AddAsync(BookDto bookDto)
    {
        try
        {
            await _bookRepository.AddAsync(bookDto);
            _logger.LogInformation("Book added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding book.");
            throw new Exception("An error occurred while adding book.", ex);
        }
    }

    public async Task UpdateAsync(BookDto bookDto)
    {
        try
        {
            await _bookRepository.UpdateAsync(bookDto);
            _logger.LogInformation("Book updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating book.");
            throw new Exception("An error occurred while updating book.", ex);
        }
    }



    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _bookRepository.DeleteAsync(id);
            _logger.LogInformation($"Book with ID: {id} deleted successfully.");
        }
        catch (BookNotFoundException ex)
        {
            _logger.LogError(ex, $"Book with ID {ex.BookId} could not be found for deletion.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting book.");
            throw new Exception("An error occurred while deleting book.", ex);
        }
    }
}