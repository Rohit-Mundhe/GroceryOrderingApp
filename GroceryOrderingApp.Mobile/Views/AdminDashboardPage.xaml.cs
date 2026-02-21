using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class AdminDashboardPage : ContentPage
{
    private AdminDashboardViewModel? _viewModel;

    public AdminDashboardPage()
    {
        InitializeComponent();
        _viewModel = new AdminDashboardViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Fade-in animation
        this.Opacity = 0;
        await this.FadeTo(1, 400, Easing.CubicOut);
        
        if (_viewModel != null)
        {
            await _viewModel.LoadDashboardAsync();
        }
    }

    private async void OnManageOrders(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("admin/orders");
    }

    private async void OnManageProducts(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("admin/products");
    }

    private async void OnManageCategories(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("admin/categories");
    }
}
