using GroceryOrderingApp.Mobile.Models;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class AdminOrderDetailViewModel : BaseViewModel
    {
        private OrderDto? _order;

        public OrderDto? Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public ICommand LoadOrderCommand { get; }
        public ICommand DeliverOrderCommand { get; }
        public ICommand CancelOrderCommand { get; }

        public AdminOrderDetailViewModel()
        {
            LoadOrderCommand = new Command<int>(async (id) => await LoadOrderAsync(id));
            DeliverOrderCommand = new Command(async () => await DeliverOrderAsync());
            CancelOrderCommand = new Command(async () => await CancelOrderAsync());
        }

        private async Task LoadOrderAsync(int orderId)
        {
            IsLoading = true;
            try
            {
                var order = await _apiService.GetAsync<OrderDto>($"api/admin/orders/{orderId}");
                Order = order;
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", $"Failed to load order: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeliverOrderAsync()
        {
            if (Order == null || Order.Status != "Pending")
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Can only deliver pending orders", "OK");
                return;
            }

            IsLoading = true;
            try
            {
                var result = await _apiService.PutAsync<object>($"api/admin/orders/{Order.Id}/deliver");
                if (result != null)
                {
                    await Application.Current!.MainPage!.DisplayAlert("Success", "Order delivered", "OK");
                    Order!.Status = "Delivered";
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", $"Failed: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task CancelOrderAsync()
        {
            if (Order == null || Order.Status != "Pending")
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Can only cancel pending orders", "OK");
                return;
            }

            IsLoading = true;
            try
            {
                var result = await _apiService.PutAsync<object>($"api/admin/orders/{Order.Id}/cancel");
                if (result != null)
                {
                    await Application.Current!.MainPage!.DisplayAlert("Success", "Order cancelled", "OK");
                    Order!.Status = "Cancelled";
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", $"Failed: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
