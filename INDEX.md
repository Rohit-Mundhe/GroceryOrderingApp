# 📑 Project Navigation Index

## Overview Documents
- **[README.md](README.md)** - Start here! Complete project overview
- **[PROJECT_COMPLETION_SUMMARY.md](PROJECT_COMPLETION_SUMMARY.md)** - What's been delivered
- **[QUICK_START.md](QUICK_START.md)** - Get running in 5 minutes
- **[DEPLOYMENT.md](DEPLOYMENT.md)** - Production deployment guide

## Getting Started
1. Read this file
2. Open [QUICK_START.md](QUICK_START.md)
3. Follow setup steps
4. Test with default credentials

## Backend Documentation
- **[GroceryOrderingApp.Backend/README.md](GroceryOrderingApp.Backend/README.md)** - Backend specific docs
- **[GroceryOrderingApp.Backend/appsettings.json](GroceryOrderingApp.Backend/appsettings.json)** - Configuration
- **[API_TESTING_GUIDE.md](API_TESTING_GUIDE.md)** - Test endpoints with Postman

## Frontend Documentation
- **[GroceryOrderingApp.Mobile/README.md](GroceryOrderingApp.Mobile/README.md)** - Mobile app docs
- **[GroceryOrderingApp.Mobile/AppConfig.cs](GroceryOrderingApp.Mobile/AppConfig.cs)** - Configuration

## Legal Documents
- **[PRIVACY_POLICY.md](PRIVACY_POLICY.md)** - Privacy compliance
- **[TERMS_OF_SERVICE.md](TERMS_OF_SERVICE.md)** - Legal terms

---

## 📁 Project Structure

```
GroceryOrderingApp/
│
├── README.md                                    [Main overview]
├── QUICK_START.md                              [5-min setup]
├── DEPLOYMENT.md                               [Deploy guide]
├── PROJECT_COMPLETION_SUMMARY.md               [What's done]
├── API_TESTING_GUIDE.md                       [Test examples]
├── PRIVACY_POLICY.md                          [Privacy]
├── TERMS_OF_SERVICE.md                        [Terms]
│
├── GroceryOrderingApp.Backend/                [ASP.NET Core API]
│   ├── README.md
│   ├── GroceryOrderingApp.Backend.csproj
│   ├── Program.cs                             [Setup & DI]
│   ├── DatabaseSeeder.cs                      [Initial data]
│   ├── appsettings.json                       [Config]
│   ├── appsettings.Development.json
│   │
│   ├── Controllers/                           [API endpoints]
│   │   ├── AuthController.cs
│   │   ├── AdminController.cs
│   │   ├── CategoriesController.cs
│   │   ├── ProductsController.cs
│   │   └── OrdersController.cs
│   │
│   ├── Models/                                [Database entities]
│   │   ├── Role.cs
│   │   ├── User.cs
│   │   ├── Category.cs
│   │   ├── Product.cs
│   │   ├── Order.cs
│   │   └── OrderItem.cs
│   │
│   ├── DTOs/                                  [Data transfer objects]
│   │   ├── AuthDtos.cs
│   │   ├── UserDtos.cs
│   │   ├── CategoryDtos.cs
│   │   ├── ProductDtos.cs
│   │   └── OrderDtos.cs
│   │
│   ├── Services/                              [Business logic]
│   │   ├── IAuthService.cs
│   │   ├── AuthService.cs
│   │   ├── IOrderService.cs
│   │   └── OrderService.cs
│   │
│   ├── Repositories/                          [Data access]
│   │   ├── IUserRepository.cs
│   │   ├── UserRepository.cs
│   │   ├── ICategoryRepository.cs
│   │   ├── CategoryRepository.cs
│   │   ├── IProductRepository.cs
│   │   ├── ProductRepository.cs
│   │   ├── IOrderRepository.cs
│   │   └── OrderRepository.cs
│   │
│   ├── Data/                                  [Database context]
│   │   ├── ApplicationDbContext.cs
│   │   └── Migrations/                        [EF Core migrations]
│
├── GroceryOrderingApp.Mobile/                 [.NET MAUI Android]
│   ├── README.md
│   ├── GroceryOrderingApp.Mobile.csproj
│   ├── App.xaml                               [App configuration]
│   ├── App.xaml.cs
│   ├── AppShell.xaml                          [Navigation]
│   ├── AppShell.xaml.cs
│   ├── MauiProgram.cs                         [Setup & DI]
│   ├── AppConfig.cs                           [Configuration]
│   │
│   ├── Views/                                 [XAML Pages]
│   │   ├── LoginPage.xaml
│   │   ├── LoginPage.xaml.cs
│   │   ├── CustomerCategoryPage.xaml
│   │   ├── CustomerCategoryPage.xaml.cs
│   │   ├── CustomerProductPage.xaml
│   │   ├── CustomerProductPage.xaml.cs
│   │   ├── CartPage.xaml
│   │   ├── CartPage.xaml.cs
│   │   ├── CustomerOrderHistoryPage.xaml
│   │   ├── CustomerOrderHistoryPage.xaml.cs
│   │   ├── CustomerOrderDetailPage.xaml
│   │   ├── CustomerOrderDetailPage.xaml.cs
│   │   ├── AdminDashboardPage.xaml
│   │   ├── AdminDashboardPage.xaml.cs
│   │   ├── AdminOrdersPage.xaml
│   │   ├── AdminOrdersPage.xaml.cs
│   │   ├── AdminOrderDetailPage.xaml
│   │   ├── AdminOrderDetailPage.xaml.cs
│   │   ├── AdminUsersPage.xaml
│   │   ├── AdminUsersPage.xaml.cs
│   │   ├── AdminCategoriesPage.xaml
│   │   ├── AdminCategoriesPage.xaml.cs
│   │   ├── AdminProductsPage.xaml
│   │   └── AdminProductsPage.xaml.cs
│   │
│   ├── ViewModels/                            [MVVM logic]
│   │   ├── BaseViewModel.cs
│   │   ├── LoginViewModel.cs
│   │   ├── CustomerCategoryViewModel.cs
│   │   ├── CustomerProductViewModel.cs
│   │   ├── CartViewModel.cs
│   │   ├── CustomerOrderHistoryViewModel.cs
│   │   ├── CustomerOrderDetailViewModel.cs
│   │   ├── AdminDashboardViewModel.cs
│   │   ├── AdminOrdersViewModel.cs
│   │   └── AdminOrderDetailViewModel.cs
│   │
│   ├── Models/                                [Data models]
│   │   ├── AuthModels.cs
│   │   ├── DataModels.cs
│   │   └── CartModel.cs
│   │
│   ├── Services/                              [Utilities]
│   │   ├── ApiService.cs                      [HTTP client]
│   │   ├── CartService.cs                     [Cart logic]
│   │   └── SecureStorageService.cs            [Token storage]
│   │
│   ├── Converters/                            [XAML value converters]
│   │   └── ValueConverters.cs
│   │
│   ├── Resources/
│   │   └── Styles/
│   │       ├── Styles.xaml
│   │       └── Colors.xaml
│   │
│   ├── Platforms/
│   │   └── Android/
│   │       ├── MainActivity.cs
│   │       └── AndroidManifest.xml
```

---

## 🔑 Key Files Reference

### Backend - Core
| File | Purpose |
|------|---------|
| Program.cs | Application startup & dependency injection |
| DatabaseSeeder.cs | Initial data population |
| appsettings.json | Configuration & connection string |

### Backend - Models
| File | Entities |
|------|----------|
| Role.cs | Admin, Customer roles |
| User.cs | Users table |
| Category.cs | Product categories |
| Product.cs | Products table |
| Order.cs | Orders table |
| OrderItem.cs | Order items table |

### Backend - Controllers
| File | Endpoints |
|------|-----------|
| AuthController.cs | /api/auth/login |
| AdminController.cs | /api/admin/* (users, categories, products, orders) |
| CategoriesController.cs | /api/categories |
| ProductsController.cs | /api/products |
| OrdersController.cs | /api/orders/* |

### Frontend - Core
| File | Purpose |
|------|---------|
| App.xaml | Application resources |
| AppShell.xaml | Navigation structure |
| MauiProgram.cs | Service registration & startup |
| AppConfig.cs | API endpoint & constants |

### Frontend - Views
| File | Screen |
|------|--------|
| LoginPage.xaml | Authentication |
| CustomerCategoryPage.xaml | Browse categories |
| CustomerProductPage.xaml | View products |
| CartPage.xaml | Shopping cart |
| CustomerOrderHistoryPage.xaml | Order history |
| CustomerOrderDetailPage.xaml | Order details |
| AdminDashboardPage.xaml | Admin menu |
| AdminOrdersPage.xaml | Order management |
| AdminOrderDetailPage.xaml | Order details & actions |

---

## 🚀 Quick Links

### Development
- [Setup Instructions](QUICK_START.md)
- [API Testing](API_TESTING_GUIDE.md)
- [Backend README](GroceryOrderingApp.Backend/README.md)
- [Mobile README](GroceryOrderingApp.Mobile/README.md)

### Deployment
- [Deployment Guide](DEPLOYMENT.md)
- [Azure Setup](DEPLOYMENT.md#option-2-azure-app-service-deployment)
- [Google Play Store](GroceryOrderingApp.Mobile/README.md#building-for-google-play-store)

### Documentation
- [Project Overview](README.md)
- [API Endpoints](README.md#-api-endpoints)
- [Architecture](README.md#-architecture)
- [Security](README.md#security-architecture)

### Legal
- [Privacy Policy](PRIVACY_POLICY.md)
- [Terms of Service](TERMS_OF_SERVICE.md)

---

## 📊 File Count Summary

| Component | Files | Type |
|-----------|-------|------|
| Backend Controllers | 5 | .cs |
| Backend Models | 6 | .cs |
| Backend DTOs | 5 | .cs |
| Backend Services | 4 | .cs |
| Backend Repositories | 8 | .cs |
| Backend Config | 3 | .json / .cs |
| Frontend Views | 12 | .xaml / .xaml.cs |
| Frontend ViewModels | 10 | .cs |
| Frontend Services | 3 | .cs |
| Frontend Models | 3 | .cs |
| Converters & Styles | 3 | .cs / .xaml |
| Documentation | 8 | .md |
| **Total** | **~70** | **files** |

---

## 🔍 How to Find Things

### Want to modify API endpoints?
1. Go to: [Controllers Folder](GroceryOrderingApp.Backend/Controllers)
2. Open relevant controller (e.g., AdminController.cs)
3. Add/modify endpoint

### Want to change business logic?
1. Go to: [Services Folder](GroceryOrderingApp.Backend/Services)
2. Open relevant service (e.g., OrderService.cs)
3. Modify logic

### Want to modify UI?
1. Go to: [Views Folder](GroceryOrderingApp.Mobile/Views)
2. Open relevant XAML file
3. Modify layout & binding

### Want to change data model?
1. Go to: [Models Folder](GroceryOrderingApp.Backend/Models)
2. Open relevant model file
3. Create migration: `dotnet ef migrations add MigrationName`
4. Apply: `dotnet ef database update`

### Want to add new field to database?
1. Modify model in [Models Folder](GroceryOrderingApp.Backend/Models)
2. Add migration:
   ```bash
   dotnet ef migrations add AddNewField
   ```
3. Update database:
   ```bash
   dotnet ef database update
   ```

---

## 🎯 Default Credentials

**Admin User:**
- UserId: `admin`
- Password: `Admin@123`

---

## 🔗 Related Documentation

### Main Sections
- [Project Overview](README.md) - Overall description of the app
- [Quick Start](QUICK_START.md) - Get running quickly
- [Features](README.md#-api-endpoints) - What's implemented
- [Architecture](README.md#-architecture) - System design
- [Deployment](DEPLOYMENT.md) - How to deploy

### API Documentation
- [Endpoints Reference](README.md#-api-endpoints)
- [Testing Guide](API_TESTING_GUIDE.md)
- [Backend README](GroceryOrderingApp.Backend/README.md)

### Mobile Documentation
- [Mobile README](GroceryOrderingApp.Mobile/README.md)
- [Building for Play Store](GroceryOrderingApp.Mobile/README.md#building-for-google-play-store)
- [Testing Mobile App](GroceryOrderingApp.Mobile/README.md#testing)

### Compliance
- [Privacy Policy](PRIVACY_POLICY.md)
- [Terms of Service](TERMS_OF_SERVICE.md)

---

## ✅ Setup Checklist

- [ ] Read [QUICK_START.md](QUICK_START.md)
- [ ] Create GroceryOrderingDb database
- [ ] Run backend migrations: `dotnet ef database update`
- [ ] Start backend: `dotnet run`
- [ ] Visit Swagger: https://localhost:7001/swagger
- [ ] Test login endpoint
- [ ] Update API URL in [AppConfig.cs](GroceryOrderingApp.Mobile/AppConfig.cs)
- [ ] Run mobile app: `dotnet maui run -f net8.0-android`
- [ ] Test customer flow
- [ ] Test admin flow
- [ ] Review [DEPLOYMENT.md](DEPLOYMENT.md) for production

---

## 🆘 Troubleshooting

### Build Issues
- See [QUICK_START.md Troubleshooting](QUICK_START.md#-troubleshooting)

### API Issues
- See [API_TESTING_GUIDE.md Error Responses](API_TESTING_GUIDE.md#error-responses)

### Deployment Issues
- See [DEPLOYMENT.md](DEPLOYMENT.md#troubleshooting)

---

**Version**: 1.0  
**Last Updated**: February 20, 2026  
**Navigation Index v1.0**
