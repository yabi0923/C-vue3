using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        // 測試用的簡單回應
        [HttpGet("hello")]
        public IActionResult Hello()
        {
            return Ok(new { success = true, message = "你好! 這是測試!" });
        }

        // CREATE
        [HttpPost("create")]
        public IActionResult Create([FromQuery] string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                    return BadRequest(new { success = false, message = "訊息不得為空" });

                var msg = new TestMessage { Message = message };
                _context.TestMessages.Add(msg);
                _context.SaveChanges();

                return Ok(new { success = true, message = "資料已新增", data = msg });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "伺服器錯誤", error = ex.Message });
            }
        }

        // READ
        [HttpGet("read")]
        public IActionResult Read()
        {
            try
            {
                var list = _context.TestMessages.ToList();
                return Ok(new { success = true, data = list });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "讀取資料時發生錯誤", error = ex.Message });
            }
        }

        // UPDATE
        [HttpPut("update")]
        public IActionResult Update([FromQuery] int id, [FromQuery] string message)
        {
            try
            {
                var item = _context.TestMessages.Find(id);
                if (item == null)
                    return NotFound(new { success = false, message = "找不到資料" });

                if (string.IsNullOrWhiteSpace(message))
                    return BadRequest(new { success = false, message = "訊息不得為空" });

                item.Message = message;
                _context.SaveChanges();

                return Ok(new { success = true, message = "資料已更新", data = item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "更新時發生錯誤", error = ex.Message });
            }
        }

        // DELETE
        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int id)
        {
            try
            {
                var item = _context.TestMessages.Find(id);
                if (item == null)
                    return NotFound(new { success = false, message = "找不到資料" });

                _context.TestMessages.Remove(item);
                _context.SaveChanges();

                return Ok(new { success = true, message = "資料已刪除" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "刪除時發生錯誤", error = ex.Message });
            }
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            try
            {
                var data = new
                {
                    stats = new { totalResidents = 128, availableUnits = 10, maintenance = 2, monthlyIncome = 162000 },
                    announcements = new[]
                    {
                        new { id = 1, title = "新公告範例", content = "這是從資料庫撈到的資料", date = "2025-10-18" }
                    },
                    activities = new[]
                    {
                        new { name = "測試住戶", room = "A102", status = "搬出" }
                    }
                };
                return Ok(new { success = true, message = "成功取得儀表板資料", data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "取得儀表板資料時發生錯誤", error = ex.Message });
            }
        }
    }
}
