using System.Windows;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly ProductService _productService;

        public MainWindow()
        {
            InitializeComponent();
            _productService = new ProductService();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProductWindow(_productService);
            addProductWindow.Show();
        }

        private void ViewProductsButton_Click(object sender, RoutedEventArgs e)
        {
            var viewProductsWindow = new ViewProductsWindow(_productService);
            viewProductsWindow.Show();
        }

        private void ViewCartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow(_productService);
            cartWindow.Show();
        }
    }
}
