using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class AddProductWindow : Window
    {
        private readonly ProductService _productService;
        private byte[] _productImage;

        public AddProductWindow(ProductService productService)
        {
            InitializeComponent();
            _productService = productService;
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            string productName = ProductNameTextBox.Text;
            string productDescription = ProductDescriptionTextBox.Text;
            if (!decimal.TryParse(ProductPriceTextBox.Text, out decimal productPrice))
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            if (_productService.AddProduct(productName, productDescription, _productImage, productPrice))
            {
                MessageBox.Show("Product added successfully!");
                Close();
            }
        }

        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _productImage = File.ReadAllBytes(openFileDialog.FileName);
                ProductImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
