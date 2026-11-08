using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RaceDayMVC.Services
{
    public class ApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("RaceDayApi");

            // If we have a token in session later, attach it here
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        // Generic GET
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var client = CreateClient();
            var response = await client.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Generic POST
        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PostAsync(endpoint, content);
        }

        // Generic PUT
        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PutAsync(endpoint, content);
        }

        // Generic DELETE
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            var client = CreateClient();
            return await client.DeleteAsync(endpoint);
        }
    }
}
