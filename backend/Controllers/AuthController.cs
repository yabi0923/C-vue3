using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest(new { success = false, message = "請提供登入資訊" });

                if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                    return BadRequest(new { success = false, message = "帳號與密碼不得為空" });

                // 假資料驗證
                if (request.Username == "admin" && request.Password == "1234")
                {
                    return Ok(new
                    {
                        success = true,
                        message = "登入成功",
                        token = "fake-jwt-token",
                        username = request.Username
                    });
                }

                return Unauthorized(new { success = false, message = "帳號或密碼錯誤" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "伺服器錯誤", error = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
