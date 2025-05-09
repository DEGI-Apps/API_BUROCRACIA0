using API_BASE.Application.Common;
using API_BASE.Application.Interfaces;
using API_BASE.Domain.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_BASE.API.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TEntity> : ControllerBase where TEntity : AuditableEntity
    {
        private readonly IRepository<TEntity> _repository;

        public BaseController(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetById:{id}")]
        public async Task<ActionResult<TEntity>> GetById(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpGet("GetPaginated")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<TEntity>>>> GetAll([FromQuery] PaginationRequest paging)
        {
            var query = _repository.GetQueryable();
            var total = await query.CountAsync();

            var data = await query
                .Skip((paging.Page - 1) * paging.Limit)
                .Take(paging.Limit)
                .ToListAsync();

            var response = PaginatedResponse<TEntity>.Create(data, total, paging.Page, paging.Limit);
            return Ok(ApiResponse<PaginatedResponse<TEntity>>.Ok(response));
        }


        [HttpPost]
        public async Task<ActionResult<TEntity>> Create([FromBody] TEntity entity)
        {
            var created = await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TEntity entity)
        {
            if (id != entity.Id) return BadRequest();
            await _repository.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return NotFound();

            await _repository.DeleteAsync(entity);
            return NoContent();
        }
    }
}
