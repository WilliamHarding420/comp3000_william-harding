using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{
    [ApiController]
    [Route("/cctv/get/{id:int}")]
    public class GetCamerasController
    {

        [HttpGet]
        [Produces("application/json")]
        public async Task<string> GetCameras([FromRoute] int id)
        {

            Database db = new();

            IEnumerable<Camera> cameras = db.Cameras.Where(camera => camera.LocationID == id);

            return await JsonResponse<Camera[]>.SingleResponse("cameras", cameras.ToArray());

        }

    }
}
