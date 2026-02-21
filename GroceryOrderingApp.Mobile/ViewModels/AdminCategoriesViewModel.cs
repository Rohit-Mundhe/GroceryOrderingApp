using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class AdminCategoriesViewModel : BaseViewModel
    {
        private List<CategoryDto> _categories = new();
        private string _errorMessage = string.Empty;
        private bool _showAddForm;

        public List<CategoryDto> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
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

        public ICommand LoadCategoriesCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand RefreshCommand { get; }

        public AdminCategoriesViewModel()
        {
            LoadCategoriesCommand = new Command(async () => await LoadCategoriesAsync());
            DeleteCategoryCommand = new Command<int>(async (id) => await DeleteCategoryAsync(id));
            RefreshCommand = new Command(async () => await LoadCategoriesAsync());
        }

        public async Task LoadCategoriesAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var response = await _apiService.GetAsync<List<CategoryDto>>("categories");
                
                if (response.IsSuccess && response.Data != null)
                {
                    Categories = response.Data;
                    
                    if (Categories.Count == 0)
                    {
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            var toastService = ServiceHelper.GetService<IToastService>();
                            await toastService.ShowInfo("No categories found");
                        });
                    }
                }
                else
                {
                    ErrorMessage = response.ErrorMessage ?? "Failed to load categories";
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

        private async Task DeleteCategoryAsync(int categoryId)
        {
            var toastService = ServiceHelper.GetService<IToastService>();
            
            var confirm = await Application.Current!.MainPage!.DisplayAlert(
                "Confirm Deletion",
                "Are you sure you want to delete this category?",
                "Yes",
                "No");

            if (!confirm)
                return;

            IsLoading = true;

            try
            {
                var response = await _apiService.DeleteAsync($"admin/categories/{categoryId}");
                
                if (response.IsSuccess)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowSuccess("Category deleted successfully ✓");
                        await LoadCategoriesAsync();
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowError(response.ErrorMessage ?? "Failed to delete category");
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

        public async Task AddCategoryAsync(string name)
        {
            var toastService = ServiceHelper.GetService<IToastService>();

            if (string.IsNullOrWhiteSpace(name))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await toastService.ShowError("Category name is required");
                });
                return;
            }

            IsLoading = true;

            try
            {
                var categoryRequest = new { name };
                var response = await _apiService.PostAsync<CategoryDto>("admin/categories", categoryRequest);
                
                if (response.IsSuccess)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowSuccess($"Category '{name}' added successfully ✓");
                        ShowAddForm = false;
                        await LoadCategoriesAsync();
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await toastService.ShowError(response.ErrorMessage ?? "Failed to add category");
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
