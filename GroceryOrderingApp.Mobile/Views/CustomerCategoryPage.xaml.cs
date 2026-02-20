using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class CustomerCategoryPage : ContentPage
{
    public CustomerCategoryPage()
    {
        InitializeComponent();
        BindingContext = new CustomerCategoryViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = (CustomerCategoryViewModel)BindingContext;
        await vm.LoadCategoriesAsync();
    }
}
