using FluentValidation;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class PublisherService(
    IPublisherRepository publisherRepository,
    ILogger<BookService> logger,
    IValidator<Publisher> publisherValidator,
    IValidator<PublisherDto> publisherDtoValidator) : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository = publisherRepository;
    private readonly ILogger<BookService> _logger = logger;
    private readonly IValidator<Publisher> _publisherValidator = publisherValidator;
    private readonly IValidator<PublisherDto> _publisherDtoValidator = publisherDtoValidator;

    public async Task<List<Publisher>> GetAllPublishersAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all publishers.");
            return await _publisherRepository.GetAllPublishersAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching publishers.");
            throw new Exception("An error occurred while fetching publishers.", ex);
        }
    }
    
    public async Task<Publisher?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Fetching publisher with ID: {id}");
            var publisher = await _publisherRepository.GetByIdAsync(id);
            return publisher;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching publisher.");
            throw new Exception("An error occurred while fetching publisher.", ex);
        }
    }

    public async Task AddAsync(PublisherDto publisherDto)
    {
        try
        {
            var validationResult = await _publisherDtoValidator.ValidateAsync(publisherDto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid publisher data provided.");
                throw new Exception("Publisher data is invalid.");
            }
            
            await _publisherRepository.AddAsync(publisherDto);

            _logger.LogInformation("Publisher added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding publisher.");
            throw new Exception("An error occurred while adding publisher.", ex);
        }
    }

    public async Task UpdateAsync(Publisher publisher)
    {
        try
        {
            var validationResult = await _publisherValidator.ValidateAsync(publisher);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid publisher data provided.");
                throw new Exception("Publisher data is invalid.");
            }

            await _publisherRepository.UpdateAsync(publisher);

            _logger.LogInformation("Publisher updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating publisher.");
            throw new Exception("An error occurred while updating publisher.", ex);
        }
    }
    
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _publisherRepository.DeleteAsync(id);
            _logger.LogInformation($"Publisher with ID: {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting publisher.");
            throw new Exception("An error occurred while deleting publisher.", ex);
        }
    }
}