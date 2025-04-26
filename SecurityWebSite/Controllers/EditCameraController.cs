using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;
using static SecurityWebSite.Controllers.NewCameraController;

namespace SecurityWebSite.Controllers
{

    [Route("/cctv/update/{id:int}")]
    [ApiController]
    public class EditCameraController : Controller
    {

        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        public async Task<string> AddCamera([FromRoute] int id, [FromBody] CameraData cameraData)
        {

            Database db = new();
            Camera? camera = db.Cameras.FirstOrDefault(cam => cam.CameraID == id);

            if (camera == null)
            {
                Response.StatusCode = 400;
                return await JsonResponse<string>.ErrorResponse("Invalid camera.");
            }

            camera.Name = cameraData.Name;
            camera.IP = cameraData.IP;
            camera.Port = cameraData.Port;
            camera.StreamURL = cameraData.CamUrl;
            camera.PublishURL = cameraData.PublishURL;

            db.Cameras.Update(camera);
            await db.SaveChangesAsync();

            await StreamUtils.StopCamera(id);

            StreamUtils.PublishCamera(camera);

            return await JsonResponse<string>.SingleResponse("success", "Camera was successfully added.");

        }

    }

}
