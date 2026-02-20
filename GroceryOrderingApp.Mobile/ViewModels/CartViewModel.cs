using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private List<CartItem> _cartItems = new();
        private decimal _totalAmount;
        private readonly ICartService _cartService;

        public List<CartItem> CartItems
        {
            get => _cartItems;
            set => SetProperty(ref _cartItems, value);
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set => SetProperty(ref _totalAmount, value);
        }

        public ICommand RemoveItemCommand { get; }
        public ICommand UpdateQuantityCommand { get; }
        public ICommand PlaceOrderCommand { get; }
        public ICommand RefreshCommand { get; }

        public CartViewModel()
        {
            _cartService = ServiceHelper.GetService<ICartService>();
            RemoveItemCommand = new Command<CartItem>((item) => RemoveItem(item));
            UpdateQuantityCommand = new Command<CartItem>((item) => UpdateQuantity(item));
            PlaceOrderCommand = new Command(async () => await PlaceOrderAsync());
            RefreshCommand = new Command(() => Refresh());
        }

        public void Refresh()
        {
            CartItems = _cartService.GetCartItems();
            TotalAmount = _cartService.GetCartTotal();
        }

        private void RemoveItem(CartItem item)
        {
            _cartService.RemoveFromCart(item.ProductId);
            Refresh();
        }

        private async void UpdateQuantity(CartItem item)
        {
            var result = await Application.Current!.MainPage!.DisplayPromptAsync("Update Quantity", $"Enter new quantity:", "Update", "Cancel", placeholder: item.Quantity.ToString(), keyboard: Keyboard.Numeric);
            if (!string.IsNullOrEmpty(result) && int.TryParse(result, out int newQuantity))
            {
                if (newQuantity <= 0)
                    _cartService.RemoveFromCart(item.ProductId);
                else
                    _cartService.UpdateCartQuantity(item.ProductId, newQuantity);
                Refresh();
            }
        }

        private async Task PlaceOrderAsync()
        {
            if (CartItems.Count == 0)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Cart is empty", "OK");
                return;
            }

            IsLoading = true;
            try
            {
                var orderItems = CartItems.Select(c => new { productId = c.ProductId, quantity = c.Quantity }).ToList();
                var orderRequest = new { items = orderItems };

                var response = await _apiService.PostAsync<OrderDto>("api/orders", orderRequest);
                if (response != null)
                {
                    _cartService.ClearCart();
                    Refresh();
                    await Application.Current!.MainPage!.DisplayAlert("Success", "Order placed successfully", "OK");
                    await Shell.Current.GoToAsync("customer/orderhistory");
                }
                else
                {
                    await Application.Current!.MainPage!.DisplayAlert("Error", "Failed to place order", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", $"Order failed: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
