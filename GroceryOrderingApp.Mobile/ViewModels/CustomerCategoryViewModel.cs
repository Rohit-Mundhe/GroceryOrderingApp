using GroceryOrderingApp.Mobile.Models;
using GroceryOrderingApp.Mobile.Services;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class CustomerCategoryViewModel : BaseViewModel
    {
        private List<CategoryDto> _categories = new();
        private string _errorMessage = string.Empty;

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

        public ICommand LoadCategoriesCommand { get; }
        public ICommand SelectCategoryCommand { get; }

        public CustomerCategoryViewModel()
        {
            LoadCategoriesCommand = new Command(async () => await LoadCategoriesAsync());
            SelectCategoryCommand = new Command<int>(async (categoryId) => await SelectCategoryAsync(categoryId));
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

        private async Task SelectCategoryAsync(int categoryId)
        {
            try
            {
                await Shell.Current.GoToAsync($"products/{categoryId}");
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var toastService = ServiceHelper.GetService<IToastService>();
                    await toastService.ShowError($"Navigation failed: {ex.Message}");
                });
            }
        }
    }
}
