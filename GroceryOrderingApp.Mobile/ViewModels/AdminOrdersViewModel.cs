using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class AdminOrdersViewModel : BaseViewModel
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
        public ICommand SelectOrderCommand { get; }

        public AdminOrdersViewModel()
        {
            LoadOrdersCommand = new Command(async () => await LoadOrdersAsync());
            SelectOrderCommand = new Command<int>(async (orderId) => await SelectOrderAsync(orderId));
        }

        public async Task LoadOrdersAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var response = await _apiService.GetAsync<List<OrderDto>>("admin/orders");
                
                if (response.IsSuccess && response.Data != null)
                {
                    Orders = response.Data;
                    
                    if (Orders.Count == 0)
                    {
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            var toastService = ServiceHelper.GetService<IToastService>();
                            await toastService.ShowInfo("No orders found");
                        });
                    }
                }
                else
                {
                    ErrorMessage = response.ErrorMessage ?? "Failed to load orders";
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var toastService = ServiceHelper.GetService<IToastService>();
                        await toastService.ShowError(ErrorMessage);
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
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

        private async Task SelectOrderAsync(int orderId)
        {
            try
            {
                await Shell.Current.GoToAsync($"orderdetail/{orderId}");
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var toastService = ServiceHelper.GetService<IToastService>();
                    await toastService.ShowError($"Navigation failed: {ex.Message}");
                });
            }
        }
    }
}
