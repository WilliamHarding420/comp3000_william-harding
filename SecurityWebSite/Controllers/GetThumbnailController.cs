using Microsoft.AspNetCore.Mvc;
using SecurityWebSite.DatabaseModels;

namespace SecurityWebSite.Controllers
{

    [ApiController]
    public class GetThumbnailController : Controller
    {

        [HttpGet]
        [Route("/thumbnails/{Thumbnail}")]
        public async Task<ActionResult> GetThumbnail([FromRoute] string Thumbnail)
        {

            return base.PhysicalFile($"/app/Thumbnails/{Thumbnail}", "image/png");

        }

    }
}
