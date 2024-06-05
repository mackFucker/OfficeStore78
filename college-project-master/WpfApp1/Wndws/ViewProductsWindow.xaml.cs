using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfApp1.db;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class ViewProductsWindow : Window
    {
        private readonly ProductService _productService;

        public ViewProductsWindow(ProductService productService)
        {
            InitializeComponent();
            _productService = productService;
            LoadProducts();
        }

        private void LoadProducts()
        {
            List<Product> products = _productService.GetAllProducts();
            foreach (var product in products)
            {
                if (product.ProductImage != null && product.ProductImage.Length > 0)
                {
                    product.ImageSource = LoadImage(product.ProductImage);
                }
            }
            ProductsDataGrid.ItemsSource = products;
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            using (var stream = new System.IO.MemoryStream(imageData))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is Product product)
            {
                _productService.AddToCart(product, 1);
                MessageBox.Show($"{product.ProductName} added to cart.");
            }
        }
    }
}
