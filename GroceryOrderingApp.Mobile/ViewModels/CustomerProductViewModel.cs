using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class CustomerProductViewModel : BaseViewModel
    {
        private List<ProductDto> _products = new();
        private int _categoryId;
        private string _errorMessage = string.Empty;
        private readonly ICartService _cartService;

        public List<ProductDto> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoadProductsCommand { get; }
        public ICommand AddToCartCommand { get; }

        public CustomerProductViewModel()
        {
            _cartService = ServiceHelper.GetService<ICartService>();
            LoadProductsCommand = new Command(async () => await LoadProductsAsync());
            AddToCartCommand = new Command<ProductDto>(async (product) => await AddToCartAsync(product));
        }

        public async Task InitializeAsync(int categoryId)
        {
            _categoryId = categoryId;
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            
            try
            {
                var response = await _apiService.GetAsync<List<ProductDto>>($"products/category/{_categoryId}");
                
                if (response.IsSuccess && response.Data != null)
                {
                    Products = response.Data;
                }
                else
                {
                    ErrorMessage = response.ErrorMessage ?? "Failed to load products";
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

        private async Task AddToCartAsync(ProductDto product)
        {
            var toastService = ServiceHelper.GetService<IToastService>();
            
            try
            {
                var result = await Application.Current!.MainPage!.DisplayPromptAsync(
                    "Add to Cart", 
                    $"Quantity for {product.Name}:", 
                    "Add", "Cancel", 
                    placeholder: "1", 
                    keyboard: Keyboard.Numeric);
                
                if (string.IsNullOrEmpty(result))
                    return;
                
                if (!int.TryParse(result, out int quantity) || quantity <= 0)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowError("Please enter a valid quantity");
                    });
                    return;
                }
                
                if (quantity > product.StockQuantity)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowError($"Only {product.StockQuantity} items in stock");
                    });
                    return;
                }

                _cartService.AddToCart(product, quantity);
                
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await toastService.ShowSuccess($"{product.Name} added to cart!");
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await toastService.ShowError($"Error: {ex.Message}");
                });
            }
        }
    }
}
