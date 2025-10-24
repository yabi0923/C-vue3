using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var totalResidents = await _context.Residents.CountAsync();
                var totalAnnouncements = await _context.Announcements.CountAsync();
                var pendingMaintenance = await _context.MaintenanceRequests
                    .CountAsync(m => m.Status == "Pending");
                var totalManagementFee = await _context.Residents
                    .SumAsync(r => (int?)r.ManagementFee ?? 0); // null-safe

                return Ok(new
                {
                    success = true,
                    message = "成功取得儀表板摘要資料",
                    data = new
                    {
                        TotalResidents = totalResidents,
                        TotalAnnouncements = totalAnnouncements,
                        PendingMaintenance = pendingMaintenance,
                        TotalManagementFee = totalManagementFee
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "伺服器錯誤",
                    error = ex.Message
                });
            }
        }
    }
}
