using System.Text.Json;

namespace SecurityWebSite
{
    public class JsonResponse
    {

        private Dictionary<string, string> ResponseInformation;

        public JsonResponse() 
        { 
            ResponseInformation = new Dictionary<string, string>();
        }

        public Task<string> BuildResponse()
        {
            return Task.FromResult(JsonSerializer.Serialize(ResponseInformation));
        }

        public void AddData(string key, string value)
        {
            ResponseInformation.Add(key, value);
        }

        public static async Task<string> ErrorResponse(string error)
        {
            JsonResponse response = new JsonResponse();

            response.AddData("error", error);

            return await response.BuildResponse();
        }

        public static async Task<string> SingleResponse(string key, string value)
        {
            JsonResponse response = new JsonResponse();

            response.AddData(key, value);

            return await response.BuildResponse();
        }

    }
}
