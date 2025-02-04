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
    private readonly IMapper _mapper;
    private readonly AuthService _authService;

    public UserController(
        UserService userService,
        IMapper mapper,
        AuthService authService)
    {
        _userService = userService;
        _mapper = mapper;
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

            var user = await _userService.GetUserByEmailAsync(email, ct);
            if (user == null)
                return NotFound();

            var userDto = _mapper.Map<UserDTO>(user);

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
            var user = await _userService.GetUserByEmailAsync(email, ct);
            if (user == null) return NotFound();

            var userDto = _mapper.Map<UserDTO>(user);
            
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
            var userModel = await _userService.GetUserByEmailAsync(user.Email, ct);
            if(userModel== null)
                return NotFound();
            
            if(!_passwordService.VerifyPasswordAsync(userModel, user.Password!, ct))
                return Unauthorized();
            
            _authService.
            
            var responseDto = _mapper.Map<UserDTO>(userModel);
            
            return Ok(responseDto);
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
            var user = await _userService.GetUserByEmailAsync(email, ct);
            if (user == null) return NotFound();

            await _userService.DeleteUserAsync(email, ct);
            return NoContent();
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "{\"message\":\"Operation was canceled\"}");
        }
    }
}