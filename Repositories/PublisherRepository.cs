﻿using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories;

public class PublisherRepository(LibraryDbContext dbContext) : IPublisherRepository
{
    private readonly LibraryDbContext _dbContext = dbContext;
    
    public async Task<List<Publisher>> GetAllPublishersAsync()
    {
        return await _dbContext.Publishers.ToListAsync();
    }

    public async Task<Publisher?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Publishers.FindAsync(id);
    }

    public async Task AddAsync(Publisher publisher)
    {
        await _dbContext.Publishers.AddAsync(publisher);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Publisher publisher)
    {
        _dbContext.Publishers.Update(publisher);
        await _dbContext.SaveChangesAsync();
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