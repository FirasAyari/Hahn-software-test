using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    public DbSet<ApiEntry> ApiEntries { get; set; }
}