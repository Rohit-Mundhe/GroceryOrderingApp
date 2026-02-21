using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class AdminCategoriesPage : ContentPage
{
    public AdminCategoriesPage()
    {
        InitializeComponent();
        BindingContext = new AdminCategoriesViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = (AdminCategoriesViewModel)BindingContext;
        await vm.LoadCategoriesAsync();
    }
}
