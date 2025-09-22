using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;

using VisionHive.Infrastructure.Contexts;

namespace VisionHive.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly VisionHiveContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(VisionHiveContext context)
    {
        _context = context;
        _dbSet  = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        => await _dbSet.AsNoTracking().ToListAsync(ct);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _dbSet.FindAsync(new object?[] { id }, ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _dbSet.AddAsync(entity, ct);

    public void Update(T entity) => _dbSet.Update(entity);

    public Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public void Delete(T entity) => _dbSet.Remove(entity);

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _dbSet.FindAsync(new object?[] { id }, ct);
        if (entity is not null) _dbSet.Remove(entity);
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
    
    
