using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.AuthHelper;
using Blog.Core.AuthHelper.OverWrite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        [Route("token")]
        [HttpGet]
        public JsonResult GetJwtToken(int id = 1, string role = "Admin")
        {
            var jwtToken = JwtHelper.IssueJWT(new TokenModelJWT { Uid = id, Role = role });
            return Json(jwtToken);
        }
    }
}