using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class PublisherService(IPublisherRepository publisherRepository) : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository = publisherRepository;

    public async Task<Publisher> GetByIdAsync(Guid id)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id);
        if (publisher == null)
        {
            throw new PublisherNotFoundException(id);
        }
        return publisher;
    }

    public async Task<List<Publisher>> GetAllPublishersAsync()
    {
        return await _publisherRepository.GetAllPublishersAsync();
    }

    public async Task AddAsync(Publisher publisher)
    {

    }

    public async Task UpdateAsync(Publisher publisher)
    {
        var existingPublisher = await _publisherRepository.GetByIdAsync(publisher.Id);
        if (existingPublisher == null)
        {
            throw new PublisherNotFoundException(publisher.Id);
        }
        await _publisherRepository.UpdateAsync(publisher);
    }

    public async Task DeleteAsync(Guid id)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id);
        if (publisher == null)
        {
            throw new PublisherNotFoundException(id);
        }
        await _publisherRepository.DeleteAsync(id);
    }
}