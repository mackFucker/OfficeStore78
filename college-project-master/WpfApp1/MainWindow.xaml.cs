using System.Windows;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;

        public MainWindow(UserService userService, ProductService productService)
        {
            InitializeComponent();
            _userService = userService;
            _productService = productService;
            ConfigureUIBasedOnRole();
        }

        private void ConfigureUIBasedOnRole()
        {
            if (UserService.LoggedInUserRole != "Admin" && UserService.LoggedInUserRole != "Manager")
            {
                AddProductButton.IsEnabled = false;
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addProductWindow = new AddProductWindow(_productService);
            addProductWindow.ShowDialog();
        }

        private void ViewProductsButton_Click(object sender, RoutedEventArgs e)
        {
            ViewProductsWindow viewProductsWindow = new ViewProductsWindow(_productService);
            viewProductsWindow.ShowDialog();
        }
    }
}
