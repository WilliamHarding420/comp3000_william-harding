using Microsoft.AspNetCore.Mvc;

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

            if (data.Username == "test" &&  data.Password == "password") 
            {
                // WILL RETURN AUTH TOKEN ONCE DATABASE AND JWT SETUP
                return "Correct Credentials.";
            }

            Response.StatusCode = StatusCodes.Status401Unauthorized;
            return "Incorrect Credentials.";

        }

    }
}
