using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IMemoryCache _database;

        public UserController(ILogger<UserController> logger, IMemoryCache database)
        {
            _logger = logger;
            _database = database;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>All users</returns>
        /// <response code="200">Returns users list</response>
        /// <response code="204">Returns empty users list</response>
        /// <response code="500">Returns empty because something wrong happened in server side. Sorry</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                var users = new List<UserDto>();

                _logger.LogInformation("Requested executed with success: {@data}", users);

                if (!users.Any())
                {
                    return NoContent();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Requested throw unhandled error {@error}", ex.Message);

                return StatusCode(500, new[] { ex.Message });
            }
        }

        /// <summary>
        /// Get user with informed id
        /// </summary>
        /// <response code="200">Returns user requested</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Returns empty because something wrong happened in server side. Sorry</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var user = _database.Get<User>(id);

                _logger.LogInformation("Requested executed with success: {@data}", user);

                if (user is null)
                {
                    return Problem(detail: $"UserId {id} was not found in database", statusCode: 404);
                }

                return Ok(user.ToUserDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Requested throw unhandled error {@error}", ex.Message);

                return StatusCode(500, new[] { ex.Message });
            }
        }

        /// <summary>
        /// Add new user
        /// </summary>
        /// <response code="201">Created new user</response>
        /// <response code="400">User informed is not valid</response>
        /// <response code="500">Returns empty because something wrong happened in server side. Sorry</response>

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult Post([FromBody] UserDto input)
        {
            try
            {
                var user = input.ToUser();

                if (!user.IsValidUser())
                {
                    _logger.LogError("Invalid input {@data}", input);

                    return BadRequest(input);
                }

                _database.Set(user.Id, user);

                _logger.LogInformation("User created with success: {@data}", user);

                return Created(nameof(Get), user.ToUserDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Requested throw unhandled error {@error}", ex.Message);

                return StatusCode(500, new[] { ex.Message });
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <response code="200">Updated new user</response>
        /// <response code="400">User informed is not valid</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Returns empty because something wrong happened in server side. Sorry</response>

        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] UserDto input)
        {
            try
            {
                var user = _database.Get<User>(id);

                if (user is null)
                {
                    return Problem(detail: $"UserId {id} was not found in database", statusCode: 404);
                }

                _logger.LogInformation("UserDto received {@data}", input);

                user = new User(
                        user.Id,
                        string.IsNullOrEmpty(input.Name) ? user.Name : input.Name,
                        string.IsNullOrEmpty(input.Email) ? user.Email : input.Email,
                        user.IsEnabled
                      );

                if (!user.IsValidUser())
                {
                    _logger.LogError("Invalid input {@data}", input);

                    return BadRequest(input);
                }

                _database.Set(user.Id, user);

                _logger.LogInformation("User updated with success: {@data}", user);

                return Ok(user.ToUserDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Requested throw unhandled error {@error}", ex.Message);

                return StatusCode(500, new[] { ex.Message });
            }
        }

        /// <summary>
        /// Update user status
        /// </summary>
        /// <response code="200">Updated new user</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Returns empty because something wrong happened in server side. Sorry</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}/status/{enable}")]
        public IActionResult Put(Guid id, bool enable)
        {
            try
            {
                var user = _database.Get<User>(id);

                if (user is null)
                {
                    return Problem(detail: $"UserId {id} was not found in database", statusCode: 404);
                }

                user.UpdateUserStatus(enable);

                _database.Set(user.Id, user);

                _logger.LogInformation("User updated with success: {@data}", user);

                return Ok(user.ToUserDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Requested throw unhandled error {@error}", ex.Message);

                return StatusCode(500, new[] { ex.Message });
            }
        }
    }
}