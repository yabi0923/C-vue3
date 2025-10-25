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

        // GET: api/residents
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var residents = await _context.Residents.ToListAsync();
                // 回傳原本陣列給前端方便使用
                return Ok(residents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "伺服器錯誤", error = ex.Message });
            }
        }

        // GET: api/residents/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var resident = await _context.Residents.FindAsync(id);
                if (resident == null)
                    return NotFound(new { success = false, message = "找不到該住戶" });

                return Ok(new { success = true, data = resident });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "伺服器錯誤", error = ex.Message });
            }
        }

        // POST: api/residents
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Resident resident)
        {
            if (resident == null || string.IsNullOrWhiteSpace(resident.Name) || string.IsNullOrWhiteSpace(resident.RoomNumber))
            {
                return BadRequest(new { success = false, message = "姓名與房號不可為空" });
            }

            resident.ManagementFee = resident.ManagementFee < 0 ? 0 : resident.ManagementFee;

            try
            {
                await _context.Residents.AddAsync(resident);
                await _context.SaveChangesAsync();
                // 直接回傳新增物件，前端可直接拿到 ID
                return Ok(new { success = true, message = "住戶已新增成功", data = resident });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { success = false, message = "資料庫更新錯誤", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "伺服器錯誤", error = ex.Message });
            }
        }

        // PUT: api/residents/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Resident resident)
        {
            if (resident == null || string.IsNullOrWhiteSpace(resident.Name) || string.IsNullOrWhiteSpace(resident.RoomNumber))
            {
                return BadRequest(new { success = false, message = "姓名與房號不可為空" });
            }

            resident.ManagementFee = resident.ManagementFee < 0 ? 0 : resident.ManagementFee;

            try
            {
                var existing = await _context.Residents.FindAsync(id);
                if (existing == null)
                    return NotFound(new { success = false, message = "找不到該住戶" });

                existing.Name = resident.Name;
                existing.RoomNumber = resident.RoomNumber;
                existing.ManagementFee = resident.ManagementFee;

                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "已更新住戶資料", data = existing });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { success = false, message = "資料庫更新錯誤", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "伺服器錯誤", error = ex.Message });
            }
        }

        // DELETE: api/residents/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resident = await _context.Residents.FindAsync(id);
                if (resident == null)
                    return NotFound(new { success = false, message = "找不到該住戶" });

                _context.Residents.Remove(resident);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "已刪除該住戶", data = resident });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { success = false, message = "資料庫更新錯誤", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "伺服器錯誤", error = ex.Message });
            }
        }
    }
}
