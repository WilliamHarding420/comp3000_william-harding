using System.Text.Json;

namespace SecurityWebSite
{
    public class JsonResponse<Type>
    {

        private Dictionary<string, Type> ResponseInformation;

        public JsonResponse() 
        { 
            ResponseInformation = new Dictionary<string, Type>();
        }

        public Task<string> BuildResponse()
        {
            return Task.FromResult(JsonSerializer.Serialize(ResponseInformation));
        }

        public void AddData(string key, Type value)
        {
            ResponseInformation.Add(key, value);
        }

        public static async Task<string> ErrorResponse(string error)
        {
            JsonResponse<string> response = new JsonResponse<string>();

            response.AddData("error", error);

            return await response.BuildResponse();
        }

        public static async Task<string> SingleResponse(string key, Type value)
        {
            JsonResponse<Type> response = new JsonResponse<Type>();

            response.AddData(key, value);

            return await response.BuildResponse();
        }

    }
}
