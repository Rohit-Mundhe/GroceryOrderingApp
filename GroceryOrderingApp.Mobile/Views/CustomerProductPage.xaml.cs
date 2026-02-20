using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class CustomerProductPage : ContentPage
{
    public CustomerProductPage()
    {
        InitializeComponent();
        BindingContext = new CustomerProductViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var categoryId = (int?)BindingContext ?? 1;
        var vm = (CustomerProductViewModel)BindingContext;
        await vm.InitializeAsync(categoryId);
    }
}
