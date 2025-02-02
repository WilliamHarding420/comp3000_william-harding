using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{

    [ApiController]
    public class UpdateThumbnailEndpoint : Controller
    {

        [HttpPost]
        [Route("/cctv/thumbnail/update/{camera}")]
        public async Task UpdateThumbnail([FromRoute] string camera)
        {

            Database db = new();

            Camera? cam = db.Cameras.FirstOrDefault(dbCam => dbCam.Name == camera);

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
