using GroceryOrderingApp.Mobile.Models;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class CustomerOrderDetailViewModel : BaseViewModel
    {
        private OrderDto? _order;

        public OrderDto? Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public ICommand LoadOrderCommand { get; }

        public CustomerOrderDetailViewModel()
        {
            LoadOrderCommand = new Command<int>(async (id) => await LoadOrderAsync(id));
        }

        private async Task LoadOrderAsync(int orderId)
        {
            IsLoading = true;
            try
            {
                var order = await _apiService.GetAsync<OrderDto>($"api/orders/{orderId}");
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
    }
}
