using System.Windows;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly ProductService _productService;
        private readonly UserService _userService;

        public MainWindow()
        {
            InitializeComponent();
            _productService = new ProductService();
            _userService = new UserService();
            ConfigureUIBasedOnRole();

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

        private void ConfigureUIBasedOnRole()
        {
            if (UserService.LoggedInUserRole == "Admin")
            {
                // Show Admin button or automatically open AdminWindow
                AdminButton.Visibility = Visibility.Visible;
            }
            else
            {
                AdminButton.Visibility = Visibility.Collapsed;
                AddProductButton.IsEnabled = UserService.LoggedInUserRole == "Manager";
            }
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow(_userService);
            adminWindow.ShowDialog();
        }

    }
}
