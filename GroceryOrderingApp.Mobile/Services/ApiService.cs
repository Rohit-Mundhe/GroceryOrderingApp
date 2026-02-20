using System.Net.Http.Json;

namespace GroceryOrderingApp.Mobile.Services
{
    public interface IApiService
    {
        Task<T?> GetAsync<T>(string endpoint);
        Task<T?> PostAsync<T>(string endpoint, object? data = null);
        Task<T?> PutAsync<T>(string endpoint, object? data = null);
        void SetAuthToken(string token);
        void ClearAuthToken();
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private string? _authToken;

        public ApiService()
        {
            var handler = new HttpClientHandler();
#if DEBUG
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
#endif
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://your-railway-url.railway.app") // Replace with your actual Railway API URL
            };
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                if (!string.IsNullOrEmpty(_authToken))
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<T>();

                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API Error: {ex.Message}");
                return default;
            }
        }

        public async Task<T?> PostAsync<T>(string endpoint, object? data = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                if (!string.IsNullOrEmpty(_authToken))
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                if (data != null)
                    request.Content = JsonContent.Create(data);

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<T>();

                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API Error: {ex.Message}");
                return default;
            }
        }

        public async Task<T?> PutAsync<T>(string endpoint, object? data = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
                if (!string.IsNullOrEmpty(_authToken))
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                if (data != null)
                    request.Content = JsonContent.Create(data);

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<T>();

                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API Error: {ex.Message}");
                return default;
            }
        }

        public void SetAuthToken(string token)
        {
            _authToken = token;
        }

        public void ClearAuthToken()
        {
            _authToken = null;
        }
    }
}
