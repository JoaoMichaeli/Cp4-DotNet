using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;

using VisionHive.Infrastructure.Contexts;

namespace VisionHive.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly VisionHiveContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(VisionHiveContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // retorna todos os registros da entidade T
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // busca um registro por id 
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    // adiciona uma entidade existente
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    // atualiza uma entidade existe
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    // atualiza uma entidade ja existente -> assinatura assincrona por compatibilidade
    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    // remove uma entidade ja carregada
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
    
    // remove uma entidade buscanod pelo ID
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }
    
    // persiste todas as alterações (INSERT, UPDATE, DELETE) no banco
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    
}