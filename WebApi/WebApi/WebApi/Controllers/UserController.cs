using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /User
        ///     {
        ///        "id": 1,
        ///        "name": "UserName"
        ///     }
        ///
        /// </remarks>
        /// <returns>All users</returns>
        /// <response code="200">Returns user list</response>
        /// <response code="500">Returns empty because something wrong happened in server side. Sorry</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                var data = new string[] { "value1", "value2" };

                _logger.LogInformation("Requested executed with success: {@data}", data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Requested throw unhandled error {@error}", ex.Message);

                return StatusCode(500, new[] { ex.Message });
            }
        }
    }
}