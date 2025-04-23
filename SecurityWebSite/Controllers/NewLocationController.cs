
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{

    [ApiController]
    [Route("/cctv/location/new")]
    public class NewLocationController : Controller
    {

        public struct LocationData
        {
            public string Name { get; set; }
            public int ParentID { get; set; }
        }

        [Authorize]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<string> CreateNewLocation([FromBody] LocationData data)
        {

            if (data.Name.Trim() == "")
            {
                Response.StatusCode = 400;
                return await JsonResponse<string>.ErrorResponse("Location name required.");
            }

            Database db = new();

            Location location = new Location()
            {
                LocationName = data.Name,
                ParentID = data.ParentID
            };

            await db.Locations.AddAsync(location);
            await db.SaveChangesAsync();

            return await JsonResponse<string>.SingleResponse("message", "Location successfully added.");

        }

    }
}
