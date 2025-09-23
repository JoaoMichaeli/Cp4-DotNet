using Microsoft.EntityFrameworkCore;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Contexts;
using VisionHive.Infrastructure.Repositories;


namespace VisionHive.Infrastructure.Repositories;

public class MotoRepository : Repository<Moto>, IMotoRepository
{
    private readonly VisionHiveContext _context;
    public MotoRepository(VisionHiveContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PageResult<Moto>> GetPaginationAsyncMoto(
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var query = _context.Motos
            .AsNoTracking()
            .OrderByDescending(m => m.Id);

        var total = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PageResult<Moto>
        {
            Items = items,
            Total = total,
            HasMore = page * pageSize < total,
            Page = page,
            PageSize = pageSize
        };
    }
}
