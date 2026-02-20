using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class AdminOrderDetailPage : ContentPage
{
    public AdminOrderDetailPage()
    {
        InitializeComponent();
        BindingContext = new AdminOrderDetailViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var orderId = (int?)BindingContext ?? 1;
        var vm = (AdminOrderDetailViewModel)BindingContext;
        await vm.LoadOrderAsync(orderId);
    }
}
