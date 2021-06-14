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
        string cardNumber = "";
        public RegisterAccount()
        {
            InitializeComponent();
            fillTextBoxes();
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

        private string GenerateNumber(int range)
        {
            string number = "";
            Random aleatorio = new Random();
            for(int i = 0; i < range; i++)
            {
                number = number + aleatorio.Next(0, 10).ToString();
            }

            return number;
        }

        private void fillTextBoxes()
        {;
            bool acceptedAccount = false;
            bool acceptedCard = false;
            using (BamexContext context = new BamexContext())
            {
                do
                {
                    accounNumber = GenerateNumber(20);
                    int resultAccount = Int32.Parse(accounNumber);
                    var retrievedAccount = context.Cuenta.FirstOrDefault(c => c.CuentaID == resultAccount);
                    if (retrievedAccount == null)
                    {
                        acceptedAccount = true;
                    }

                } while (acceptedAccount != true);

                do
                {
                    cardNumber = "5579" + GenerateNumber(12);
                    int resultCard = Int32.Parse(accounNumber);
                    var retrievedCard = context.Tarjeta.FirstOrDefault(t => t.TarjetaID == resultCard);
                    if (retrievedCard == null)
                    {
                        acceptedCard = true;
                    }
                } while (acceptedCard != true);
            }

            AccountNumberTextBox.Text = accounNumber;
            CardNumberTextBox.Text = cardNumber;
        }
    }
}
