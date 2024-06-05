using System.Collections.Generic;
using System.Windows;
using WpfApp1.db;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class AdminWindow : Window
    {
        private readonly UserService _userService;

        public AdminWindow(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
            LoadUsers();
        }

        private void LoadUsers()
        {
            List<User> users = _userService.GetAllUsers();
            UsersDataGrid.ItemsSource = users;
        }

        private void RegisterUserButton_Click(object sender, RoutedEventArgs e)
        {
            var registerUserWindow = new RegisterUserWindow(true);
            registerUserWindow.ShowDialog();
            LoadUsers(); 
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is int userId)
            {
                _userService.DeleteUser(UserService.LoggedInUserRole, userId);
                LoadUsers();
            }
        }
    }
}
