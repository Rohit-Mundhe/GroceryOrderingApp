using GroceryOrderingApp.Mobile.Models;
using Microsoft.AspNetCore.Identity;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        public ICommand LogoutCommand { get; }

        public AdminDashboardViewModel()
        {
            LogoutCommand = new Command(async () => await LogoutAsync());
        }

        private async Task LogoutAsync()
        {
            _apiService.ClearAuthToken();
            await _storageService.RemoveAsync("token");
            await _storageService.RemoveAsync("role");
            await _storageService.RemoveAsync("userId");
            Shell.Current.GoToAsync("login");
        }
    }
}
