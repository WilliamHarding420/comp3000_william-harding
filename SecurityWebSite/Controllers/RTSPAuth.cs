using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{

    [ApiController]
    public class RTSPAuth : Controller
    {

        public struct AuthDetails
        {
            public string user { get; set; }
            public string password { get; set; }
            public string? IP { get; set; }
            public string? Action { get; set; }
            public string? Path { get; set; }
            public string? Protocol { get; set; }
            public string? ID { get; set; }
            public string? Query { get; set; }

        }

        [HttpPost]
        [Route("/auth/rtsp")]
        public Task RTSPAuthEndpoint([FromBody] AuthDetails details)
        {

            Database db = new();

            User? user = db.Users.Where(dbUser => dbUser.Username == details.user).FirstOrDefault();

            if (user == null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }

            string hashedPassword = SecurityUtils.HashPassword(details.password, user.Salt);

            if (hashedPassword != user.PasswordHash || !user.CanStream)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }

            Response.StatusCode = StatusCodes.Status200OK;
            return Task.CompletedTask;

        }

    }
}
