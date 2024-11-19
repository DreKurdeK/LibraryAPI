using FluentValidation;
using LibraryAPI.DTOs;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class PublisherService(
    IPublisherRepository publisherRepository,
    ILogger<BookService> logger) : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository = publisherRepository;
    private readonly ILogger<BookService> _logger = logger;
    
    public async Task<PagedResult<Publisher>> GetAllPublishersAsync(int pageNumber, int pageSize, string sortBy = "Name", bool ascending = true)
    {
        try
        {
            _logger.LogInformation("Fetching publishers from repository.");

            // Wywołanie repozytorium z parametrami sortowania, paginacji
            var result = await _publisherRepository.GetAllPublishersAsync(pageNumber, pageSize, sortBy, ascending);

            _logger.LogInformation($"Fetched {result.Items.Count()} publishers.");

            return result;
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
        catch (PublisherNotFoundException ex)
        {
            _logger.LogError(ex, $"Publisher with ID {ex.PublisherId} was not found.");
            throw;
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
            await _publisherRepository.AddAsync(publisherDto);
            _logger.LogInformation("Publisher added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding publisher.");
            throw new Exception("An error occurred while adding publisher.", ex);
        }
    }

    public async Task UpdateAsync(PublisherDto publisherDto)
    {
        try
        {
            await _publisherRepository.UpdateAsync(publisherDto);
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
        catch (PublisherNotFoundException ex)
        {
            _logger.LogError(ex, $"Publisher with ID {ex.PublisherId} could not be found for deletion.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting publisher.");
            throw new Exception("An error occurred while deleting publisher.", ex);
        }
    }
}