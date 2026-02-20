# 🎯 Feature Showcase & Usage Guide

## Welcome to Grocery Ordering Application!

This document showcases all features and demonstrates how to use them.

---

## 📱 Application Overview

### For Customers
A clean, simple app to:
1. Browse grocery categories and products
2. Add items to cart
3. Place orders with Cash on Delivery
4. Track order status
5. View order history

### For Admins
A management app to:
1. Manage users
2. Manage product catalog
3. View and process orders
4. Confirm delivery and reduce stock
5. Cancel orders if needed

---

## 🔐 Authentication Features

### How It Works
1. **Login Page** - Enter UserId and Password
2. **JWT Token** - Secure 8-hour authentication
3. **Role Detection** - Admin vs Customer routing
4. **Secure Storage** - Token stored securely on device
5. **Logout** - Clear token and return to login

### Test It
```
Admin Login:
UserId: admin
Password: Admin@123

Customer Login (created via admin):
UserId: customer1
Password: Customer@123
```

### What Happens
- ✅ Successful login → Navigate to respective dashboard
- ❌ Wrong password → "Invalid credentials" error
- ❌ Non-existent user → "Invalid credentials" error

---

## 🛍️ Customer Features

### 1. Browse Categories
**Feature**: View all active product categories

**How to Use**:
1. Login as customer
2. Go to "Categories" tab
3. See list of categories (Vegetables, Fruits, Dairy, etc.)
4. Swipe down to load more

**What to Test**:
- ✅ Categories load from API
- ✅ Only active categories show
- ✅ Loading indicator appears
- ✅ Pull to refresh updates list

---

### 2. Browse Products by Category
**Feature**: View products in selected category

**How to Use**:
1. Select any category
2. See products with:
   - Product name
   - Description  
   - Price (₹)
   - Available stock
3. Add desired products to cart

**What to Test**:
- ✅ Products load correctly
- ✅ Price displays with currency
- ✅ Stock quantity visible
- ✅ Category filter works
- ✅ "Add to Cart" button functional

---

### 3. Shopping Cart Management
**Feature**: Add, update, and remove items from cart

**How to Use**:
1. Browse products
2. Click "Add to Cart"
3. Enter quantity in prompt
4. Go to "Cart" tab
5. See all items with:
   - Product name
   - Unit price
   - Quantity
   - Line total
   - Cart subtotal

**Cart Operations**:
- **Update Quantity**: Click "Update" button
- **Remove Item**: Click "Remove" button
- **Clear Cart**: Automatically clears after successful order

**What to Test**:
- ✅ Items add correctly
- ✅ Quantity updates work
- ✅ Totals calculate correctly
- ✅ Remove removes item
- ✅ Cart persists during session
- ✅ Stock validation works (error if qty > stock)

---

### 4. Place Order
**Feature**: Submit order with items and get confirmation

**How to Use**:
1. Add products to cart
2. Go to Cart tab
3. Review items and total
4. Click "Place Order"
5. See success message
6. Order appears in history

**Order Details Captured**:
- Order ID (auto-generated)
- Order date/time
- Status: **Pending**
- Total amount
- Individual item list
- Price at time of order

**What to Test**:
- ✅ Order requires cart items
- ✅ Stock validation occurs
- ✅ Order total correct
- ✅ Order created with Pending status
- ✅ Cart clears after order
- ✅ Error if item stock insufficient
- ✅ Order date/time correct

**Validation Rules**:
- ❌ Cannot order empty cart
- ❌ Cannot exceed available stock
- ❌ Cannot order from inactive products

---

### 5. View Order History
**Feature**: See all past orders with status

**How to Use**:
1. Go to "Orders" tab
2. See list of all your orders:
   - Order number
   - Order date
   - Status (Pending/Delivered/Cancelled)
   - Total amount
   - Item count
3. Click order to see details

**Status Values**:
- 🔵 **Pending** - Order placed, awaiting Admin confirmation
- ✅ **Delivered** - Order confirmed & delivered, stock reduced
- ❌ **Cancelled** - Order cancelled by Admin

**What to Test**:
- ✅ All customer orders shown
- ✅ Orders sorted by date (newest first)
- ✅ Status displays correctly
- ✅ Amounts show currency
- ✅ Can click to see details

---

### 6. View Order Details
**Feature**: See itemized order information

**How to Use**:
1. Go to Orders tab
2. Tap any order
3. See:
   - Order number
   - Order date
   - Current status
   - Total amount
   - List of items with:
     - Product name
     - Quantity ordered
     - Price per item
     - Line total

**What to Test**:
- ✅ Correct order loaded
- ✅ All items shown
- ✅ Prices match order
- ✅ Status current
- ✅ Total amount correct

---

## 👑 Admin Features

### 1. Admin Dashboard
**Feature**: Main navigation hub

**Options**:
- Manage Users
- Manage Categories
- Manage Products
- View Orders
- Logout

---

### 2. View All Orders
**Feature**: Monitor all customer orders

**How to Use**:
1. Login as admin
2. Go to "Orders" tab
3. See all orders with:
   - Order ID
   - Customer ID (User ID)
   - Order date
   - Order status
   - Total amount
   - Item count
4. Click to see details and take action

**What to Test**:
- ✅ All orders visible
- ✅ Status shows correctly
- ✅ Totals accurate
- ✅ Can click for details
- ✅ Orders sorted by date

---

### 3. Process Orders - Deliver
**Feature**: Confirm delivery and reduce stock

**How to Use**:
1. View order details (click order)
2. See items and total
3. Click "Mark as Delivered"
4. See confirmation message
5. Button becomes disabled
6. Status changes to "Delivered"

**What Happens Behind Scenes**:
- 🔄 Stock quantity reduced for each item by order quantity
- 📦 Stock never goes below 0 (hard limit)
- 📋 Order becomes read-only
- 📊 Order status changes to "Delivered"

**Example**:
```
Product: Tomato
  Before: Stock = 100
  Order Qty: 5
  After: Stock = 95 (reduced by 5)
```

**Validations**:
- ✅ Can only deliver Pending orders
- ✅ Stock cannot go negative
- ✅ All items processed together
- ✅ One-time operation (no undo)

**What to Test**:
- ✅ Deliver button works
- ✅ Status changes to Delivered
- ✅ Button becomes disabled
- ✅ Stock actually reduces
- ✅ Cannot re-deliver same order

---

### 4. Process Orders - Cancel
**Feature**: Cancel orders without stock impact

**How to Use**:
1. View order details
2. Click "Cancel Order"
3. See confirmation
4. Order status → "Cancelled"
5. Button becomes disabled

**What Happens**:
- 📋 Order status changes to "Cancelled"
- 📦 **Stock NOT affected** (unlike Deliver)
- 🔒 Order becomes read-only

**When to Cancel**:
- Customer requests cancellation
- Out of stock items
- Delivery unable to be made
- Payment issues

**What to Test**:
- ✅ Cancel button works
- ✅ Status changes to Cancelled
- ✅ Stock unchanged after cancel
- ✅ Cannot re-cancel
- ✅ Can only cancel Pending orders

---

## 🔄 Key Workflows

### Complete Customer Workflow
```
1. User opens app → Login screen

2. Login → Authenticate

3. Categories tab → Browse categories

4. Select category → View products

5. Add to cart → Prompt quantity

6. Cart tab → Manage items

7. Place order → Create order

8. Success message → Order created

9. Orders tab → View history

10. Select order → See details

11. Logout → Return to login
```

### Complete Admin Workflow
```
1. User opens app → Login screen

2. Login → Dashboard

3. Orders tab → View all orders

4. Select order → See details

5. Mark Delivered → Stock reduces

6. Confirm → Order complete

7. Back to list → See updated status

8. Logout → Return to login
```

---

## 📊 Data Examples

### Sample Products After Seeding
```
Category: Vegetables
- Tomato (₹40) - Stock: 100
- Onion (₹30) - Stock: 150  
- Potato (₹25) - Stock: 200
- Carrot (₹35) - Stock: 80

Category: Fruits
- Apple (₹100) - Stock: 50
- Banana (₹25) - Stock: 120
- Orange (₹50) - Stock: 60
- Mango (₹80) - Stock: 40

Category: Dairy
- Milk 1L (₹50) - Stock: 200
- Yogurt (₹35) - Stock: 100
- Cheese (₹200) - Stock: 30
- Butter (₹150) - Stock: 50
```

### Sample Order
```
Order #1
Order Date: 2026-02-20 10:30:00
Status: Pending
Items:
  - Tomato (₹40) x 2 = ₹80
  - Apple (₹100) x 1 = ₹100
Total: ₹180

After Admin Delivers:
Status: Delivered
Stock Changes:
  - Tomato: 100 → 98
  - Apple: 50 → 49
```

---

## ✅ Testing Scenarios

### Scenario 1: Happy Path (Complete Order)
1. Login as customer
2. Browse Vegetables category
3. Add 2x Tomato to cart
4. Add 1x Onion to cart  
5. Go to Cart
6. Verify total = ₹110 (80 + 30)
7. Place order
8. View in order history
9. See status: Pending
10. Logout and login as admin
11. View order in admin list
12. Mark as Delivered
13. Verify status changed
14. Verify stock reduced (Tomato: 100→98, Onion: 150→149)

### Scenario 2: Stock Validation
1. Login as customer
2. Try to order 200x Tomato (only 100 available)
3. See "Only 100 items available" error
4. Go back and adjust to 100
5. Order succeeds
6. After admin delivers, stock = 0

### Scenario 3: Admin Order Management
1. Login as admin
2. View all orders (should see multiple)
3. Sort oldest to newest
4. Pick any Pending order
5. Deliver it → Status changes
6. Try to deliver again → Button disabled
7. View another order
8. Cancel it → Status changes
9. Try different action → Correct response

### Scenario 4: Cart Management
1. Add multiple items to cart
2. Update quantity of one item
3. Remove one item
4. Verify totals recalculate
5. Clear all items
6. Try to order empty cart → Error
7. Add items again
8. Place order → Cart clears

---

## 🐛 Error Testing

### Expected Errors (And How to Trigger)

#### 1. Invalid Credentials
```
Action: Login with wrong password
Expected: "Invalid credentials" error
```

#### 2. Insufficient Stock
```
Action: Try to order more items than in stock
Expected: "Only X items available" error
```

#### 3. Empty Cart
```
Action: Click Place Order with empty cart
Expected: "Cart is empty" error
```

#### 4. Network Error
```
Action: (Offline) Try any API call
Expected: "Connection failed" message
```

#### 5. Unauthorized Access
```
Action: Customer tries to access admin endpoint
Expected: 403 Forbidden error
```

---

## 🎨 UI Features

### Navigation
- ✅ Tab-based navigation (Customer)
- ✅ Menu-based navigation (Admin)
- ✅ Back button support
- ✅ Deep linking support
- ✅ Logout clears session

### UI Elements
- ✅ Loading spinners on async operations
- ✅ Error messages in red
- ✅ Success messages in green
- ✅ Buttons disable when loading
- ✅ Cards with shadow for products
- ✅ Proper spacing and alignment

### Display Formats
- ✅ Currency with ₹ symbol
- ✅ Date/time in readable format
- ✅ Quantities as numbers
- ✅ Status in uppercase

---

## 🔐 Security Features Demonstrated

### Authentication
- Login with JWT token
- Token expires after 8 hours
- Token stored securely on device
- Logout clears token

### Authorization
- Admin endpoints reject customer token
- Customer cannot access admin APIs
- Role verified on every request
- Endpoints properly protected

### Data Protection
- Passwords hashed before storage
- No sensitive data in logs
- HTTPS for all communication
- Tokens transmitted securely

---

## 📈 Performance Testing

### What to Test
1. **Load Time**
   - App startup: < 3 seconds
   - Page load: < 1 second
   - API response: < 500ms

2. **Memory Usage**
   - Initial load: < 50MB
   - After 10 orders: < 100MB

3. **Network**
   - Works on WiFi
   - Works on 4G/LTE
   - Handles network interruptions

---

## 🎯 Feature Completeness Checklist

### Customer-Facing Features
- [x] Login/Logout
- [x] Category browsing
- [x] Product browsing
- [x] Add to cart
- [x] Update cart quantities
- [x] Remove from cart
- [x] Place order
- [x] View order history
- [x] View order details
- [x] Stock validation
- [x] Error handling

### Admin-Facing Features
- [x] Login/Logout
- [x] View all orders
- [x] View order details
- [x] Deliver orders (stock reduction)
- [x] Cancel orders (no stock change)
- [x] Order status tracking
- [x] Role-based access

### Backend Features
- [x] JWT authentication
- [x] Role-based authorization
- [x] API validation
- [x] Database transactions
- [x] Error handling
- [x] HTTPS support

### Database Features
- [x] User management
- [x] Category management
- [x] Product management
- [x] Order management
- [x] Stock tracking
- [x] Audit fields (CreatedBy, CreatedAt)

---

## 🚀 Next Steps After Testing

1. **Performance Optimization**
   - Add caching where appropriate
   - Optimize database queries
   - Minimize API calls

2. **Enhanced Features** (Future)
   - Search functionality
   - Favorites/Wishlist
   - Edit profile
   - Advanced filtering
   - Order tracking/notifications

3. **Deployment**
   - Follow [DEPLOYMENT.md](DEPLOYMENT.md)
   - Test on real device
   - Deploy to Play Store
   - Monitor production

---

## 📞 Support

If features don't work as expected:
1. Check [QUICK_START.md](QUICK_START.md) troubleshooting
2. Verify API is running: `https://localhost:7001/swagger`
3. Check network connectivity
4. Clear app cache
5. Rebuild and reinstall

---

**Feature Showcase v1.0**  
**Last Updated**: February 20, 2026  
**All Features Complete**: ✅
