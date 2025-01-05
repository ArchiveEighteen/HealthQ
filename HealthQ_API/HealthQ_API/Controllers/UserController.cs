using HealthQ_API.Entities;
using HealthQ_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthQ_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken ct)
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

    [HttpGet("{id}")]
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

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] UserModel user, CancellationToken ct)
    {
        try
        {
            var createdUser = await _userService.CreateUserAsync(user, ct);
            return Ok(createdUser);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(string email, UserModel user, CancellationToken ct)
    {

        try
        {
            if (email != user.Email) return BadRequest();

            var updatedUser = await _userService.UpdateUserAsync(user, ct);
            return Ok(updatedUser);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "Internal Server Error");
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string email, CancellationToken ct)
    {
        var user = await _userService.GetUserByEmailAsync(email, ct);
        if (user == null) return NotFound();

        await _userService.DeleteUserAsync(email, ct);
        return NoContent();
    }
}