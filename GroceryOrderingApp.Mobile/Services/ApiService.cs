using System.Net.Http.Json;
using System.Text.Json;

namespace GroceryOrderingApp.Mobile.Services
{
    public interface IApiService
    {
        Task<ApiResponse<T>> GetAsync<T>(string endpoint);
        Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? data = null);
        Task<ApiResponse<T>> PutAsync<T>(string endpoint, object? data = null);
        Task<ApiResponse<bool>> DeleteAsync(string endpoint);
        void SetAuthToken(string token);
        void ClearAuthToken();
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public int? StatusCode { get; set; }
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private string? _authToken;
        private const int TimeoutSeconds = 30;

        public ApiService()
        {
            var handler = new HttpClientHandler();
#if DEBUG
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
#endif
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(AppConfig.ApiBaseUrl),
                Timeout = TimeSpan.FromSeconds(TimeoutSeconds)
            };
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                AddAuthHeader(request);

                Debug.WriteLine($"[API] GET {AppConfig.ApiBaseUrl}/{endpoint}");
                var response = await _httpClient.SendAsync(request);

                return await HandleResponse<T>(response, endpoint);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = "Network error. Please check your internet connection.",
                    Data = default
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = "Request timeout. Server is taking too long to respond.",
                    Data = default
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[API Error] GET {endpoint}: {ex.Message}");
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = default
                };
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? data = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                AddAuthHeader(request);

                if (data != null)
                {
                    var json = JsonSerializer.Serialize(data);
                    request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                }

                Debug.WriteLine($"[API] POST {AppConfig.ApiBaseUrl}/{endpoint}");
                var response = await _httpClient.SendAsync(request);

                return await HandleResponse<T>(response, endpoint);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = "Network error. Please check your internet connection.",
                    Data = default
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = "Request timeout. Server is taking too long to respond.",
                    Data = default
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[API Error] POST {endpoint}: {ex.Message}");
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = default
                };
            }
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object? data = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
                AddAuthHeader(request);

                if (data != null)
                {
                    var json = JsonSerializer.Serialize(data);
                    request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                }

                Debug.WriteLine($"[API] PUT {AppConfig.ApiBaseUrl}/{endpoint}");
                var response = await _httpClient.SendAsync(request);

                return await HandleResponse<T>(response, endpoint);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = "Network error. Please check your internet connection.",
                    Data = default
                };
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = "Request timeout. Server is taking too long to respond.",
                    Data = default
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[API Error] PUT {endpoint}: {ex.Message}");
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = default
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string endpoint)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
                AddAuthHeader(request);

                Debug.WriteLine($"[API] DELETE {AppConfig.ApiBaseUrl}/{endpoint}");
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool>
                    {
                        IsSuccess = true,
                        Data = true
                    };
                }

                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = GetErrorMessage(response.StatusCode),
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[API Error] DELETE {endpoint}: {ex.Message}");
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "An unexpected error occurred."
                };
            }
        }

        private async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response, string endpoint)
        {
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(content))
                    {
                        return new ApiResponse<T>
                        {
                            IsSuccess = true,
                            Data = default
                        };
                    }

                    var data = JsonSerializer.Deserialize<T>(content);
                    return new ApiResponse<T>
                    {
                        IsSuccess = true,
                        Data = data
                    };
                }

                // Handle specific error status codes
                var errorMessage = GetErrorMessage(response.StatusCode);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Token expired - will be handled by ViewModel to redirect to login
                    ClearAuthToken();
                }

                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = errorMessage,
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Response Parse Error] {endpoint}: {ex.Message}");
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to parse server response."
                };
            }
        }

        private string GetErrorMessage(System.Net.HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                System.Net.HttpStatusCode.BadRequest => "Invalid input. Please check your data.",
                System.Net.HttpStatusCode.Unauthorized => "Session expired. Please log in again.",
                System.Net.HttpStatusCode.Forbidden => "You don't have permission to perform this action.",
                System.Net.HttpStatusCode.NotFound => "The requested item was not found.",
                System.Net.HttpStatusCode.InternalServerError => "Server error. Please try again later.",
                System.Net.HttpStatusCode.ServiceUnavailable => "Service is temporarily unavailable.",
                _ => $"Error: {statusCode}"
            };
        }

        private void AddAuthHeader(HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(_authToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
            }
        }

        public void SetAuthToken(string token)
        {
            _authToken = token;
            Debug.WriteLine("[Auth] Token set");
        }

        public void ClearAuthToken()
        {
            _authToken = null;
            Debug.WriteLine("[Auth] Token cleared");
        }
    }
}
