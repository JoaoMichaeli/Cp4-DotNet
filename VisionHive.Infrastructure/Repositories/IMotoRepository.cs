using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;

namespace VisionHive.Infrastructure.Repositories;

public interface IMotoRepository : IRepository<Moto>
{
    Task<PageResult<Moto>> GetPaginationAsyncMoto(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
    
}