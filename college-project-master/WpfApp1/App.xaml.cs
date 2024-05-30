using System.Windows;
using WpfApp1.Service;

namespace WpfApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UserService userService = new UserService();
            ProductService productService = new ProductService();

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
