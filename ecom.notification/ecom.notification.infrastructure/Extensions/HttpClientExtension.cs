using System.Net.Http.Headers;
using System.Text.Json;

namespace ecom.notification.infrastructure.Extensions
{
    public static class HttpClientExtension
    {
        public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PutAsync(url, content);
        }


        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            try 
            {
                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

                var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Console.Write($"from notification api ==========> {dataAsString}");

                return JsonSerializer.Deserialize<T>(dataAsString.FirstOrDefault(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex) {
                throw new ApplicationException(ex.Message);                
            
            }
            
        }
    }
}
