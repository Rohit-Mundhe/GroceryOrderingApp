using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class AdminProductsViewModel : BaseViewModel
    {
        private List<ProductDto> _products = new();
        private string _errorMessage = string.Empty;
        private bool _showAddForm;

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

        public bool ShowAddForm
        {
            get => _showAddForm;
            set => SetProperty(ref _showAddForm, value);
        }

        public ICommand LoadProductsCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand RefreshCommand { get; }

        public AdminProductsViewModel()
        {
            LoadProductsCommand = new Command(async () => await LoadProductsAsync());
            DeleteProductCommand = new Command<int>(async (id) => await DeleteProductAsync(id));
            RefreshCommand = new Command(async () => await LoadProductsAsync());
        }

        public async Task LoadProductsAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var response = await _apiService.GetAsync<List<ProductDto>>("products");
                
                if (response.IsSuccess && response.Data != null)
                {
                    Products = response.Data;
                    
                    if (Products.Count == 0)
                    {
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            var toastService = ServiceHelper.GetService<IToastService>();
                            await toastService.ShowInfo("No products found");
                        });
                    }
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

        private async Task DeleteProductAsync(int productId)
        {
            var toastService = ServiceHelper.GetService<IToastService>();
            
            var confirm = await Application.Current!.MainPage!.DisplayAlert(
                "Confirm Deletion",
                "Are you sure you want to delete this product?",
                "Yes",
                "No");

            if (!confirm)
                return;

            IsLoading = true;

            try
            {
                var response = await _apiService.DeleteAsync($"admin/products/{productId}");
                
                if (response.IsSuccess)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowSuccess("Product deleted successfully ✓");
                        await LoadProductsAsync();
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowError(response.ErrorMessage ?? "Failed to delete product");
                    });
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await toastService.ShowError($"Error: {ex.Message}");
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task AddProductAsync(string name, string description, decimal price, int categoryId)
        {
            var toastService = ServiceHelper.GetService<IToastService>();

            if (string.IsNullOrWhiteSpace(name))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await toastService.ShowError("Product name is required");
                });
                return;
            }

            IsLoading = true;

            try
            {
                var productRequest = new
                {
                    name,
                    description,
                    price,
                    categoryId,
                    stockQuantity = 100 // Default stock
                };

                var response = await _apiService.PostAsync<ProductDto>("admin/products", productRequest);
                
                if (response.IsSuccess)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowSuccess($"Product '{name}' added successfully ✓");
                        ShowAddForm = false;
                        await LoadProductsAsync();
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowError(response.ErrorMessage ?? "Failed to add product");
                    });
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await toastService.ShowError($"Error: {ex.Message}");
                });
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
