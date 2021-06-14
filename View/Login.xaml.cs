using System.Windows;
using System.Windows.Controls;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Clicked(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow?.ChangeView(new MainMenu());
            return;
        }

        // Cuando un cliente inicia sesión debe mandarlo directo a su estado de cuenta
        private void ClientLogIn(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow?.ChangeView(new GenerateAccountStatement("")); // <- Mandar el número de cuenta con el que inicia sesión
            return;
        }
    }
}
