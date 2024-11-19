using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IPublisherRepository
{
    Task<List<Publisher>> GetAllPublishersAsync();
    Task<Publisher?> GetByIdAsync(Guid id);
    Task AddAsync(PublisherDto publisher);
    Task UpdateAsync(PublisherDto publisherDto);
    Task DeleteAsync(Guid id);
}