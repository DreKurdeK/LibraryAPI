﻿using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories;

public class AuthorRepository(LibraryDbContext dbContext) : IAuthorRepository
{
    private readonly LibraryDbContext _dbContext = dbContext;
    
    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        return await _dbContext.Authors.ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Authors.FindAsync(id);
    }

    public async Task AddAsync(Author author)
    {
        await _dbContext.Authors.AddAsync(author);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Author author)
    {
        _dbContext.Authors.Update(author);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var author = await _dbContext.Authors.FindAsync(id);
        if (author != null)
        {
            _dbContext.Authors.Remove(author);
            await _dbContext.SaveChangesAsync();
        }
    }
}