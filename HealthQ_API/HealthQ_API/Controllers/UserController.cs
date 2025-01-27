using HealthQ_API.DTOs;
using HealthQ_API.Entities;
using HealthQ_API.Security;
using HealthQ_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Controllers;

[Authorize]
[Route("[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult> Get(CancellationToken ct)
    {
        try
        {
            var users = await _userService.GetAllUsersAsync(ct);
            return Ok(users);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{email}")]
    public async Task<ActionResult> GetById(string email, CancellationToken ct)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email, ct);
            if (user == null) return NotFound();
            return Ok(user);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }    
    
    [AllowAnonymous]
    [HttpGet]
    public Task<ActionResult> IsAuthenticated()
    {
        if(User.Identity is { IsAuthenticated: true })
        {
            return Task.FromResult<ActionResult>(Ok(new { isAuthenticated = true }));
        }

        return Task.FromResult<ActionResult>(Unauthorized(new {isAuthenticated = false}));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Register([FromBody] UserDTO user, CancellationToken ct)
    {
        try
        {
            var createdUser = await _userService.CreateUserAsync(user, ct);
            
            var accessToken = JwtUtility.GenerateToken(createdUser.Email);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript from accessing it (Mitigates XSS)
                Secure = true,   // TODO: set to 'true' after release
                SameSite = SameSiteMode.Lax, // Helps prevent CSRF attacks + allows cross-origin requests
                Path = "/",
                Expires = DateTime.UtcNow.AddHours(1), // Token expiry
            };
            HttpContext.Response.Cookies.Append("auth_token", accessToken, cookieOptions);
            
            return Ok(createdUser);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
        catch (InvalidCastException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "{\"message\":\"Internal Server Error\"}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }

    [AllowAnonymous]
    [HttpPut]
    public async Task<ActionResult> Login(UserDTO user, CancellationToken ct)
    {
        try
        {
            var updatedUser = await _userService.VerifyUserAsync(user, ct);
            
            var accessToken = JwtUtility.GenerateToken(updatedUser.Email);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript from accessing it (Mitigates XSS)
                Secure = true,   // TODO: set to 'true' after release
                SameSite = SameSiteMode.Lax, // Helps prevent CSRF attacks + allows cross-origin requests
                Path = "/",
                Expires = DateTime.UtcNow.AddHours(1), // Token expiry
            };
            HttpContext.Response.Cookies.Append("auth_token", accessToken, cookieOptions);
            
            return Ok(updatedUser);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }

    [HttpDelete("{email}")]
    public async Task<ActionResult> Delete(string email, CancellationToken ct)
    {
        var user = await _userService.GetUserByEmailAsync(email, ct);
        if (user == null) return NotFound();

        await _userService.DeleteUserAsync(email, ct);
        return NoContent();
    }
}