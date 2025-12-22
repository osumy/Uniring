using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Uniring.Application.Interfaces;
using Uniring.Application.Utils;
using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Auth;
using Uniring.Domain.Entities.IdentityEntities;

namespace Uniring.Application.Services
{

    /// <summary>
    /// IdentityService implements registration and login flows:
    /// - Register: create user with phone number and username; sets RegistrationDateTimeUtc
    /// - Login: accepts phone as Identifier, checks password, issues JWT for API clients
    /// - SetLastPurchaseAsync: update LastPurchaseAtUtc
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public IdentityService(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager,
                               RoleManager<ApplicationRole> roleManager,
                               IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // Register: use DisplayName -> UserName, require phone
        public async Task<Result<LoginResponse>> RegisterUserAsync(RegisterRequest request)
        {
            // TODO: Validation

            // normalize phone
            var normalizedPhone = PhoneNumberNormalizer.ToE164(request.PhoneNumber);

            if (string.IsNullOrWhiteSpace(normalizedPhone))
                return Result<LoginResponse>.Error("Signup Failed: Wrong PhoneNumber.");

            // check duplicate phone
            var existing = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == normalizedPhone);
            if (existing != null)
                return Result<LoginResponse>.Error("Signup Failed: Already Registered.");

            var user = new ApplicationUser
            {
                UserName = request.DisplayName,     // display name only
                PhoneNumber = normalizedPhone,
                RegistrationDateTimeUtc = DateTime.UtcNow,
                Email = null
            };

            var res = await _userManager.CreateAsync(user, request.Password);
            if (!res.Succeeded)
                return Result<LoginResponse>.Error("Signup Failed.");

            await _userManager.AddToRoleAsync(user, "guest");

            var user2 = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == normalizedPhone);
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "guest";
            return Result<LoginResponse>.Success(new LoginResponse
            { Id = user.Id.ToString(), Role = userRole, PhoneNumber = user.PhoneNumber }
            );
        }

        public Task<Result<LoginResponse>> RegisterAdminAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        // Login: accept phone only
        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var normalizedPhone = PhoneNumberNormalizer.ToE164(request.PhoneNumber);


            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == normalizedPhone);
            if (user == null) return Result<LoginResponse>.Error("Login Failed: User Not Found.");

            var pwdCheck = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!pwdCheck.Succeeded)
                return Result<LoginResponse>.Error("Login Failed: Wrong Password.");

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "guest";
            return Result<LoginResponse>.Success(new LoginResponse
            { Id = user.Id.ToString(), Role = userRole, PhoneNumber = user.PhoneNumber }
            );
        }

        public async Task<bool> ConfirmPhoneAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber!, token);
            return result.Succeeded;
        }

        public async Task SetLastPurchaseAsync(string userId, DateTime purchaseTime)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;
            user.LastPurchaseAtUtc = purchaseTime.ToUniversalTime();
            await _userManager.UpdateAsync(user);
        }

        //    // JWT creation helper
        //    private async Task<(string TokenString, DateTime ExpiresAt)> GenerateJwtTokenAsync(ApplicationUser user)
        //    {
        //        var jwtSection = _configuration.GetSection("Jwt");
        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //        var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpiresMinutes"] ?? "60"));

        //        var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id),
        //        // Put phone as claim so API consumers can see it
        //        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
        //        // Also include display name (username)
        //        new Claim("display_name", user.UserName ?? string.Empty)
        //    };

        //        // Add roles to claims
        //        var roles = await _userManager.GetRolesAsync(user);
        //        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        //        var token = new JwtSecurityToken(
        //            issuer: jwtSection["Issuer"],
        //            audience: jwtSection["Audience"],
        //            claims: claims,
        //            expires: expires,
        //            signingCredentials: creds);

        //        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        //        return (tokenString, expires);
        //    }
    }
}
