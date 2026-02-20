# Grocery Ordering Application - Mobile Frontend (.NET MAUI)

## Overview
Production-ready .NET MAUI Android application for a Grocery Ordering System with local cart management, JWT authentication, and comprehensive order tracking.

## Technology Stack
- **Framework**: .NET MAUI 8.0
- **Target**: Android 12.0+ (API 24+)
- **UI Framework**: XAML with MVVM
- **HTTP Client**: HttpClient with custom API service
- **Storage**: Secure storage for tokens
- **State Management**: ObservableCollection with INotifyPropertyChanged

## Project Structure
```
GroceryOrderingApp.Mobile/
├── Views/                  # XAML pages
├── ViewModels/            # MVVM ViewModels
├── Models/                # Data models & DTOs
├── Services/              # API & Cart services
├── Resources/             # Styles & assets
├── Platforms/             # Platform-specific code
├── App.xaml               # App configuration
├── AppShell.xaml          # Navigation shell
├── MauiProgram.cs         # Dependency injection
└── AppConfig.cs           # Configuration constants
```

## Key Features
✅ Role-based login (Admin/Customer)
✅ Customer: Browse categories & products
✅ Customer: Local shopping cart
✅ Customer: Place orders with stock validation
✅ Customer: View order history & details
✅ Admin: View all orders
✅ Admin: Mark orders as delivered (stock reduction)
✅ Admin: Cancel pending orders
✅ Secure local storage for authentication tokens
✅ Responsive UI with loading indicators
✅ Error handling with user-friendly messages

## Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 with MAUI workload OR
- VS Code with MAUI extension
- Android SDK (API 24+)
- Java Development Kit (JDK 11+)

## Installation & Setup

### 1. Install MAUI Workload
```bash
dotnet workload install maui
```

### 2. Update API Endpoint
Edit `Services/ApiService.cs`:
```csharp
BaseAddress = new Uri("https://YOUR_API_URL:PORT")
```

Default development: `https://localhost:7001`

### 3. Build & Run
```bash
# Build for Android
dotnet build -f net8.0-android

# Run on emulator
dotnet maui run -f net8.0-android
```

## Application Screens

### Customer Flow
1. **Login Page** - UserId & Password authentication
2. **Categories Page** - Browse all active categories
3. **Products Page** - View products by category
4. **Cart Page** - Manage cart items & place order
5. **Order History** - View all customer orders
6. **Order Detail** - View order items & status

### Admin Flow
1. **Login Page** - Authentication
2. **Dashboard** - Admin menu
3. **Orders List** - View all orders with status
4. **Order Detail** - Mark as delivered or cancel order

## Default Credentials
```
UserId: admin
Password: Admin@123
Role: Admin
```

## Cart Management
- **Storage**: In-memory during session
- **Persistence**: Cleared on logout
- **Stock Validation**: Checked before order placement
- **Quantity Update**: Real-time UI update

## Navigation Routes
```
login              → LoginPage
customer           → TabBar with:
  ├── categories   → CustomerCategoryPage
  ├── cart         → CartPage
  └── orderhistory → CustomerOrderHistoryPage
customer/products  → CustomerProductPage
customer/orderdetail → CustomerOrderDetailPage
admin              → TabBar with:
  ├── dashboard    → AdminDashboardPage
  └── orders       → AdminOrdersPage
admin/orderdetail  → AdminOrderDetailPage
```

## Building for Google Play Store

### 1. Generate Keystore
```bash
keytool -genkey -v -keystore grocery-app.keystore -keyalg RSA -keysize 2048 -validity 10000 -alias grocerykey
```

### 2. Update MAUI Project File
Edit `.csproj`:
```xml
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">
    <AndroidKeyStore>True</AndroidKeyStore>
    <AndroidSigningKeyStore>grocery-app.keystore</AndroidSigningKeyStore>
    <AndroidSigningKeyAlias>grocerykey</AndroidSigningKeyAlias>
    <AndroidSigningKeyPass>YOUR_KEY_PASSWORD</AndroidSigningKeyPass>
    <AndroidSigningStorePass>YOUR_STORE_PASSWORD</AndroidSigningStorePass>
</PropertyGroup>
```

### 3. Build Signed App Bundle (AAB)
```bash
dotnet publish -f net8.0-android -c Release
```

Output: `bin/Release/net8.0-android/publish/*.aab`

### 4. Google Play Store Requirements
- ✅ App Icon (512x512 PNG)
- ✅ Feature Graphic (1024x500 PNG)
- ✅ Screenshots (4-5 per device type)
- ✅ Short Description (50 chars)
- ✅ Full Description (4000 chars)
- ✅ Privacy Policy URL
- ✅ Content Rating
- ✅ App Category: Shopping

## Security Features
✅ HTTPS-only API communication
✅ JWT token secure storage
✅ Token validation on all authenticated requests
✅ Input validation on all forms
✅ Secure storage using platform capabilities
✅ No hardcoded credentials in code

## Testing
```bash
# Unit test (create Tests project)
dotnet test

# Manual testing checklist:
☑ Login with admin credentials
☑ Login with customer credentials
☑ Browse categories
☑ Add products to cart
☑ Update cart quantities
☑ Place order with valid stock
☑ Verify order in history
☑ Admin: View orders
☑ Admin: Mark order as delivered
☑ Verify stock reduction
```

## Performance Optimization
- Lazy loading of order lists
- Local cart caching
- Image optimization
- Minimal UI animations
- Efficient API calls

## Troubleshooting

### API Connection Issues
```
Error: Unable to connect to API
Solution: 
1. Verify API is running (https://localhost:7001)
2. Check firewall settings
3. Update ApiService.cs with correct URL
4. For device: use actual server IP instead of localhost
```

### Android Build Errors
```
Error: MissingReferenceException
Solution: Ensure Android SDK is properly installed
```

## Deployment Checklist
- [ ] Update API endpoint to production URL
- [ ] Update JWT secret on backend
- [ ] Test all workflows end-to-end
- [ ] Build signed APK/AAB
- [ ] Create app store listing
- [ ] Upload privacy policy
- [ ] Submit for review

## Version History
- **v1.0** - Initial release with MVP features

## Support
For issues or feature requests, contact the development team.

## License
MIT
