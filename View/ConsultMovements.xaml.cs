using BAMEX.Utilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPFCustomMessageBox;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para ConsultMovements.xaml
    /// </summary>
    public partial class ConsultMovements : Page
    {
        public ConsultMovements()
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

        private void ConsutlDetailsButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void MovementsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private bool VerificateFields()
        {
            return FieldsVerificator.VerificateDate(dpSearchDate.Text, "Fecha");
        }
    }
}
