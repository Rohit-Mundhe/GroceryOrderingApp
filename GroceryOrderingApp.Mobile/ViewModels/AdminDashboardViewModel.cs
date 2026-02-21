using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        private int _totalOrders;
        private decimal _totalRevenue;
        private int _pendingOrders;
        private int _totalProducts;
        private string _errorMessage = string.Empty;

        public int TotalOrders
        {
            get => _totalOrders;
            set => SetProperty(ref _totalOrders, value);
        }

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        public int PendingOrders
        {
            get => _pendingOrders;
            set => SetProperty(ref _pendingOrders, value);
        }

        public int TotalProducts
        {
            get => _totalProducts;
            set => SetProperty(ref _totalProducts, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoadDashboardCommand { get; }
        public ICommand LogoutCommand { get; }

        public AdminDashboardViewModel()
        {
            LoadDashboardCommand = new Command(async () => await LoadDashboardAsync());
            LogoutCommand = new Command(async () => await LogoutAsync());
        }

        public async Task LoadDashboardAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                // Load all orders to calculate stats
                var ordersResponse = await _apiService.GetAsync<List<OrderDto>>("orders");
                
                if (ordersResponse.IsSuccess && ordersResponse.Data != null)
                {
                    var orders = ordersResponse.Data;
                    TotalOrders = orders.Count;
                    TotalRevenue = orders.Sum(o => o.TotalAmount);
                    PendingOrders = orders.Count(o => o.Status?.Equals("Pending", StringComparison.OrdinalIgnoreCase) == true);
                }

                // Load products count
                var productsResponse = await _apiService.GetAsync<List<ProductDto>>("products");
                if (productsResponse.IsSuccess && productsResponse.Data != null)
                {
                    TotalProducts = productsResponse.Data.Count;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading dashboard: {ex.Message}";
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

        private async Task LogoutAsync()
        {
            _apiService.ClearAuthToken();
            await _storageService.RemoveAsync("token");
            await _storageService.RemoveAsync("role");
            await _storageService.RemoveAsync("userId");

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var toastService = ServiceHelper.GetService<IToastService>();
                await toastService.ShowSuccess("Logged out successfully");
                await Shell.Current.GoToAsync("login");
            });
        }
    }
}
