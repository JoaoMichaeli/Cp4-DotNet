using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VisionHive.API.Models;
using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
using VisionHive.Domain.Entities;
using VisionHive.Domain.Pagination;
using VisionHive.Infrastructure.Contexts;
using VisionHive.Infrastructure.Repositories;

namespace VisionHive.API.Controllers
{
    [Route("api/[controller]")]
    [Tags("Motos")]
    [ApiController]
    public class MotoController : ControllerBase
    {
        private readonly IMotoRepository _motoRepository;
        private readonly IRepository<Moto> _repository;


        public MotoController(IRepository<Moto> repository, IMotoRepository motoRepository)
        {
            _repository = repository;
            _motoRepository = motoRepository;

        }

        // ====================================
        // [GET] /moto
        // Retorna todas as motos cadastradas
        // ====================================
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var motos = await _repository.GetAllAsync(ct);
            return Ok(motos);
        }

        // ============================
        // [GET] /moto/{id}
        // Busca uma moto específica por ID
        // ============================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var moto = await _repository.GetByIdAsync(id, ct);
            if (moto == null)
            {
                return NotFound("Moto não encontrada");
            }

            return Ok(moto);
        }

        // ============================
        // [GET] /moto/filtro
        // Filtro simples por prioridade e/ou placa
        // ============================
        [HttpGet("filtro")]
        public async Task<IActionResult> Filtrar(
            [FromQuery] int? prioridade, // enum prioridade como int
            [FromQuery] string? placa,
            CancellationToken ct = default)
        {
            var motos = await _repository.GetAllAsync(ct);
            if (prioridade.HasValue)
            {
                motos = motos.Where(m => (int)m.Prioridade == prioridade.Value).ToList();
            }

            if (!string.IsNullOrWhiteSpace(placa))
                motos = motos.Where(m => (m.Placa ?? string.Empty)
                        .Contains(placa, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            return Ok(motos);
        }

        // ============================
        // [POST] /moto
        // Cria uma nova moto
        // ============================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MotoInputModel input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var moto = new Moto(
                    placa: input.Placa,
                    chassi: input.Chassi,
                    numeroMotor: input.NumeroMotor,
                    prioridade: input.Prioridade,
                    patioId: input.PatioId
                );

                await _repository.AddAsync(moto, ct);
                await _repository.SaveChangesAsync(ct);

                return CreatedAtAction(nameof(GetById), new { id = moto.Id }, moto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadatrar moto: {ex.Message}");
            }
        }
        
        // ============================
        // [PUT] /moto/{id}
        // Atualiza os dados de uma moto existente
        // ============================
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] MotoInputModel input, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var motoExistente = await _repository.GetByIdAsync(id, ct);
            if (motoExistente == null)
            {
                return NotFound("Moto não encontrada");
            }

            try
            {
                motoExistente.AtualizarDados(
                    placa: input.Placa,
                    chassi: input.Chassi,
                    numeroMotor: input.NumeroMotor,
                    prioridade: input.Prioridade,
                    patioId: input.PatioId
                );

                await _repository.UpdateAsync(motoExistente, ct);
                await _repository.SaveChangesAsync(ct);
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar moto: {ex.Message}");
            }
        }
        
        // ============================
        // [DELETE] /moto/{id}
        // Remove uma moto pelo ID
        // ============================
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var moto = await _repository.GetByIdAsync(id, ct);
            if (moto == null)
            {
                return NotFound("Moto não encontrada");
            }

            try
            {
                await _repository.DeleteAsync(id, ct);
                await _repository.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir moto: {ex.Message}");
            }
        }
        
        // ============================
        // [GET] /moto/paginado?page=1&pageSize=10
        // Lista paginada via IMotoRepository
        // ============================
        [HttpGet("paginado")]
        [ProducesResponseType(typeof(PageResult<Moto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageResult<Moto>>> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var result = await _motoRepository.GetPaginationAsyncMoto(page, pageSize, ct);
            return Ok(result);
        }
    }
}
        

            
            
        
    
