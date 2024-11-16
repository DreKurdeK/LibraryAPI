using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories;

public class PublisherRepository(LibraryDbContext dbContext, IMapper mapper) : IPublisherRepository
{
    private readonly LibraryDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<List<Publisher>> GetAllPublishersAsync()
    {
        return await _dbContext.Publishers.ToListAsync();
    }

    public async Task<Publisher?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Publishers.FindAsync(id);
    }

    public async Task AddAsync(PublisherDTO publisherDto)
    {
        var publisher = _mapper.Map<Publisher>(publisherDto);
        publisher.Id = Guid.NewGuid();
        await _dbContext.Publishers.AddAsync(publisher);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Publisher publisher)
    {
        var publisherUpdated = await _dbContext.Publishers.FindAsync(publisher.Id);
        if (publisherUpdated != null)
        {
            _mapper.Map(publisher, publisherUpdated);
            _dbContext.Publishers.Update(publisherUpdated);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var publisher = await _dbContext.Publishers.FindAsync(id);
        if (publisher != null)
        {
            _dbContext.Publishers.Remove(publisher);
            await _dbContext.SaveChangesAsync();
        }
    }
}