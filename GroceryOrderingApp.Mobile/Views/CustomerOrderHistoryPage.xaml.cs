using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class CustomerOrderHistoryPage : ContentPage
{
    public CustomerOrderHistoryPage()
    {
        InitializeComponent();
        BindingContext = new CustomerOrderHistoryViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = (CustomerOrderHistoryViewModel)BindingContext;
        await vm.LoadOrdersAsync();
    }
}
