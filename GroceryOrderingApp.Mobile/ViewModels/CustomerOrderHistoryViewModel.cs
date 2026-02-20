using GroceryOrderingApp.Mobile.Models;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class CustomerOrderHistoryViewModel : BaseViewModel
    {
        private List<OrderDto> _orders = new();

        public List<OrderDto> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public ICommand LoadOrdersCommand { get; }

        public CustomerOrderHistoryViewModel()
        {
            LoadOrdersCommand = new Command(async () => await LoadOrdersAsync());
        }

        private async Task LoadOrdersAsync()
        {
            IsLoading = true;
            try
            {
                var orders = await _apiService.GetAsync<List<OrderDto>>("api/orders/my");
                Orders = orders ?? new();
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", $"Failed to load orders: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
