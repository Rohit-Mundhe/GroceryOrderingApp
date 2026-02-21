using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class CustomerOrderDetailViewModel : BaseViewModel
    {
        private OrderDto? _order;
        private string _errorMessage = string.Empty;

        public OrderDto? Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoadOrderCommand { get; }

        public CustomerOrderDetailViewModel()
        {
            LoadOrderCommand = new Command<int>(async (id) => await LoadOrderAsync(id));
        }

        public async Task LoadOrderAsync(int orderId)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var response = await _apiService.GetAsync<OrderDto>($"orders/{orderId}");
                
                if (response.IsSuccess && response.Data != null)
                {
                    Order = response.Data;
                }
                else
                {
                    ErrorMessage = response.ErrorMessage ?? "Failed to load order";
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
