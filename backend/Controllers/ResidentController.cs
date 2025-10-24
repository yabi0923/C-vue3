using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Data;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResidentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResidentsController(AppDbContext context)
        {
            _context = context;
        }

        // READ 全部住戶
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var residents = await _context.Residents.ToListAsync();
                return Ok(residents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "伺服器錯誤", error = ex.Message });
            }
        }

        // CREATE 新增住戶
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Resident resident)
        {
            if (resident == null)
                return BadRequest(new { message = "request body is null" });

            try
            {
                await _context.Residents.AddAsync(resident);
                await _context.SaveChangesAsync();
                return Created("", new { message = "住戶已新增成功", data = resident });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "資料庫更新錯誤", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "伺服器錯誤", error = ex.Message });
            }
        }

        // UPDATE 更新住戶資料
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Resident resident)
        {
            if (resident == null)
                return BadRequest(new { message = "request body is null" });

            try
            {
                var existing = await _context.Residents.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "找不到該住戶" });

                existing.Name = resident.Name;
                existing.RoomNumber = resident.RoomNumber;
                existing.ManagementFee = resident.ManagementFee;

                await _context.SaveChangesAsync();
                return Ok(new { message = "已更新住戶資料", data = existing });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "資料庫更新錯誤", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "伺服器錯誤", error = ex.Message });
            }
        }

        // DELETE 刪除住戶
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resident = await _context.Residents.FindAsync(id);
                if (resident == null)
                    return NotFound(new { message = "找不到該住戶" });

                _context.Residents.Remove(resident);
                await _context.SaveChangesAsync();

                return Ok(new { message = "已刪除該住戶", data = resident });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "資料庫更新錯誤", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "伺服器錯誤", error = ex.Message });
            }
        }
    }
}
