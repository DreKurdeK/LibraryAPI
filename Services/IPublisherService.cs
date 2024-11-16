using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IPublisherService
{
    Task<List<Publisher>> GetAllPublishersAsync();
    Task<Publisher?> GetByIdAsync(Guid id);
    Task AddAsync(PublisherDto publisherDto);
    Task UpdateAsync(Publisher publisher);
    Task DeleteAsync(Guid id);
}