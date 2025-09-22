using VisionHive.Application.DTO.Request;
using VisionHive.Domain.Entities;
using VisionHive.Infrastructure.Repositories;

namespace VisionHive.Application.UseCases;

public class PatioUseCase : IPatioUseCase
{
    private readonly IRepository<Patio> _repository;

    public PatioUseCase(IRepository<Patio> repository)
    {
        _repository = repository;
    }

    public async Task<Patio> CreateAsync(
        PatioRequest request, CancellationToken ct = default)
    {
        var patio = request.toDomain();
        await _repository.AddAsync(patio, ct);
        await _repository.SaveChangesAsync(ct);
        return patio;
    }
    

    public Task<Patio?> GetByIdAsync(Guid id, CancellationToken ct = default) 
        => _repository.GetByIdAsync(id, ct);
    
    public Task<IEnumerable<Patio>> GetAllAsync(CancellationToken ct = default) 
        => _repository.GetAllAsync(ct);

    public async Task<bool> UpdateAsync(Guid id, PatioRequest request, CancellationToken ct = default)
    {
        var patio = await _repository.GetByIdAsync(id, ct);
        if (patio == null) return false;
        
        // método de dominio
        patio.AtualizarDados(request.Nome, request.LimiteMotos);
        _repository.Update(patio);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var patio = await _repository.GetByIdAsync(id, ct);
        if (patio == null) return false;

        await _repository.DeleteAsync(id, ct);
        await _repository.SaveChangesAsync(ct);
        return true;
    }
    
    
}