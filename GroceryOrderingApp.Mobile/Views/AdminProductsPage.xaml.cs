using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class AdminProductsPage : ContentPage
{
    public AdminProductsPage()
    {
        InitializeComponent();
        BindingContext = new AdminProductsViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = (AdminProductsViewModel)BindingContext;
        await vm.LoadProductsAsync();
    }
}
