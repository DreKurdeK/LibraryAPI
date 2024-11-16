using AutoMapper;
using FluentValidation;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class BookService(
    IBookRepository bookRepository,
    ILogger<BookService> logger,
    IValidator<Book> bookValidator,
    IValidator<BookDto> bookDtoValidator) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IValidator<Book> _bookValidator = bookValidator;
    private readonly IValidator<BookDto> _bookDtoValidator = bookDtoValidator;
    private readonly ILogger<BookService> _logger = logger;

    public async Task<List<Book>> GetAllBooksAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all books.");
            return await _bookRepository.GetAllBooksAsync();
        }
        catch (BookNotFoundException ex)
        {
            _logger.LogError(ex, $"Book with ID {ex.BookId} was not found.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching book.");
            throw new Exception("An error occurred while fetching book.", ex);
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
            var validationResult = await _bookDtoValidator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid book data provided.");
                throw new Exception("Book data is invalid.");
            }
            
            await _bookRepository.AddAsync(bookDto);
            _logger.LogInformation("Book added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding book.");
            throw new Exception("An error occurred while adding book.", ex);
        }
    }
    
    public async Task UpdateAsync(Book book)
    {
        try
        {
            var validationResult = await _bookValidator.ValidateAsync(book);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid book data provided.");
                throw new Exception("Book data is invalid.");
            }
            
            await _bookRepository.UpdateAsync(book);

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