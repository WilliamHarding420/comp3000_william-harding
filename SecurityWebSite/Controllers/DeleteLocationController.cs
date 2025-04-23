using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{

    [Route("/cctv/location/remove/{id:int}")]
    public class DeleteLocationController : Controller
    {

        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task DeleteLocation([FromRoute] int id)
        {

            Database db = new Database();

            Location? deleteLocation = db.Locations.FirstOrDefault(loc => loc.LocationID == id);

            if (deleteLocation == null)
            {
                Response.StatusCode = 400;
                return;
            }

            db.Locations.Remove(deleteLocation);
            await db.SaveChangesAsync();

            return;

        }

    }
}
