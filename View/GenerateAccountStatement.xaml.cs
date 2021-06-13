using BAMEX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using WPFCustomMessageBox;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para GenerateAccountStatement.xaml
    /// </summary>
    public partial class GenerateAccountStatement : Page
    {
        public GenerateAccountStatement()
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

        private void DownloadButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void MovementsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private bool VerificateFields()
        {
            return FieldsVerificator.VerificateDate(dpInitialDate.Text, "Fecha de inicio")
            && FieldsVerificator.VerificateDate(dpFinalDate.Text, "Fecha de fin");
        }
    }
}
