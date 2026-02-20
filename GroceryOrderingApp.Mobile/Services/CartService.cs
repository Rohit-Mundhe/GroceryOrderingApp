using GroceryOrderingApp.Mobile.Models;

namespace GroceryOrderingApp.Mobile.Services
{
    public interface ICartService
    {
        List<CartItem> GetCartItems();
        void AddToCart(ProductDto product, int quantity);
        void UpdateCartQuantity(int productId, int quantity);
        void RemoveFromCart(int productId);
        void ClearCart();
        decimal GetCartTotal();
    }

    public class CartService : ICartService
    {
        private readonly List<CartItem> _cartItems = new();

        public List<CartItem> GetCartItems()
        {
            return _cartItems.ToList();
        }

        public void AddToCart(ProductDto product, int quantity)
        {
            var existingItem = _cartItems.FirstOrDefault(c => c.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }
        }

        public void UpdateCartQuantity(int productId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                    _cartItems.Remove(item);
                else
                    item.Quantity = quantity;
            }
        }

        public void RemoveFromCart(int productId)
        {
            var item = _cartItems.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
                _cartItems.Remove(item);
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }

        public decimal GetCartTotal()
        {
            return _cartItems.Sum(c => c.TotalPrice);
        }
    }
}
