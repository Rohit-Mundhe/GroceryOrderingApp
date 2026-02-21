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
        
        // Fade-in animation
        this.Opacity = 0;
        await this.FadeTo(1, 400, Easing.CubicOut);
        
        var vm = (AdminOrdersViewModel)BindingContext;
        await vm.LoadOrdersAsync();
    }
}
