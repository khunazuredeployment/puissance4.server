using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Puissance4.API.DTO;
using Puissance4.Business.DTO;
using Puissance4.Business.Services;
using System.Security.Authentication;

namespace Puissance4.API.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("api/login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            try
            {
                return Ok(await _authenticationService.LoginAsync(dto));
            }
            catch(AuthenticationException)
            {
                return BadRequest("Bad Request");
            }
        }

        [HttpPost("api/register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            try
            {
                await _authenticationService.RegisterAsync(dto);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ValidationErrorDTO(ex));
            }
        }

        [HttpGet]
        public async Task<int> Get()
        {
            return await Task.FromResult(42);
        }
    }
}
