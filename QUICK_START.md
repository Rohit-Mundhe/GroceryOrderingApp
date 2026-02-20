# Quick Start Guide - Grocery Ordering App

## ⚡ 5-Minute Setup

### Prerequisites
- Windows 10/11 with .NET 8.0 SDK installed
- SQL Server Express (free version)
- Visual Studio 2022 OR VS Code

### Step 1: Download & Extract (1 min)
```bash
# Navigate to project folder
cd "e:\Rohit_Mundhe\WOrk\Test"
```

### Step 2: Setup Database (1 min)
```bash
# Open SQL Server Management Studio and run:
CREATE DATABASE GroceryOrderingDb;

# Or use SQL Server command line:
sqlcmd -S (local)\SQLEXPRESS -Q "CREATE DATABASE GroceryOrderingDb;"
```

### Step 3: Run Backend API (1 min)
```bash
cd GroceryOrderingApp.Backend
dotnet restore
dotnet ef database update
dotnet run
```

**Expected Output:**
```
info: Microsoft.AspNetCore.Server.Kestrel[35]
      Application started. Press Ctrl+C to exit.
listen: https://localhost:7001
```

✅ **Open in browser**: https://localhost:7001/swagger

### Step 4: Login & Test (1 min)
```
UserId: admin
Password: Admin@123
```

1. Click "Try it out" on POST /api/auth/login
2. Enter credentials
3. Copy token from response

### Step 5: Run Mobile App (Optional - 1 min)
```bash
cd ../GroceryOrderingApp.Mobile
dotnet restore
dotnet workload install maui
dotnet maui run -f net8.0-android
```

---

## 🔐 Default Logins

### Admin User
```
UserId: admin
Password: Admin@123
```

### Create Customer
Use Admin API: POST `/api/admin/users`
```json
{
  "userId": "customer1",
  "password": "Customer@123",
  "role": "Customer"
}
```

---

## 🧪 Quick Test Workflow

### Using Swagger (Recommended)
1. Go to: https://localhost:7001/swagger
2. Click any endpoint
3. "Try it out"
4. Fill in request body
5. "Execute"

### Sample Workflow
1. **Login** → Get token
2. **Create Customer** (use admin token)
3. **Get Categories** (no auth needed)
4. **Get Products** by category (no auth needed)
5. **Create Order** (use customer token)
6. **View Order** (use customer token)
7. **Deliver Order** (use admin token)

---

## 📱 Mobile App Testing

### First Run
1. Ensure backend is running
2. Update API URL in: `GroceryOrderingApp.Mobile/Services/ApiService.cs`
   ```csharp
   BaseAddress = new Uri("https://localhost:7001")
   ```
3. Run: `dotnet maui run -f net8.0-android`
4. Login with admin or customer credentials

### Test Scenario
1. **Login** as customer
2. **Browse categories** 
3. **Select category** → View products
4. **Add products** to cart
5. **Go to cart** → Review items
6. **Place order** 
7. **View order history**
8. **Logout**

---

## 🐛 Troubleshooting

### "Cannot connect to database"
```
Solution:
1. Verify SQL Server is running: Services app
2. Check connection string in appsettings.json
3. Run: netstat -an | findstr 1433
```

### "API not accessible from mobile"
```
Solution:
1. For emulator: Use https://10.0.2.2:7001
2. For device: Use actual machine IP (ipconfig)
3. Ensure firewall allows port 7001
```

### "MAUI workload not found"
```
Solution:
dotnet workload install maui
dotnet workload restore
```

### "Port 7001 already in use"
```
Solution:
# Find process using port
netstat -ano | findstr :7001

# Kill process
taskkill /PID <PID> /F

# Or use different port in launchSettings.json
```

---

## 📚 Next Steps

### For Development
1. Read [Backend README](GroceryOrderingApp.Backend/README.md)
2. Read [Mobile README](GroceryOrderingApp.Mobile/README.md)
3. Check [API Endpoints](README.md#-api-endpoints)

### For Deployment
1. Read [Deployment Guide](DEPLOYMENT.md)
2. Create Azure resources
3. Configure credentials
4. Build & publish

### For Testing
1. Create multiple users
2. Test order workflow
3. Try admin delivery feature
4. Test error scenarios

---

## 🎯 Feature Checklist

### Backend ✅
- [x] JWT authentication
- [x] Role-based authorization
- [x] User management
- [x] Category management
- [x] Product management
- [x] Order management
- [x] Stock reduction on delivery
- [x] API documentation (Swagger)
- [x] Error handling
- [x] Database migrations

### Frontend ✅
- [x] Login page
- [x] Category listing
- [x] Product listing
- [x] Shopping cart
- [x] Order placement
- [x] Order history
- [x] Order details
- [x] Admin dashboard
- [x] Admin order management
- [x] Local storage

---

## 📞 Common Commands

```bash
# Backend
dotnet run                          # Run with watch mode
dotnet watch run                    # Auto-reload on file change
dotnet ef database update           # Apply migrations
dotnet ef migrations add InitialCreate  # Create migration
dotnet test                         # Run tests

# Mobile
dotnet maui run -f net8.0-android  # Run on emulator
dotnet maui build -f net8.0-android # Build APK
dotnet publish -f net8.0-android -c Release # Build for store
```

---

## 🚀 Performance Tips

### Backend
- Use connection pooling
- Implement caching for categories/products
- Use async/await everywhere
- Profile with Azure Application Insights

### Mobile
- Lazy load order lists
- Cache API responses
- Minimize network calls
- Use local storage for cart

---

## 📊 Database Reset

To start fresh:
```bash
# Delete and recreate database
# In SQL Server Management Studio:
DROP DATABASE GroceryOrderingDb;

# Or via command line:
sqlcmd -S (local)\SQLEXPRESS -Q "DROP DATABASE GroceryOrderingDb;"

# Then run migrations again:
cd GroceryOrderingApp.Backend
dotnet ef database update
```

---

## ✅ Production Readiness

Before going live:
- [ ] Update JWT secret to secure value
- [ ] Change default admin password
- [ ] Configure Azure resources
- [ ] Enable HTTPS with valid certificate
- [ ] Set up monitoring/logging
- [ ] Create database backups
- [ ] Test with actual users
- [ ] Document deployment process

---

## 📞 Support

For issues:
1. Check Troubleshooting section above
2. Review [README.md](README.md)
3. Check logs: `dotnet run` output
4. Check Azure portal for cloud deployments

---

**Version**: 1.0  
**Last Updated**: February 2026  
**Difficulty Level**: Beginner  
**Estimated Setup Time**: 5 minutes
