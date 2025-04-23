using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;
using System.Text.Json;

namespace SecurityWebSite.Controllers
{
    [ApiController]
    [Route("/cctv/get/{id:int?}")]
    public class GetCamerasController
    {

        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        public async Task<string> GetCameras([FromRoute] int? id)
        {

            Database db = new();

            if (id == 0)
                id = null;

            IEnumerable<Camera> cameras = db.Cameras.Where(camera => camera.LocationID == id);
            IEnumerable<Location> locations = db.Locations.Where(location => location.ParentID == id);

            JsonResponse<object> response = new JsonResponse<object>();
            response.AddData("cameras", cameras.ToArray());
            response.AddData("locations", locations.ToArray());

            return await response.BuildResponse();

        }

    }
}
