using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        // 測試用的簡單回應
        [HttpGet("hello")]
        public IActionResult Hello()
        {
            return Ok(new { success = true, message = "你好! 這是測試!" });
        }
    }
}
