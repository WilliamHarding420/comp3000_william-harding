using Microsoft.AspNetCore.Mvc;

namespace SecurityWebSite.Controllers
{

    [Controller]
    public class RTSPAuth : Controller
    {

        public struct AuthDetails
        {
            public string User { get; set; }
            public string Password { get; set; }
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

            Response.StatusCode = 200;
            return Task.CompletedTask;

        }

    }
}
