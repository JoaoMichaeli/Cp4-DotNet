
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Repositories;

namespace VisionHive.Application.UseCases;

public class MotoUseCase : IMotoUseCase
{
    private readonly IMotoRepository _motoRepository;

    public MotoUseCase(IMotoRepository motoRepository)
        => _motoRepository = motoRepository;

    public Task<PageResult<Moto>> GetPaginationAsyncMoto(
        int page, int pageSize, CancellationToken ct = default)
        => _motoRepository.GetPaginationAsyncMoto(page, pageSize, ct);

    public async Task<Moto> CreateAsync(Moto moto, CancellationToken ct = default)
    {
        await _motoRepository.AddAsync(moto, ct);
        await _motoRepository.SaveChangesAsync(ct);
        return moto;
    }
}
    
