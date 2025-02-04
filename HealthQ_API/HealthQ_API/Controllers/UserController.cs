using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using HealthQ_API.DTOs;
using HealthQ_API.Entities;
using HealthQ_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthQ_API.Controllers;

[Authorize]
[Route("[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthService _authService;

    public UserController(
        UserService userService,
        AuthService authService)
    {
        _userService = userService;
        _authService = authService;
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
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetUser(CancellationToken ct)
    {
        try
        {
            var email = (User.Identity as ClaimsIdentity)!.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized();
            
            var userDto = await _userService.GetUserByEmailAsync(email, ct);

            return Ok(userDto);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }

    [HttpGet("{email}")]
    public async Task<ActionResult> GetById(string email, CancellationToken ct)
    {
        try
        {
            var userDto = await _userService.GetUserByEmailAsync(email, ct);
            
            return Ok(userDto);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
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
            
            var accessToken = _authService.GenerateToken(createdUser.Email);
            var cookieOptions = _authService.GetCookieOptions(createdUser.Email);
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
            return Conflict($"{{\"message\":\"{e.Message}\"}}");
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Login(UserDTO user, CancellationToken ct)
    {
        try
        {
            var verifiedUser = await _userService.VerifyUserAsync(user, ct);
            
            var accessToken = _authService.GenerateToken(verifiedUser.Email);
            var cookieOptions = _authService.GetCookieOptions(verifiedUser.Email);
            HttpContext.Response.Cookies.Append("auth_token", accessToken, cookieOptions);
            
            return Ok(verifiedUser);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser([FromBody] UserDTO user, CancellationToken ct)
    {
        try
        {
            var updatedUser = await _userService.UpdateUserAsync(user, ct);
            
            return Ok(updatedUser);
        }
        catch (OperationCanceledException)
        {
            
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, $"{{\"message\":\"{e.Message}\"}}");
        }
        
    }

    [HttpDelete]
    public Task<ActionResult> Logout()
    {
        
        HttpContext.Response.Cookies.Delete("auth_token");
        
        return Task.FromResult<ActionResult>(Ok());
    }
    

    [HttpDelete("{email}")]
    public async Task<ActionResult> Delete(string email, CancellationToken ct)
    {
        try
        {
            await _userService.DeleteUserAsync(email, ct);
            return NoContent();
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
    }
}