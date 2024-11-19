using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IPublisherService
{
    Task<PagedResult<Publisher>> GetAllPublishersAsync(int pageNumber, int pageSize, string sortBy = "Name",
        bool ascending = true);
    Task<Publisher?> GetByIdAsync(Guid id);
    Task AddAsync(PublisherDto publisherDto);
    Task UpdateAsync(PublisherDto publisherDto);
    Task DeleteAsync(Guid id);
}