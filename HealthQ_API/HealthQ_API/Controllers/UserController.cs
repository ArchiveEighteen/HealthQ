using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HealthQ_API.DTOs;
using HealthQ_API.Entities;
using HealthQ_API.Security;
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
    private readonly DoctorPatientService _doctorPatientService;

    public UserController(UserService userService, DoctorPatientService doctorPatientService)
    {
        _userService = userService;
        _doctorPatientService = doctorPatientService;
    }

    [HttpGet]
    public async Task<ActionResult> Get(CancellationToken ct)
    {
        try
        {
            var users = await _userService.GetAllUsersAsync(ct);
            
            var usersDto = users.Select(user => new UserDTO
                {
                    Email = user.Email,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    BirthDate = user.BirthDate.ToDateTime(new TimeOnly(0, 0)),
                    Gender = user.Gender.ToString(),
                    UserType = user.UserType.ToString(),
                    Password = ""
                })
                .ToList();
            return Ok(usersDto);
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
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var user = await _userService.GetUserByEmailAsync(email, ct);
            
            if (user == null)
                return NotFound();

            var userDto = new UserDTO
            {
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate.ToDateTime(new TimeOnly(0, 0)),
                Gender = user.Gender.ToString(),
                UserType = user.UserType.ToString(),
                Password = ""
            };

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

            var userDto = new UserDTO
            {
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate.ToDateTime(new TimeOnly(0, 0)),
                Gender = user.Gender.ToString(),
                UserType = user.UserType.ToString(),
                Password = ""
            };
            
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
            if (!Enum.TryParse<EGender>(user.Gender, out var gender))
                throw new InvalidCastException("Invalid gender value");

            if (!Enum.TryParse<EUserType>(user.UserType, out var role))
                throw new InvalidCastException("Invalid user type value");
        
            var userModel = new UserModel
            {
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = DateOnly.FromDateTime(user.BirthDate),
                Gender = gender,
                PhoneNumber = user.PhoneNumber,
                UserType = role,
                PasswordHash = "",
                PasswordSalt = ""

            };
            var createdUser = await _userService.CreateUserAsync(userModel, user.Password!, ct);
            
            var accessToken = JwtUtility.GenerateToken(createdUser.Email);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Expires = DateTime.UtcNow.AddHours(1)
            };
            HttpContext.Response.Cookies.Append("auth_token", accessToken, cookieOptions);
            
            var responseDto = new UserDTO
            {
                Email = createdUser.Email,
                Username = createdUser.Username,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                PhoneNumber = createdUser.PhoneNumber,
                BirthDate = createdUser.BirthDate.ToDateTime(new TimeOnly(0, 0)),
                Gender = createdUser.Gender.ToString(),
                UserType = createdUser.UserType.ToString(),
                Password = ""
            };
            
            return Ok(responseDto);
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
    [HttpPost]
    public async Task<ActionResult> Login(UserDTO user, CancellationToken ct)
    {
        try
        {
            var updatedUser = await _userService.VerifyUserAsync(user.Email, user.Password!, ct);
            
            var accessToken = JwtUtility.GenerateToken(updatedUser.Email);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Expires = DateTime.UtcNow.AddHours(1)
            };
            HttpContext.Response.Cookies.Append("auth_token", accessToken, cookieOptions);
            
            var responseDto = new UserDTO
            {
                Email = updatedUser.Email,
                Username = updatedUser.Username,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                PhoneNumber = updatedUser.PhoneNumber,
                BirthDate = updatedUser.BirthDate.ToDateTime(new TimeOnly(0, 0)),
                Gender = updatedUser.Gender.ToString(),
                UserType = updatedUser.UserType.ToString(),
                Password = ""
            };
            
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
            var userModel = await _userService.GetUserByEmailAsync(user.Email, ct);
            
            if(userModel == null) return NotFound();
            
            userModel.Username = user.Username;
            userModel.FirstName = user.FirstName;
            userModel.LastName = user.LastName;
            userModel.BirthDate = DateOnly.FromDateTime(user.BirthDate);
            userModel.Gender = Enum.Parse<EGender>(user.Gender);
            userModel.PhoneNumber = user.PhoneNumber;
            userModel.UserType = Enum.Parse<EUserType>(user.UserType);

            var updatedUserModel = await _userService.UpdateUserAsync(userModel, ct);

            var userDto = new UserDTO
            {
                Email = updatedUserModel.Email,
                Username = updatedUserModel.Username,
                FirstName = updatedUserModel.FirstName,
                LastName = updatedUserModel.LastName,
                PhoneNumber = updatedUserModel.PhoneNumber,
                BirthDate = updatedUserModel.BirthDate.ToDateTime(new TimeOnly(0, 0)),
                Gender = updatedUserModel.Gender.ToString(),
                UserType = updatedUserModel.UserType.ToString(),
                Password = ""
            };
            
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