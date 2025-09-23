using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;

namespace VisionHive.Application.UseCases;

public interface IPatioUseCase
{
    Task<Patio> CreateAsync(PatioRequest request,  CancellationToken ct =  default);
    Task<Patio?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Patio>> GetAllAsync(CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, PatioRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    
    Task<PageResult<Patio>> GetPaginationAsync(PaginatedRequest paginatedRequest, CancellationToken ct = default);
}