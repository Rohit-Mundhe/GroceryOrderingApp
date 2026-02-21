using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private List<CartItem> _cartItems = new();
        private decimal _totalAmount;
        private string _errorMessage = string.Empty;
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

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
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
            var toastService = ServiceHelper.GetService<IToastService>();
            _cartService.RemoveFromCart(item.ProductId);
            Refresh();
            
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await toastService.ShowInfo($"{item.ProductName} removed from cart");
            });
        }

        private async void UpdateQuantity(CartItem item)
        {
            try
            {
                var result = await Application.Current!.MainPage!.DisplayPromptAsync(
                    "Update Quantity", 
                    $"New quantity for {item.ProductName}:", 
                    "Update", "Cancel", 
                    placeholder: item.Quantity.ToString(), 
                    keyboard: Keyboard.Numeric);
                    
                if (string.IsNullOrEmpty(result))
                    return;
                    
                var toastService = ServiceHelper.GetService<IToastService>();
                
                if (!int.TryParse(result, out int newQuantity) || newQuantity < 0)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowError("Please enter a valid quantity");
                    });
                    return;
                }
                
                if (newQuantity == 0)
                {
                    _cartService.RemoveFromCart(item.ProductId);
                    Refresh();
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowInfo("Item removed from cart");
                    });
                }
                else if (newQuantity != item.Quantity)
                {
                    _cartService.UpdateCartQuantity(item.ProductId, newQuantity);
                    Refresh();
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowSuccess("Quantity updated");
                    });
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var toastService = ServiceHelper.GetService<IToastService>();
                    await toastService.ShowError($"Error: {ex.Message}");
                });
            }
        }

        private async Task PlaceOrderAsync()
        {
            var toastService = ServiceHelper.GetService<IToastService>();
            
            if (CartItems.Count == 0)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await toastService.ShowError("Your cart is empty");
                });
                return;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;
            
            try
            {
                var orderItems = CartItems.Select(c => new { productId = c.ProductId, quantity = c.Quantity }).ToList();
                var orderRequest = new { items = orderItems };

                var response = await _apiService.PostAsync<OrderDto>("orders", orderRequest);
                
                if (response.IsSuccess && response.Data != null)
                {
                    _cartService.ClearCart();
                    Refresh();
                    
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowSuccess("Order placed successfully! ✓");
                        await Shell.Current.GoToAsync("customer/orderhistory");
                    });
                }
                else
                {
                    ErrorMessage = response.ErrorMessage ?? "Failed to place order";
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
