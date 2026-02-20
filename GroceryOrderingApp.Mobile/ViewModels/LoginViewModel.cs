using GroceryOrderingApp.Mobile.Models;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userId = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public string UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "UserId and Password are required";
                return;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var response = await _apiService.PostAsync<LoginResponse>("api/auth/login", new LoginRequest
                {
                    UserId = UserId,
                    Password = Password
                });

                if (response != null)
                {
                    await _storageService.SetAsync("token", response.Token);
                    await _storageService.SetAsync("role", response.Role);
                    await _storageService.SetAsync("userId", response.UserId.ToString());
                    _apiService.SetAuthToken(response.Token);

                    // Navigate based on role
                    if (response.Role == "Admin")
                        Shell.Current.GoToAsync("admin");
                    else
                        Shell.Current.GoToAsync("customer");
                }
                else
                {
                    ErrorMessage = "Invalid credentials";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
