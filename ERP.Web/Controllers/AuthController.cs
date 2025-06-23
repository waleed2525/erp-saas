using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ERP.Infrastructure.Identity;
using ERP.Infrastructure.Security;
using System.Security.Claims;
using System;
using Microsoft.EntityFrameworkCore;
using ERP.Infrastructure.Persistence;

// اسم مستعار يفرض استخدام RefreshToken الصحيح
using RefreshTokenEntity = ERP.Infrastructure.Security.RefreshToken;

namespace ERP.Web.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly IJwtTokenService _jwt;
        private readonly ApplicationDbContext _db;

        public AuthController(
            UserManager<ApplicationUser> users,
            IJwtTokenService jwt,
            ApplicationDbContext db)
        {
            _users = users;
            _jwt = jwt;
            _db = db;
        }

        // **************  POST /auth/login  **************
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _users.FindByEmailAsync(dto.Email);
            if (user is null || !await _users.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid credentials");

            // Claims
            var claims = new[]
            {
                new Claim("sub", user.Id),
                new Claim("email", user.Email!),
                new Claim("tenant", user.TenantId.ToString())
            };

            // Generate tokens
            var token = _jwt.GenerateToken(claims);
            var rToken = _jwt.GenerateRefreshToken();

            // Save refresh-token
            _db.RefreshTokens.Add(new RefreshTokenEntity
            {
                Id       = Guid.NewGuid(),
                TenantId = user.TenantId,
                UserId   = Guid.Parse(user.Id),
                Token    = rToken,
                Expires  = DateTime.UtcNow.AddDays(7)
            });
            await _db.SaveChangesAsync();

            return Ok(new { token, refreshToken = rToken });
        }

        // **************  POST /auth/refresh  **************
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto dto)
        {
            var stored = await _db.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == dto.Token && !r.Revoked);

            if (stored is null || stored.Expires < DateTime.UtcNow)
                return Unauthorized();

            stored.Revoked = true;

            var user = await _users.FindByIdAsync(stored.UserId.ToString());

            var claims = new[]
            {
                new Claim("sub", user!.Id),
                new Claim("email", user.Email!),
                new Claim("tenant", user.TenantId.ToString())
            };

            var token = _jwt.GenerateToken(claims);
            var rToken = _jwt.GenerateRefreshToken();

            _db.RefreshTokens.Add(new RefreshTokenEntity
            {
                Id       = Guid.NewGuid(),
                TenantId = user.TenantId,
                UserId   = stored.UserId,
                Token    = rToken,
                Expires  = DateTime.UtcNow.AddDays(7)
            });
            await _db.SaveChangesAsync();

            return Ok(new { token, refreshToken = rToken });
        }

        /************  DTOs  ************/
        public record LoginDto(string Email, string Password);
        public record RefreshDto(string Token);
    }
}
