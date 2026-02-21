using GroceryOrderingApp.Mobile.Services;
using GroceryOrderingApp.Mobile.Views;

namespace GroceryOrderingApp.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Services
        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddSingleton<ICartService, CartService>();
        builder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();
        builder.Services.AddSingleton<IToastService, ToastService>();

        // Register Pages
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<CustomerCategoryPage>();
        builder.Services.AddSingleton<CustomerProductPage>();
        builder.Services.AddSingleton<CartPage>();
        builder.Services.AddSingleton<CustomerOrderHistoryPage>();
        builder.Services.AddSingleton<CustomerOrderDetailPage>();
        builder.Services.AddSingleton<AdminDashboardPage>();
        builder.Services.AddSingleton<AdminOrdersPage>();
        builder.Services.AddSingleton<AdminOrderDetailPage>();
        builder.Services.AddSingleton<AdminUsersPage>();
        builder.Services.AddSingleton<AdminCategoriesPage>();
        builder.Services.AddSingleton<AdminProductsPage>();

        // Register Views as Transient (for routing)
        builder.Services.AddTransient<CustomerProductPage>();
        builder.Services.AddTransient<CustomerOrderDetailPage>();
        builder.Services.AddTransient<AdminOrderDetailPage>();

        return builder.Build();
    }
}

public static class ServiceHelper
{
    public static T GetService<T>() where T : class
    {
        if (Application.Current is MauiApp app)
        {
            var service = app.Services.GetService(typeof(T));
            return (T)service!;
        }
        throw new InvalidOperationException("MauiApp not initialized");
    }
}
