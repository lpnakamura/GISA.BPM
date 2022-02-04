using GISA.BPM.HttpContext.Shared.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace GISA.BPM.HttpContext.Shared.Models
{
    public class ClaimContext : IClaimContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserName()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token is required");

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            return jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "name").Value;
        }
    }
}
