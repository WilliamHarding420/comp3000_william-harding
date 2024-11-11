using Microsoft.AspNetCore.Mvc;

namespace SecurityWebSite.Controllers
{
    public class StyleSheetController : Controller
    {

        public static string StylesFolder = @"/app/Styles/";

        [HttpGet]
        [Route("/style/{Style}")]
        public async Task<ActionResult> GetStyle([FromRoute] string Style)
        {

            return Content(await ReadStyleContent(Style));

        }

        private Task<string> ReadStyleContent(string Style)
        {

            Response.ContentType = "text/css";
            return Task.FromResult(System.IO.File.ReadAllText($"{StylesFolder}{Style}"));

        }

    }
}
