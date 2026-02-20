# Deployment Guide - Grocery Ordering Application

## 📋 Deployment Checklist

### Pre-Deployment
- [ ] All code reviewed
- [ ] Tests passed
- [ ] API endpoints documented
- [ ] Database migrations tested
- [ ] Security audit completed
- [ ] Credentials changed from defaults

### Backend Deployment

## Option 1: Local IIS Deployment

### Prerequisites
- Windows Server with IIS installed
- .NET 8.0 Hosting Bundle
- SQL Server instance
- SSL certificate

### Steps
```bash
# 1. Publish backend
cd GroceryOrderingApp.Backend
dotnet publish -c Release -o C:\iis\grocery-api

# 2. Create IIS Application Pool
# In IIS Manager:
# - New App Pool: "GroceryAPI" (No Managed Code)
# - Set .NET version to No Managed Code

# 3. Create IIS Website
# - Physical path: C:\iis\grocery-api
# - Hostname: api.groceryapp.com
# - HTTPS: Yes (with valid certificate)

# 4. Update appsettings.json
# Connection string and JWT secret via environment variables

# 5. Create web.config
# Ensure aspNetCore module configured
```

## Option 2: Azure App Service Deployment

### Prerequisites
- Azure subscription
- Resource group created
- Azure SQL Database created
- Azure Key Vault (optional but recommended)

### Steps

#### 1. Create Azure Resources
```bash
# Create resource group
az group create --name GroceryApp --location eastus

# Create App Service Plan (Basic tier)
az appservice plan create \
    --name GroceryAppPlan \
    --resource-group GroceryApp \
    --sku B1 \
    --is-linux

# Create Web App
az webapp create \
    --resource-group GroceryApp \
    --plan GroceryAppPlan \
    --name grocery-api \
    --runtime "DOTNETCORE|8.0"

# Create SQL Server
az sql server create \
    --resource-group GroceryApp \
    --name grocery-sql-server \
    --admin-user sqladmin \
    --admin-password 'P@ssw0rd!Secure123'

# Create SQL Database
az sql db create \
    --resource-group GroceryApp \
    --server grocery-sql-server \
    --name GroceryOrderingDb \
    --edition Basic
```

#### 2. Configure Connection String
```bash
# Get connection string
az sql db show-connection-string \
    --server grocery-sql-server \
    --name GroceryOrderingDb \
    --client ado.net

# Set in App Service
az webapp config appsettings set \
    --resource-group GroceryApp \
    --name grocery-api \
    --settings "ConnectionStrings__DefaultConnection=Server=tcp:grocery-sql-server.database.windows.net,1433;Initial Catalog=GroceryOrderingDb;Persist Security Info=False;User ID=sqladmin;Password=P@ssw0rd!Secure123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

#### 3. Configure JWT Settings
```bash
# Set JWT secret
az webapp config appsettings set \
    --resource-group GroceryApp \
    --name grocery-api \
    --settings "Jwt__Secret=SuperSecureKeyAtLeast32CharactersLongForJWT1"

# Set other JWT settings
az webapp config appsettings set \
    --resource-group GroceryApp \
    --name grocery-api \
    --settings \
        "Jwt__Issuer=GroceryOrderingApp" \
        "Jwt__Audience=GroceryOrderingAppUsers"
```

#### 4. Publish to Azure
```bash
# Install Azure CLI tools
dotnet tool install -g Microsoft.Azure.Coree.Tools

# Publish directly
cd GroceryOrderingApp.Backend
dotnet publish -c Release -o ./publish

# Deploy using zipdeploy
$file = Get-ChildItem -Path "./publish" -Recurse | Compress-Archive -DestinationPath "app.zip"
az webapp deployment source config-zip \
    --resource-group GroceryApp \
    --name grocery-api \
    --src app.zip
```

#### 5. Enable HTTPS
```bash
# Azure automatically provides HTTPS
# Verify in portal: grocery-api.azurewebsites.net
```

#### 6. Verify Deployment
```bash
# Check health
curl https://grocery-api.azurewebsites.net/swagger

# Monitor logs
az webapp log tail --resource-group GroceryApp --name grocery-api
```

### Frontend Deployment

## Google Play Store Submission

### Prerequisites
- Google Play Developer Account ($25)
- Signed APK/AAB
- App Store listing assets
- Privacy Policy
- Support email

### Steps

#### 1. Build Signed AAB
```bash
cd GroceryOrderingApp.Mobile

# Generate keystore (one-time)
keytool -genkey -v -keystore grocery.keystore \
    -keyalg RSA \
    -keysize 2048 \
    -validity 10000 \
    -alias grocerykey

# Update .csproj with signing info
# Build signed AAB
dotnet publish -f net8.0-android -c Release
```

#### 2. Create Play Store Listing
1. Go to [Google Play Console](https://play.google.com/console)
2. Create new app
3. Fill in:
   - **App Name**: Grocery Ordering
   - **Default Language**: English
   - **App Category**: Shopping
   - **App Type**: Free

#### 3. Add App Assets
- **App Icon**: 512x512 PNG
- **Feature Graphic**: 1024x500 PNG
- **Screenshots**: 3-5 images per device type (phone)
- **Privacy Policy**: URL from your website

#### 4. Add App Description
```
Short Description (50 chars):
"Order fresh groceries online with easy delivery"

Full Description (4000 chars):
"Grocery Ordering App - Order fresh groceries online with easy cash-on-delivery payment. Browse from various categories including vegetables, fruits, dairy, and more. Track your orders in real-time.

Features:
• Easy login with secure authentication
• Browse categories and products
• Add items to cart
• Place orders with cash-on-delivery
• Track order status
• View order history

Supported Categories:
• Vegetables
• Fruits  
• Dairy Products
• Grains
• Spices

Download now and start ordering fresh groceries!"
```

#### 5. Content Rating
- Complete content rating questionnaire
- Declare appropriate rating

#### 6. Pricing & Distribution
- Price: Free
- Countries: Select target countries
- Device requirement: Android 12.0+

#### 7. Upload AAB
1. Go to "Release" section
2. Create new release
3. Upload AAB from: `bin/Release/net8.0-android/publish/*.aab`
4. Add release notes

#### 8. Submit for Review
- Review all information
- Agree to policies
- Submit for publication

**Timeline**: Usually 24-48 hours for review

### Android Emulator Testing

```bash
# List available devices
emulator -list-avds

# Run on emulator
dotnet maui run -f net8.0-android --device "emulator-5554"

# Test checklist:
☑ Login flow
☑ Category browsing
☑ Product listing
☑ Add to cart
☑ Place order
☑ Order history
☑ Admin features
☑ Network error handling
```

### Server Configuration

## Environment Variables

### Backend
```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<your-connection-string>
Jwt__Secret=<your-secure-secret>
Jwt__Issuer=GroceryOrderingApp
Jwt__Audience=GroceryOrderingAppUsers
```

### SSL/TLS Certificate
```bash
# Azure automatically manages SSL
# For on-prem: use Let's Encrypt
# Certificate must be valid for API domain
```

## Monitoring & Logging

### Azure Application Insights
```bash
# Enable Application Insights
az webapp config appsettings set \
    --resource-group GroceryApp \
    --name grocery-api \
    --settings "APPINSIGHTS_INSTRUMENTATIONKEY=<key>"

# Monitor dashboards in Azure portal
```

### Log Analysis
```bash
# Real-time logs
az webapp log tail --resource-group GroceryApp --name grocery-api

# Download logs
az webapp log download --resource-group GroceryApp --name grocery-api
```

## Database Backup

### Azure SQL Backup
```bash
# Azure handles daily backups automatically
# Retention: 7 days for Basic tier
# Configure geo-redundancy for disaster recovery

# Manual backup
az sql db copy \
    --resource-group GroceryApp \
    --server grocery-sql-server \
    --name GroceryOrderingDb \
    --dest-name GroceryOrderingDb-backup
```

## Performance Optimization

### Backend Caching
- Implement Redis for session caching (optional)
- Database query optimization
- Connection pooling
- Async/await throughout

### Mobile App
- Lazy loading lists
- Image caching
- Minimize API calls
- Local storage for cart

## Security Hardening

### Post-Deployment
1. Change default admin password
2. Enable Azure DDoS protection
3. Configure Azure Firewall
4. Enable audit logging
5. Review security recommendations
6. Enable Azure Defender

```bash
# Update admin password (via API)
curl -X POST https://api.groceryapp.com/api/admin/users \
    -H "Authorization: Bearer <admin-token>" \
    -H "Content-Type: application/json" \
    -d '{"userId":"newadmin","password":"NewSecurePassword123!","role":"Admin"}'
```

## Cost Optimization

### Production Estimated Costs
```
Azure App Service Basic B1:     ~$13/month
Azure SQL Database Basic:       ~$5/month
Static IP (optional):           ~$2/month
Storage (backups):              ~$1/month
────────────────────────────────
Total:                          ~$21/month
```

### Cost Reduction Tips
- Use Basic tier for low traffic
- Scheduled auto-shutdown for dev environments
- Reserved instances for committed use
- Monitor usage and scale as needed

## Rollback Plan

### If Deployment Fails
```bash
# Azure: Swap to previous deployment
az webapp deployment slot swap \
    --resource-group GroceryApp \
    --name grocery-api \
    --slot staging

# Or restore from backup
az sql db restore \
    --resource-group GroceryApp \
    --name GroceryOrderingDb-backup \
    --dest-name GroceryOrderingDb
```

## Post-Deployment Verification

### Health Checks
```bash
# API health
curl https://api.groceryapp.com/swagger

# Database connectivity
curl -X POST https://api.groceryapp.com/api/auth/login \
    -H "Content-Type: application/json" \
    -d '{"userId":"admin","password":"Admin@123"}'

# Expected: 200 OK with token in response
```

### Performance Baselines
- API response time: < 500ms
- Database query: < 200ms
- Mobile app startup: < 3 seconds
- Order placement: < 2 seconds

## Support & Escalation

### Incident Response
1. Check Application Insights for errors
2. Review logs for root cause
3. Implement fix
4. Test in staging
5. Deploy to production
6. Monitor for 24 hours

### Contact Information
- Support Email: support@groceryapp.com
- On-call: Developer team
- Escalation: Development manager

---

**Deployment Version**: 1.0  
**Last Updated**: February 2026  
**Next Review**: April 2026
