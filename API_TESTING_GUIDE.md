# API Testing Guide - Postman Collection

## Setup
1. Download [Postman](https://www.postman.com)
2. Import this collection
3. Set baseUrl variable: `https://localhost:7001`

## Collections

### 1. Authentication
```
POST /api/auth/login
Body:
{
  "userId": "admin",
  "password": "Admin@123"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR...",
  "role": "Admin",
  "userId": 1
}
```

Save token from response. Use in Authorization header for protected endpoints:
```
Bearer <token>
```

---

### 2. Admin - User Management

#### Create User
```
POST /api/admin/users
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "userId": "customer1",
  "password": "Pass@123",
  "role": "Customer"
}

Response: 201 Created
{
  "id": 2,
  "userId": "customer1",
  "role": "Customer",
  "createdAt": "2026-02-20T10:00:00",
  "isActive": true
}
```

#### Get All Users
```
GET /api/admin/users
Authorization: Bearer <admin-token>

Response: 200 OK
[
  {
    "id": 1,
    "userId": "admin",
    "role": "Admin",
    "createdAt": "2026-02-20T09:00:00",
    "isActive": true,
    "createdBy": null
  },
  ...
]
```

---

### 3. Admin - Category Management

#### Create Category
```
POST /api/admin/categories
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "name": "Electronics"
}

Response: 201 Created
{
  "id": 6,
  "name": "Electronics",
  "isActive": true
}
```

#### Update Category
```
PUT /api/admin/categories/6
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "name": "Electronics & Gadgets",
  "isActive": true
}

Response: 200 OK
{
  "id": 6,
  "name": "Electronics & Gadgets",
  "isActive": true
}
```

---

### 4. Admin - Product Management

#### Create Product
```
POST /api/admin/products
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "name": "Organic Lettuce",
  "description": "Fresh organic lettuce",
  "price": 45.50,
  "stockQuantity": 100,
  "categoryId": 1
}

Response: 201 Created
{
  "id": 19,
  "name": "Organic Lettuce",
  "description": "Fresh organic lettuce",
  "price": 45.50,
  "stockQuantity": 100,
  "categoryId": 1,
  "isActive": true
}
```

#### Update Product
```
PUT /api/admin/products/19
Authorization: Bearer <admin-token>
Content-Type: application/json

{
  "name": "Organic Lettuce",
  "description": "Fresh organic green lettuce",
  "price": 50.00,
  "stockQuantity": 150,
  "categoryId": 1,
  "isActive": true
}

Response: 200 OK
```

---

### 5. Customer - Browsing

#### Get Active Categories
```
GET /api/categories
Authorization: Not required

Response: 200 OK
[
  {
    "id": 1,
    "name": "Vegetables",
    "isActive": true
  },
  {
    "id": 2,
    "name": "Fruits",
    "isActive": true
  },
  ...
]
```

#### Get Products by Category
```
GET /api/products?categoryId=1
Authorization: Not required

Response: 200 OK
[
  {
    "id": 1,
    "name": "Tomato",
    "description": "Fresh red tomato",
    "price": 40.00,
    "stockQuantity": 100,
    "categoryId": 1,
    "isActive": true
  },
  ...
]
```

---

### 6. Customer - Ordering

#### Create Order
```
POST /api/orders
Authorization: Bearer <customer-token>
Content-Type: application/json

{
  "items": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 5,
      "quantity": 3
    }
  ]
}

Response: 201 Created
{
  "id": 1,
  "userId": 2,
  "orderDate": "2026-02-20T10:30:00",
  "status": "Pending",
  "totalAmount": 200.00,
  "items": [
    {
      "id": 1,
      "productId": 1,
      "productName": "Tomato",
      "quantity": 2,
      "priceAtTime": 40.00
    },
    {
      "id": 2,
      "productId": 5,
      "productName": "Apple",
      "quantity": 3,
      "priceAtTime": 100.00
    }
  ]
}
```

#### Get My Orders
```
GET /api/orders/my
Authorization: Bearer <customer-token>

Response: 200 OK
[
  {
    "id": 1,
    "userId": 2,
    "orderDate": "2026-02-20T10:30:00",
    "status": "Pending",
    "totalAmount": 200.00,
    "items": [...]
  }
]
```

#### Get Order Detail
```
GET /api/orders/1
Authorization: Bearer <customer-token>

Response: 200 OK
{
  "id": 1,
  "userId": 2,
  "orderDate": "2026-02-20T10:30:00",
  "status": "Pending",
  "totalAmount": 200.00,
  "items": [...]
}
```

---

### 7. Admin - Order Management

#### Get All Orders
```
GET /api/admin/orders
Authorization: Bearer <admin-token>

Response: 200 OK
[
  {
    "id": 1,
    "userId": 2,
    "orderDate": "2026-02-20T10:30:00",
    "status": "Pending",
    "totalAmount": 200.00,
    "items": [...]
  }
]
```

#### Get Order Detail
```
GET /api/admin/orders/1
Authorization: Bearer <admin-token>

Response: 200 OK
{
  "id": 1,
  "userId": 2,
  "orderDate": "2026-02-20T10:30:00",
  "status": "Pending",
  "totalAmount": 200.00,
  "items": [...]
}
```

#### Mark Order as Delivered
```
PUT /api/admin/orders/1/deliver
Authorization: Bearer <admin-token>

Response: 200 OK
{
  "message": "Order delivered successfully"
}

Side Effects:
- Stock reduced for all items
- Order status → "Delivered"
- Order becomes read-only
```

#### Cancel Order
```
PUT /api/admin/orders/1/cancel
Authorization: Bearer <admin-token>

Response: 200 OK
{
  "message": "Order cancelled successfully"
}

Side Effects:
- Stock unchanged
- Order status → "Cancelled"
- Order becomes read-only
```

---

## Error Responses

### 400 Bad Request
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "Invalid input"
}
```

### 401 Unauthorized
```json
{
  "detail": "Invalid credentials"
}
```

### 403 Forbidden
```json
{
  "detail": "Insufficient permissions"
}
```

### 404 Not Found
```json
{
  "detail": "Resource not found"
}
```

### 500 Internal Server Error
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "An error occurred",
  "status": 500,
  "detail": "Internal server error"
}
```

---

## Test Scenarios

### Scenario 1: Happy Path (Customer Order)
1. Login as customer → Get token
2. Get categories → List active categories
3. Get products by category → List products
4. Create order → Place order with items
5. Get my orders → View order history
6. Get order detail → View order items

### Scenario 2: Admin Processing
1. Login as admin → Get token
2. Get all orders → View all placed orders
3. Get order detail → View customer order
4. Deliver order → Reduce stock and mark delivered
5. Verify stock reduction → Check product stock

### Scenario 3: Error Handling
1. Create order with invalid product → 400 error
2. Create order with insufficient stock → 400 error
3. Access customer order as different customer → 404 error
4. Access admin endpoint as customer → 403 error
5. Login with wrong password → 401 error

---

## Performance Testing

### Load Test (Apache JMeter)
```
10 concurrent users
100 requests per user
Target: < 500ms response time
```

### Database Query Performance
```
Indexes on:
- Users.UserId
- Orders.UserId
- OrderItems.OrderId
- Products.CategoryId
```

---

## Security Testing

### Checklist
- [ ] HTTPS only (no HTTP)
- [ ] JWT token validation
- [ ] Role-based access control
- [ ] Input validation
- [ ] SQL injection prevention
- [ ] Password hashing verification
- [ ] CORS properly configured

---

## Import to Postman

Create collection with these requests:
1. Save each API call
2. Use variables for token and baseUrl
3. Add pre-request scripts for token refresh
4. Add tests for response validation
5. Generate API documentation

---

**Version**: 1.0  
**Last Updated**: February 2026  
**API Version**: 1.0
