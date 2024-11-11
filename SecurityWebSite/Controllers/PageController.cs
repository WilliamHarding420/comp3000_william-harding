using Microsoft.AspNetCore.Mvc;

namespace SecurityWebSite.Controllers
{
    public class PageController : Controller
    {

        public static string PagesFolder = @"/app/Pages/";

        [HttpGet]
        [Route("/")]
        public async Task<ActionResult> GetIndexPage()
        {

            return Content(await ReadPageContent("index.html"));

        }

        [HttpGet]
        [Route("/pages/{Page}")]
        public async Task<ActionResult> GetPage([FromRoute]string Page)
        {

            return Content(await ReadPageContent(Page));

        }

        private Task<string> ReadPageContent(string Page)
        {

            Response.ContentType = "text/html";
            return Task.FromResult(System.IO.File.ReadAllText($"{PagesFolder}{Page}"));

        }

    }
}
