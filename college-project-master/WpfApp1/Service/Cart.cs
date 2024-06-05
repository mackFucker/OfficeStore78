using System.Collections.Generic;
using WpfApp1.db;

namespace WpfApp1.Service
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public decimal TotalPrice => Product.ProductPrice * Quantity;
    }

    public class Cart
    {
        private readonly List<CartItem> _items;

        public Cart()
        {
            _items = new List<CartItem>();
        }

        public void AddToCart(Product product, int quantity)
        {
            var cartItem = _items.Find(item => item.Product.ProductID == product.ProductID);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItem(product, quantity));
            }
        }

        public void RemoveFromCart(Product product)
        {
            _items.RemoveAll(item => item.Product.ProductID == product.ProductID);
        }

        public List<CartItem> GetCartItems()
        {
            return _items;
        }

        public decimal GetTotalPrice()
        {
            decimal total = 0;
            foreach (var item in _items)
            {
                total += item.TotalPrice;
            }
            return total;
        }

        public void ClearCart()
        {
            _items.Clear();
        }
    }
}
