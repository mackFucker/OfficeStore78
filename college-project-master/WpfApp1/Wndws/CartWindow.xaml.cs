using System.Collections.Generic;
using System.Windows;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class CartWindow : Window
    {
        private readonly ProductService _productService;

        public CartWindow(ProductService productService)
        {
            InitializeComponent();
            _productService = productService;
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            List<CartItem> cartItems = _productService.GetCartItems();
            CartDataGrid.ItemsSource = cartItems;
            TotalPriceTextBlock.Text = _productService.GetTotalPrice().ToString("C");
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Purchase successful!");
            _productService.ClearCart();
            LoadCartItems();
        }

        private void ClearCartButton_Click(object sender, RoutedEventArgs e)
        {
            _productService.ClearCart();
            LoadCartItems();
        }
    }
}
