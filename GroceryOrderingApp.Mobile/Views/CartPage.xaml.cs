using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class CartPage : ContentPage
{
    public CartPage()
    {
        InitializeComponent();
        BindingContext = new CartViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var vm = (CartViewModel)BindingContext;
        vm.Refresh();
    }
}
