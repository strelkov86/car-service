using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SibintekTask.API.Contracts;
using SibintekTask.Application.Auth;
using System.Threading.Tasks;

namespace SibintekTask.API.Controllers
{
    [ApiController]
    [Route("auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.Register(request.Name, request.Surname, request.Itn, request.Password);
            if (result is null) return BadRequest($"Пользователь с ИНН {request.Itn} уже существует");
            return Ok(new { User = result });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.Login(request.Itn, request.Password);
            if (result is null) return BadRequest("Проверьте правильность ИНН и/или пароля");
            HttpContext.Response.Cookies.Append("sibintek-cookie", result);
            return Ok(result);
        }

        [HttpPost("customer/register")]
        public async Task<ActionResult> RegisterExistingCustomer([FromBody] RegisterExistingCustomerRequest request)
        {
            var result = await _authService.RegisterForExistingCustomer(request.Itn, request.Password);
            if (result is null) return BadRequest("Не удалось найти клиента с указанным ИНН");
            return Ok(new { User = result });
        }
    }
}
