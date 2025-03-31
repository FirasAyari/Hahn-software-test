using Domain.Core.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ApiEntryRepository
{
    private readonly ApiDbContext _context;

    public ApiEntryRepository(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApiEntry>> GetAllAsync()
    {
        return await _context.ApiEntries.ToListAsync();
    }

    public async Task AddRangeAsync(IEnumerable<ApiEntry> entities)
    {
        await _context.ApiEntries.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task ClearTable()
    {
        _context.ApiEntries.RemoveRange(_context.ApiEntries);
        await _context.SaveChangesAsync();
    }
} 