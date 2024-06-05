using System.Windows;
using System.Windows.Navigation;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;

        public LoginWindow()
        {
            InitializeComponent();
            _userService = new UserService();
            _productService = new ProductService();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordBox.Password;

            bool isLoggedIn = _userService.LoginUser(email, password);
            if (isLoggedIn)
            {
                var mainWindow = new MainWindow(_userService, _productService);
                mainWindow.Show();
                this.Close();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var registerUserWindow = new RegisterUserWindow();
            registerUserWindow.Show();
            this.Close();
            e.Handled = true;
        }
    }
}
