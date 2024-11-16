﻿using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface IPublisherRepository
{
    Task<List<Publisher>> GetAllPublishersAsync();
    Task<Publisher?> GetByIdAsync(Guid id);
    Task AddAsync(Publisher publisher);
    Task UpdateAsync(Publisher publisher);
    Task DeleteAsync(Guid id);
}