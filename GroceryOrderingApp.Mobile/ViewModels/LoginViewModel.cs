using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

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
            if (string.IsNullOrWhiteSpace(UserId))
            {
                ErrorMessage = "UserId is required";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Password is required";
                return;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var apiResponse = await _apiService.PostAsync<LoginResponse>("auth/login", new LoginRequest
                {
                    UserId = UserId,
                    Password = Password
                });

                if (apiResponse.IsSuccess && apiResponse.Data != null)
                {
                    var loginData = apiResponse.Data;
                    await _storageService.SetAsync("token", loginData.Token);
                    await _storageService.SetAsync("role", loginData.Role);
                    await _storageService.SetAsync("userId", loginData.UserId.ToString());
                    _apiService.SetAuthToken(loginData.Token);

                    // Toast notification
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var toastService = ServiceHelper.GetService<IToastService>();
                        await toastService.ShowSuccess($"Welcome, {loginData.Role}!");
                    });

                    // Navigate based on role
                    if (loginData.Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true)
                        await Shell.Current.GoToAsync("admin");
                    else
                        await Shell.Current.GoToAsync("customer");
                }
                else
                {
                    ErrorMessage = apiResponse.ErrorMessage ?? "Invalid credentials. Please try again.";
                    
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var toastService = ServiceHelper.GetService<IToastService>();
                        await toastService.ShowError(ErrorMessage);
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
                
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var toastService = ServiceHelper.GetService<IToastService>();
                    await toastService.ShowError(ErrorMessage);
                });
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
