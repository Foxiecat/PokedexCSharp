using Microsoft.AspNetCore.Mvc;
using src.Features.User.DTOs;

namespace src.Features.User;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    [HttpPost]
    public Task<IActionResult> Register([FromQuery] string userName, 
                                        [FromQuery] string password)
    {
        return Task.FromResult<IActionResult>(Ok());
    }

    [HttpGet]
    public Task<ActionResult<string>> Login(string userName, string password)
    {
        throw new NotImplementedException();
    }
}