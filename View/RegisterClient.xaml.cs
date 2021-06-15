using BAMEX.Model;
using BAMEX.Utilities;
using System.Data.Entity.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFCustomMessageBox;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para RegisterClient.xaml
    /// </summary>
    public partial class RegisterClient : Page
    {
        public RegisterClient()
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
            try
            {
                if (VerificateFields())
                {
                    using (BamexContext context = new BamexContext())
                    {
                        var client = context.Cliente.Find(ClientNumberTextBox.Text);
                        if (client == null)
                        {
                            var clientRegistered = RegistNewClient();
                            CustomMessageBox.ShowOK("El cliente con numero: " + clientRegistered.ClienteID + " se ha registrado exitosamente", "Registro exitoso", "Aceptar");
                            BackIcon_Clicked(new object(), new RoutedEventArgs());
                        }
                        else
                        {
                            CustomMessageBox.ShowOK("Ya existe un cliente con el numero:" + client.ClienteID, "Estudiante ya registrado", "Aceptar");
                        }
                    }
                }
            }
            catch (EntityException)
            {
                Restarter.RestarBAMEX();
            }
        }

        private Cliente RegistNewClient()
        {
            var client = new Cliente
            {
                Nombre = NameTextBox.Text,
                Apellidos = SournamesTextBox.Text,
                Dirección = AddressTextBox.Text,
                Contrasenia = Encrypt.GetSHA256(PasswordTextBox.Password),
                ClienteID = ClientNumberTextBox.Text
            };
            using (BamexContext context = new BamexContext())
            {
                context.Cliente.Add(client);
                context.SaveChanges();
            }
            return client;
        }

        private bool VerificateFields()
        {
            return FieldsVerificator.VerificateString(NameTextBox.Text, "Nombre")
                && FieldsVerificator.VerificateString(SournamesTextBox.Text, "Apellidos");
        }

        private void NumbersTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
