using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class AdminOrderDetailPage : ContentPage
{
    private AdminOrderDetailViewModel? _viewModel;

    public AdminOrderDetailPage()
    {
        InitializeComponent();
        _viewModel = new AdminOrderDetailViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null && this.GetRouteParameters() is Dictionary<string, object> parameters)
        {
            if (parameters.TryGetValue("orderId", out var orderId) && int.TryParse(orderId.ToString(), out var id))
            {
                await _viewModel.LoadOrderAsync(id);
            }
        }
    }
}
