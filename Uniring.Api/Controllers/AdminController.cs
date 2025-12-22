using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;
using Uniring.Contracts.Auth;

namespace Uniring.Api.Controllers
{
    public class AdminController : ApiControllerBase
    {
        private readonly IIdentityService _identity;

        public AdminController(IIdentityService identity)
        {
            _identity = identity;
        }

        [HttpPost("register-user")]
        public async Task<ActionResult> Register(RegisterRequest req)
        {
            var res = await _identity.RegisterCustomerAsync(req);
            if (!res.IsSuccess) return BadRequest(res.ErrorMessage);

            return Ok();
        }

    }
}
