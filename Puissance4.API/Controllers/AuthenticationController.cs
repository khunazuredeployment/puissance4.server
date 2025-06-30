using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Puissance4.API.DTO;
using Puissance4.Business.DTO;
using Puissance4.Business.Services;
using System.Security.Authentication;

namespace Puissance4.API.Controllers
{
    [ApiController]
    public class AuthenticationController(AuthenticationService authenticationService, ILogger<AuthenticationController> logger) : ControllerBase
    {
        private readonly AuthenticationService _authenticationService = authenticationService;
        private readonly ILogger<AuthenticationController> _logger = logger;

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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
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
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet("api/test")]
        public async Task<int> Get()
        {
            return await Task.FromResult(42);
        }
    }
}
