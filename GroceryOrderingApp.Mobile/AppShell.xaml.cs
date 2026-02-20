namespace GroceryOrderingApp.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("customer/products", typeof(Views.CustomerProductPage));
        Routing.RegisterRoute("customer/orderdetail", typeof(Views.CustomerOrderDetailPage));
        Routing.RegisterRoute("admin/orderdetail", typeof(Views.AdminOrderDetailPage));
    }
}
