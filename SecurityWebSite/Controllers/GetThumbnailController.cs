using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{

    [ApiController]
    public class GetThumbnailController : Controller
    {

        [HttpGet]
        [Route("/thumbnails/{thumbnail}")]
        public async Task<ActionResult> GetThumbnail([FromRoute] string thumbnail)
        {

            return base.PhysicalFile($"/app/Thumbnails/{thumbnail}", "image/png");

        }

    }
}
