using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class AdminOrdersPage : ContentPage
{
    public AdminOrdersPage()
    {
        InitializeComponent();
        BindingContext = new AdminOrdersViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = (AdminOrdersViewModel)BindingContext;
        await vm.LoadOrdersAsync();
    }
}
