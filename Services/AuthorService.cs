using AutoMapper;
using FluentValidation;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class AuthorService(
    IAuthorRepository authorRepository, 
    IMapper mapper,
    IValidator<AuthorDto> authorDtoValidator,
    IValidator<Author> authorValidator,
    ILogger<AuthorService> logger) : IAuthorService
{
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IValidator<AuthorDto> _authorDtoValidator = authorDtoValidator;
    private readonly IValidator<Author> _authorValidator = authorValidator;
    private readonly ILogger<AuthorService> _logger = logger;

    
    public async Task<Author?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Fetching author with ID: {id}");
            var author = await _authorRepository.GetByIdAsync(id);
            return author;
        }
        catch (AuthorNotFoundException ex)
        {
            _logger.LogError(ex, $"Author with ID {ex.AuthorId} was not found.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching author.");
            throw new Exception("An error occurred while fetching author.", ex);
        }
    }

    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all authors.");
            return await _authorRepository.GetAllAuthorsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching authors.");
            throw new Exception("An error occurred while fetching authors.", ex);
        }
    }

    public async Task AddAsync(AuthorDto authorDto)
    {
        try
        {
            var validationResult = await _authorDtoValidator.ValidateAsync(authorDto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid author data provided.");
                throw new Exception("Author data is invalid.");
            }
            
            await _authorRepository.AddAsync(authorDto);

            _logger.LogInformation("Author added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding author.");
            throw new Exception("An error occurred while adding author.", ex);
        }
    }

    public async Task UpdateAsync(Author author)
    {
        try
        {
            var validationResult = await _authorValidator.ValidateAsync(author);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid author data provided.");
                throw new Exception("Author data is invalid.");
            }

            await _authorRepository.UpdateAsync(author);

            _logger.LogInformation("Author updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating author.");
            throw new Exception("An error occurred while updating author.", ex);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _authorRepository.DeleteAsync(id);
            _logger.LogInformation($"Author with ID: {id} deleted successfully.");
        }
        catch (AuthorNotFoundException ex)
        {
            _logger.LogError(ex, $"Author with ID {ex.AuthorId} could not be found for deletion.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting author.");
            throw new Exception("An error occurred while deleting author.", ex);
        }
    }
}