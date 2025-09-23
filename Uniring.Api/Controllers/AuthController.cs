using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;

namespace Uniring.Api.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class AuthController : ControllerBase
    //{
    //    private readonly IIdentityService _identity;

    //    public AuthController(IIdentityService identity) => _identity = identity;

    //    [HttpPost("register")]
    //    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    //    {
    //        var (succeeded, errors) = await _identity.RegisterAsync(req, role: "User");
    //        if (!succeeded) return BadRequest(new { errors });
    //        return Ok();
    //    }

    //    [HttpPost("login")]
    //    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    //    {
    //        var res = await _identity.LoginAsync(req);
    //        if (!res.Success) return Unauthorized(new { res.Errors });
    //        return Ok(new { token = res.Token, expires = res.ExpiresAt });
    //    }
    //}

}
