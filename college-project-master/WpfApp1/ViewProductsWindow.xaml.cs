using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
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
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public byte[] ProductImage { get; set; }
        public BitmapImage ImageSource { get; set; }
    }
}
