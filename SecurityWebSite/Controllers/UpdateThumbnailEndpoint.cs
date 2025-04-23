using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{

    [ApiController]
    public class UpdateThumbnailEndpoint : Controller
    {

        [Authorize]
        [HttpPost]
        [Route("/cctv/thumbnail/update/{id:int}")]
        public async Task UpdateThumbnail([FromRoute] int id)
        {

            Database db = new();

            Camera? cam = db.Cameras.FirstOrDefault(dbCam => dbCam.CameraID == id);

            if (cam == null)
            {
                Response.StatusCode = 400;
                return;
            }

            await StreamUtils.ReadThumbnailFromStream(cam);

            return;

        }

    }
}
