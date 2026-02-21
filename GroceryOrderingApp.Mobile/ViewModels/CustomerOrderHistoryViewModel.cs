using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class CustomerOrderHistoryViewModel : BaseViewModel
    {
        private List<OrderDto> _orders = new();
        private string _errorMessage = string.Empty;

        public List<OrderDto> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoadOrdersCommand { get; }
        public ICommand RefreshCommand { get; }

        public CustomerOrderHistoryViewModel()
        {
            LoadOrdersCommand = new Command(async () => await LoadOrdersAsync());
            RefreshCommand = new Command(async () => await LoadOrdersAsync());
        }

        public async Task LoadOrdersAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var response = await _apiService.GetAsync<List<OrderDto>>("orders");
                
                if (response.IsSuccess && response.Data != null)
                {
                    Orders = response.Data;
                }
                else
                {
                    ErrorMessage = response.ErrorMessage ?? "Failed to load orders";
                    var toastService = ServiceHelper.GetService<IToastService>();
                    MainThread.BeginInvokeOnMainThread(async () => await toastService.ShowError(ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                var toastService = ServiceHelper.GetService<IToastService>();
                MainThread.BeginInvokeOnMainThread(async () => await toastService.ShowError(ErrorMessage));
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
