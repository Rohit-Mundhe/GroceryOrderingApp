using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class CustomerProductViewModel : BaseViewModel
    {
        private List<ProductDto> _products = new();
        private int _categoryId;
        private readonly ICartService _cartService;

        public List<ProductDto> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
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
            try
            {
                var products = await _apiService.GetAsync<List<ProductDto>>($"api/products?categoryId={_categoryId}");
                Products = products ?? new();
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddToCartAsync(ProductDto product)
        {
            var result = await Application.Current!.MainPage!.DisplayPromptAsync("Add to Cart", $"Enter quantity for {product.Name}:", "Add", "Cancel", placeholder: "1", keyboard: Keyboard.Numeric);
            
            if (!string.IsNullOrEmpty(result) && int.TryParse(result, out int quantity) && quantity > 0)
            {
                if (quantity > product.StockQuantity)
                {
                    await Application.Current!.MainPage!.DisplayAlert("Error", $"Only {product.StockQuantity} items available", "OK");
                    return;
                }

                _cartService.AddToCart(product, quantity);
                await Application.Current!.MainPage!.DisplayAlert("Success", $"{product.Name} added to cart", "OK");
            }
        }
    }
}
