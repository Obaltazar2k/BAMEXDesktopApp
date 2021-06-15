using BAMEX.Model;
using BAMEX.Utilities;
using System;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Cliente> ClientsCollection { get; set; }
        public RegisterAccount()
        {
            InitializeComponent();
            fillFields();
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
                using (BamexContext context = new BamexContext())
                {
                    RegistNewAccount();
                    CustomMessageBox.ShowOK("La cuenta se ha registrado exitosamente", "Registro exitoso", "Aceptar");
                    BackIcon_Clicked(new object(), new RoutedEventArgs());
                }
            }
            
        }

        private void RegistNewAccount()
        {
            Sesion userSesion = Sesion.GetSesion;

                var selectedClient = (Cliente)ClientComboBox.SelectedItem;
                var account = new Cuenta
                {
                    Fechacorte = dpCutDate.SelectedDate,
                    Montoinicial = float.Parse(InitialAmountTextBox.Text),
                    Saldo = float.Parse(BalanceTextBox.Text),
                    GerenteID = userSesion.Username,
                    CuentaID = AccountNumberTextBox.Text,
                    ClienteID = selectedClient.ClienteID
                };

                var card = new Tarjeta
                {
                    Fechaexpiracion = dpExpirationDate.SelectedDate,
                    TarjetaID = CardNumberTextBox.Text,
                    Pincode = Int32.Parse(PinTextBox.Password),
                    CuentaID = AccountNumberTextBox.Text,
                    Nombreentarjeta = selectedClient.Nombre + " " + selectedClient.Apellidos
                };

            using (BamexContext context = new BamexContext())
            {
                context.Cuenta.Add(account);
                context.Tarjeta.Add(card);
                context.SaveChanges();
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
            return FieldsVerificator.VerificateDate(dpCutDate.Text, "Fecha de corte")
                && FieldsVerificator.VerificateDate(dpExpirationDate.Text, "Fecha de expiración")
                && FieldsVerificator.VerificateDecimal(InitialAmountTextBox.Text, "Monto inicial")
             && FieldsVerificator.VerificateDecimal(BalanceTextBox.Text, "Saldo");
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

        private void fillFields()
        {;
            bool acceptedAccount = false;
            bool acceptedCard = false;
            ClientsCollection = new ObservableCollection<Cliente>();
            using (BamexContext context = new BamexContext())
            {
                do
                {
                    accounNumber = GenerateNumber(20);
                    var retrievedAccount = context.Cuenta.FirstOrDefault(c => c.CuentaID == accounNumber);
                    if (retrievedAccount == null)
                    {
                        acceptedAccount = true;
                    }

                } while (acceptedAccount != true);

                do
                {
                    cardNumber = "5579" + GenerateNumber(12);
                    var retrievedCard = context.Tarjeta.FirstOrDefault(t => t.TarjetaID == cardNumber);
                    if (retrievedCard == null)
                    {
                        acceptedCard = true;
                    }
                } while (acceptedCard != true);

                CVVTextBox.Text = GenerateNumber(3);

                var clientsList = context.Cliente.OrderByDescending(c => c.Nombre + c.Apellidos);
                if (clientsList != null)
                {
                    foreach (Cliente client in clientsList)
                    {
                        if (client != null)
                            ClientsCollection.Add(client);
                    }
                }
            }
            ClientComboBox.ItemsSource = ClientsCollection;
            AccountNumberTextBox.Text = accounNumber;
            CardNumberTextBox.Text = cardNumber;
        }
    }
}
