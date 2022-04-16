using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                return StatusCode(500, ex);
            }
        }
    }
}