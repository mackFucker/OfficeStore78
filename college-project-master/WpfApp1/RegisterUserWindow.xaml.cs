using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class RegisterUserWindow : Window
    {
        private UserService userService;

        public RegisterUserWindow()
        {
            InitializeComponent();
            userService = new UserService();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            bool isRegistered = userService.RegisterUser(email, firstName, lastName, password, "Manager");
            if (isRegistered)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
            e.Handled = true;
        }
    }

}