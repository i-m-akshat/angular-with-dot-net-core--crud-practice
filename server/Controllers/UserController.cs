using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("User")]
public class UserController : ControllerBase
{
    [HttpGet] // Add this line
    public async Task<IActionResult> Index()
    {
        return Ok("Hello, this is the Index action.");
    }
}