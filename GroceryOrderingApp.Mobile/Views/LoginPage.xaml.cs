using GroceryOrderingApp.Mobile.ViewModels;

namespace GroceryOrderingApp.Mobile.Views;

public partial class LoginPage : ContentPage
{
    private bool _isPasswordVisible = false;

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
        
        // Wire up password toggle button
        PasswordToggle.Clicked += OnPasswordToggleClicked;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Fade-in animation
        this.Opacity = 0;
        await this.FadeTo(1, 600, Easing.CubicOut);
    }

    private void OnPasswordToggleClicked(object? sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;
        PasswordEntry.IsPassword = !_isPasswordVisible;
        PasswordToggle.Text = _isPasswordVisible ? "👁‍🗨" : "👁";
    }
}