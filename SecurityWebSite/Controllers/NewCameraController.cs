using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{
    [Route("/cctv/new")]
    [ApiController]
    public class NewCameraController : Controller
    {

        public struct CameraData
        {
            public string Name { get; set; }
            public string IP { get; set; }
            public string Port { get; set; }
            public string CamUrl { get; set; }
            public string PublishURL { get; set; }
            public int? LocationID { get; set; }
        }

        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        public async Task<string> AddCamera([FromBody] CameraData cameraData)
        {

            Camera camera = new Camera()
            {
                Name = cameraData.Name,
                IP = cameraData.IP,
                Port = cameraData.Port,
                StreamURL = cameraData.CamUrl,
                PublishURL = cameraData.PublishURL,
                LocationID = cameraData.LocationID
            };
            Database db = new();

            await db.Cameras.AddAsync(camera);
            await db.SaveChangesAsync();

            StreamUtils.PublishCamera(camera);

            return await JsonResponse<string>.SingleResponse("success", "Camera was successfully added.");

        }

    }
}
