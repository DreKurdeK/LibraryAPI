using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IPublisherRepository
{
    Task<PagedResult<Publisher>> GetAllPublishersAsync(int pageNumber, int pageSize, string sortBy = "Name",
        bool ascending = true);
    Task<Publisher?> GetByIdAsync(Guid id);
    Task AddAsync(PublisherDto publisher);
    Task UpdateAsync(PublisherDto publisherDto);
    Task DeleteAsync(Guid id);
}