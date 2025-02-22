using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace HealthQ_API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected async Task<ActionResult> ExecuteSafely(Func<Task<ActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, 
                JsonSerializer.Serialize(new { message = "Operation was canceled" }));
        }
        catch (NullReferenceException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, 
                JsonSerializer.Serialize(new { message = e.Message }));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, 
                JsonSerializer.Serialize(new { message = e.Message }));
        }
    }
}