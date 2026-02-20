using GroceryOrderingApp.Mobile.Models;

namespace GroceryOrderingApp.Mobile.ViewModels
{
    public class CustomerCategoryViewModel : BaseViewModel
    {
        private List<CategoryDto> _categories = new();

        public List<CategoryDto> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public ICommand LoadCategoriesCommand { get; }

        public CustomerCategoryViewModel()
        {
            LoadCategoriesCommand = new Command(async () => await LoadCategoriesAsync());
        }

        public async Task LoadCategoriesAsync()
        {
            IsLoading = true;
            try
            {
                var categories = await _apiService.GetAsync<List<CategoryDto>>("api/categories");
                Categories = categories ?? new();
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
