using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{
    public class LoginController : Controller
    {

        public struct LoginData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        [Route("/auth/login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<string> LoginEndpoint([FromBody] LoginData data)
        {

            Database db = new();

            User? user = db.Users.FirstOrDefault(user => user.Username == data.Username);

            if (user == null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return await JsonResponse<string>.ErrorResponse("Invalid Credentials.");
            }

            string inputPasswordHash = SecurityUtils.HashPassword(data.Password, user.Salt);

            if (inputPasswordHash == user.PasswordHash)
            {
                return await JsonResponse<string>.SingleResponse("token", await JwtUtils.AuthorizeUser(user.UserID));
            }

            Response.StatusCode = StatusCodes.Status401Unauthorized;
            return await JsonResponse<string>.ErrorResponse("Invalid Credentials");

        }

    }
}
