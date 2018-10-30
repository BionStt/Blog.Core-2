using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Core.AuthHelper.OverWrite
{
    public class JwtTokenAuth
    {
        private readonly RequestDelegate _next;

        public JwtTokenAuth(RequestDelegate next)
        {
            this._next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(context);
            }

            var tokenHeader = context.Request.Headers["Authorization"].ToString();

            var tm = JwtHelper.SerializeJWT(tokenHeader);

            var claimList = new List<Claim>();
            var claim = new Claim(ClaimTypes.Role, tm.Role);
            claimList.Add(claim);

            var identity = new ClaimsIdentity(claimList);
            var principal = new ClaimsPrincipal(identity);

            context.User = principal;

            return _next(context);
        }
    }
}
