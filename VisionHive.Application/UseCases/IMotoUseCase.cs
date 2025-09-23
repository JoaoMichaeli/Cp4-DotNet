using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;

namespace VisionHive.Application.UseCases;

public interface IMotoUseCase
{
    Task<PageResult<Moto>> GetPaginationAsyncMoto( int page, int pageSize, CancellationToken ct = default);
    Task<Moto> CreateAsync(Moto moto, CancellationToken ct = default);
}