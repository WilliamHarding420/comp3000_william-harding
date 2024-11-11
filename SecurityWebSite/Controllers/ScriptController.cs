using Microsoft.AspNetCore.Mvc;

namespace SecurityWebSite.Controllers
{
    public class ScriptController : Controller
    {

        public static string ScriptFolder = @"/app/Scripts/";

        [HttpGet]
        [Route("/scripts/{Script}")]
        public async Task<ActionResult> GetScript([FromRoute] string Script)
        {

            return Content(await ReadScriptContent(Script));

        }

        private Task<string> ReadScriptContent(string Script)
        {

            Response.ContentType = "text/javascript";
            return Task.FromResult(System.IO.File.ReadAllText($"{ScriptFolder}{Script}"));

        }

    }
}
