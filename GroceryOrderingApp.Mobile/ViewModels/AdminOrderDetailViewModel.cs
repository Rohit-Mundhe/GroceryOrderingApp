using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class AdminOrderDetailViewModel : BaseViewModel
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
        public ICommand DeliverOrderCommand { get; }
        public ICommand CancelOrderCommand { get; }

        public AdminOrderDetailViewModel()
        {
            LoadOrderCommand = new Command<int>(async (id) => await LoadOrderAsync(id));
            DeliverOrderCommand = new Command(async () => await UpdateOrderStatusAsync("Delivered"));
            CancelOrderCommand = new Command(async () => await UpdateOrderStatusAsync("Cancelled"));
        }

        private async Task LoadOrderAsync(int orderId)
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

        private async Task UpdateOrderStatusAsync(string newStatus)
        {
            var toastService = ServiceHelper.GetService<IToastService>();

            if (Order == null)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await toastService.ShowError("No order loaded");
                });
                return;
            }

            // Confirm action
            var confirm = await Application.Current!.MainPage!.DisplayAlert(
                "Confirm", 
                $"Mark order as {newStatus}?", 
                "Yes", 
                "No");

            if (!confirm)
                return;

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var updateRequest = new { status = newStatus };
                var response = await _apiService.PutAsync<OrderDto>($"orders/{Order.Id}/status", updateRequest);

                if (response.IsSuccess && response.Data != null)
                {
                    Order = response.Data;
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowSuccess($"Order marked as {newStatus} ✓");
                    });
                }
                else
                {
                    ErrorMessage = response.ErrorMessage ?? $"Failed to update order";
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowError(ErrorMessage);
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                MainThread.BeginInvokeOnMainThread(async () =>
                {
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
