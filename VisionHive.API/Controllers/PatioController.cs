using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
using VisionHive.Application.UseCases;
using VisionHive.Infrastructure.Contexts;

namespace VisionHive.API.Controllers
{
    [Route("api/[controller]")]
    [Tags("Patios")]
    [ApiController]
    public class PatioController : ControllerBase
    {
        private readonly IPatioUseCase _patioUseCase;

        public PatioController(IPatioUseCase patioUseCase)
        {
            _patioUseCase = patioUseCase;
        }

        // ---------- GET /api/patio (paginado) ----------
        /// <summary>
        /// Lista paginada de pátios.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PageResult<PatioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageResult<PatioDto>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var query = _context.Patios
                .AsNoTracking()
                .OrderBy(p => p.Nome);

            var total = await query.CountAsync(ct);

            var pageEntities = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            var items = pageEntities.Select(p => new PatioDto
            {
                PatioId = p.PatioId.ToString(),
                Nome = p.Nome,
                Endereco = p.Endereco ?? string.Empty,
                Links = BuildPatioLinks(p.PatioId)
            }).ToList();

            var result = new PageResult<PatioDto>
            {
                Items = items,
                Total = total,
                Page = page,
                PageSize = pageSize,
                HasMore = page * pageSize < total
            };

            return Ok(result);
        }

        // ---------- GET /api/patio/{id} ----------
        /// <summary>
        /// Busca um pátio pelo Id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PatioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
        {
            var patio = await _context.Patios
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PatioId == id, ct);

            if (patio is null) return NotFound("Pátio não encontrado");

            var dto = new PatioDto
            {
                PatioId = patio.PatioId.ToString(),
                Nome = patio.Nome,
                Endereco = patio.Endereco ?? string.Empty,
                Links = BuildPatioLinks(patio.PatioId)
            };

            return Ok(dto);
        }

        // ---------- POST /api/patio ----------
        /// <summary>
        /// Cria um novo pátio.
        /// </summary>
        // [SwaggerRequestExample(typeof(PatioInputModel), typeof(VisionHive.API.SwaggerExamples.PatioInputExample))]
        [HttpPost]
        [ProducesResponseType(typeof(PatioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] PatioInputModel input, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var patio = new Patio
            {
                PatioId = Guid.NewGuid(),
                Nome = input.Nome,
                Endereco = input.Endereco
            };

            _context.Patios.Add(patio);
            await _context.SaveChangesAsync(ct);

            var dto = new PatioDto
            {
                PatioId = patio.PatioId.ToString(),
                Nome = patio.Nome,
                Endereco = patio.Endereco ?? string.Empty,
                Links = BuildPatioLinks(patio.PatioId)
            };

            return CreatedAtAction(nameof(GetById), new { id = patio.PatioId }, dto);
        }

        // ---------- PUT /api/patio/{id} ----------
        /// <summary>
        /// Atualiza um pátio existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] PatioInputModel input, CancellationToken ct = default)
        {
            var patio = await _context.Patios.FirstOrDefaultAsync(p => p.PatioId == id, ct);
            if (patio is null) return NotFound("Pátio não encontrado");

            patio.Nome = input.Nome;
            patio.Endereco = input.Endereco;

            await _context.SaveChangesAsync(ct);
            return NoContent();
        }

        // ---------- DELETE /api/patio/{id} ----------
        /// <summary>
        /// Remove um pátio pelo Id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
        {
            var patio = await _context.Patios.FirstOrDefaultAsync(p => p.PatioId == id, ct);
            if (patio is null) return NotFound("Pátio não encontrado");

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}