using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class CustomerOrderDetailPage : ContentPage
{
    public CustomerOrderDetailPage()
    {
        InitializeComponent();
        BindingContext = new CustomerOrderDetailViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var orderId = (int?)BindingContext ?? 1;
        var vm = (CustomerOrderDetailViewModel)BindingContext;
        await vm.LoadOrderAsync(orderId);
    }
}
