using Domain.Models.Auth;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IConfiguration _configuration;

        public AuthController(ILogger<UserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Generate authorization
        /// </summary>
        /// <param name="dto"></param>        
        /// <response code="200">Returns successfully</response>
        /// <response code="401">User unauthorized</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Returns empty because something wrong happened in server side. Sorry</response>
        [HttpPost]
        [ProducesResponseType(typeof(AuthOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] AuthInput dto)
        {
            try
            {
                if (dto is null)
                {
                    return BadRequest(new[] { "Invalid input informations {@input}" });
                }

                _logger.LogInformation("Generating user {@user} authentication", dto.Name);

                var adminId = _configuration.GetSection("ASPNETCore3.1-APIRest:AdminUserId").Value;

                if (string.IsNullOrEmpty(adminId))
                {
                    return NotFound(new[] { "AdminId is not add in settings" });
                }

                if (!dto.Secret.Equals(adminId))
                {
                    return Unauthorized(new[] { "User is not autorized" });
                }

                var token = TokenService.GenerateToken(dto
                    , _configuration.GetSection("ASPNETCore3.1-APIRest:JWT:Secret").Value);

                _logger.LogInformation("Token for user {@user} was generated with success", dto.Name);

                return Ok(new AuthOutput { Name = dto.Name, Secret = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new[] { ex.Message });
            }
        }
    }
}