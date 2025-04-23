using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{

    [Route("/cctv/remove/{id:int}")]
    public class DeleteCameraController : Controller
    {

        [HttpPost]
        [Authorize]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task RemoveCamera([FromRoute] int id)
        {

            Database db = new();

            Camera? camera = db.Cameras.FirstOrDefault(cam => cam.CameraID == id);

            if (camera == null)
            {
                Response.StatusCode = 400;
                return;
            }

            db.Cameras.Remove(camera);
            await db.SaveChangesAsync();

            return;

        }

    }
}
