using Application.Services;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AreasController : ControllerBase
    {
        private readonly IAreaService areaService;

        public AreasController(IAreaService areaService)
        {
            this.areaService = areaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AreaDTO>>> GetAll()
        {
            var areas = await areaService.GetAllAsync();
            return Ok(areas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AreaDTO>> Get(int id)
        {
            var area = await areaService.GetAsync(id);
            if (area == null)
                return NotFound();
            return Ok(area);
        }

        [HttpPost]
        public async Task<ActionResult<AreaDTO>> Add(AreaDTO dto)
        {
            try
            {
                var created = await areaService.AddAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AreaDTO dto)
        {
            dto.Id = id;
            try
            {
                var updated = await areaService.UpdateAsync(dto);
                if (!updated)
                    return NotFound();
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await areaService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
