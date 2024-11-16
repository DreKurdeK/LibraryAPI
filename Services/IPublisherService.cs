using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IPublisherService
{
    Task<List<Publisher>> GetAllPublishersAsync();
    Task<Publisher?> GetByIdAsync(Guid id);
    Task AddAsync(Publisher publisher);
    Task UpdateAsync(Publisher publisher);
    Task DeleteAsync(Guid id);
}