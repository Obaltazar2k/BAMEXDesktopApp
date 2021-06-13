using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPFCustomMessageBox;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para AccountsConsult.xaml
    /// </summary>
    public partial class AccountsConsult : Page
    {
        public AccountsConsult()
        {
            InitializeComponent();
        }

        private void BackIcon_Clicked(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                CustomMessageBox.ShowOK("No hay entrada a la cual volver.", "Error al navegar hacía atrás", "Aceptar");
        }

        private void ConsutlMovementsButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
