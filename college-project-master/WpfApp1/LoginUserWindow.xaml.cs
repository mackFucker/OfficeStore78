using System.Windows;
using System.Windows.Navigation;
using WpfApp1.db;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordBox.Password;

            UserService userService = new UserService();
            userService.LoginUser(email, password);
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