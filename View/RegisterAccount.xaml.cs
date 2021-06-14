using BAMEX.Model;
using BAMEX.Utilities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFCustomMessageBox;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para RegisterAccount.xaml
    /// </summary>
    public partial class RegisterAccount : Page
    {
        string accounNumber = "";
        string cardNumber = "5579";
        public RegisterAccount()
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

        private void RegistButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (VerificateFields())
            {
                CustomMessageBox.ShowOK("La cuenta se ha registrado exitosamente", "Registro exitoso", "Aceptar");
                BackIcon_Clicked(new object(), new RoutedEventArgs());
            }
            
        }

        private void NumbersTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private bool VerificateFields()
        {
            return FieldsVerificator.VerificateString(NameTextBox.Text, "Nombre")
                && FieldsVerificator.VerificateString(SournamesTextBox.Text, "Apellidos")
                && FieldsVerificator.VerificateDate(dpBirthDate.Text, "Fecha de nacimiento")
                && FieldsVerificator.VerificateString(CountryTextBox.Text, "País")
                && FieldsVerificator.VerificateString(StateTextBox.Text, "Estado")
                && FieldsVerificator.VerificateString(CityTextBox.Text, "Ciudad/Municipio")
                && FieldsVerificator.VerificateDate(dpCutDate.Text, "Fecha de corte")
                && FieldsVerificator.VerificateDate(dpExpirationDate.Text, "Fecha de expiración");
        }

        private string GenerateNumber(string number, int range)
        {
            Random aleatorio = new Random();
            for(int i = 0; i < range; i++)
            {
                number = number + aleatorio.Next(0, 10).ToString();
            }

            return number;
        }

        private void fillTextBoxes()
        {
            string account;
            string card;
            bool acceptedAccount = false;
            bool acceptedCard = false;
            using (BamexContext context = new BamexContext())
            {
                do
                {
                    account = GenerateNumber(accounNumber, 20);
                    var client = context.Cliente.FirstOrDefault(c => c);
                } while (acceptedAccount != true);
            }
        }
    }
}
