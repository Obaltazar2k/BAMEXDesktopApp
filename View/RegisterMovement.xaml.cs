using BAMEX.Model;
using BAMEX.Utilities;
using System;
using System.Data.Entity.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WPFCustomMessageBox;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para RegisterMovement.xaml
    /// </summary>
    public partial class RegisterMovement : Page
    {
        private readonly DateTime thisDay = DateTime.Today;
        Sesion userSesion = Sesion.GetSesion;

        public RegisterMovement()
        {
            InitializeComponent();
            DateTextBox.Text = thisDay.ToString("d");
        }

        private void BackIcon_Clicked(object sender, RoutedEventArgs e)
        {
            Sesion.CloseSesion();
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
                    MessageBoxResult confirmation = CustomMessageBox.ShowYesNo("¿Desea realizar el movimiento?", "Confirmación", "Si", "No");
                    if (confirmation == MessageBoxResult.Yes)
                    {
                        using (BamexContext context = new BamexContext())
                        {
                            var account = context.Cuenta.Find(AccountTextBox.Text.Replace(" ", string.Empty));
                            var movement = new Movimiento
                            {
                                Concepto = ConceptTextBox.Text,
                                Fecha = thisDay,
                                Cuenta = account,
                                CajeroID = Sesion.GetSesion.Username
                            };
                            if ((bool)EntryRadioButton.IsChecked)
                            {
                                movement.Cargo = new Cargo
                                {
                                    Monto = double.Parse(AmountTextBox.Text),
                                    Movimiento = movement
                                };
                                account.Saldo += movement.Cargo.Monto;
                            }
                            else if ((bool)PayRadioButton.IsChecked)
                            {
                                movement.Abono = new Abono()
                                {
                                    Monto = double.Parse(AmountTextBox.Text),
                                    Movimiento = movement
                                };
                                account.Saldo += movement.Abono.Monto;
                                context.Abono.Add(movement.Abono);
                            }
                            else if ((bool)EgressRadioButton.IsChecked)
                            {
                                account.Saldo -= float.Parse(AmountTextBox.Text);
                            }

                            var registMovement = new Movimiento
                            {
                                Concepto = movement.Concepto,
                                Fecha = movement.Fecha,
                                CajeroID = movement.CajeroID,
                                CuentaID = AccountTextBox.Text
                            };

                            context.Movimiento.Add(registMovement);
                            context.SaveChanges();

                            CustomMessageBox.ShowOK("Se ha registrado el movimiento con éxito", "Éxito", "Aceptar");
                            AccountTextBox.Clear();
                            ConceptTextBox.Clear();
                            AmountTextBox.Clear();
                        }
                    }
                    else
                        return;
                }
            }
            catch (EntityException)
            {
                Restarter.RestarBAMEX();
            }
        }

        private bool VerificateFields()
        {
            return VerificateAmount();
        }

        private bool VerificateAmount()
        {
            if (double.TryParse(AmountTextBox.Text, out _))
                return true;
            else
            {
                CustomMessageBox.ShowOK("Cantidad invalida", "Campos invalidos", "Aceptar");
                return false;
            }
        }

        private void AmountTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab || e.Key == Key.OemComma || e.Key == Key.OemPeriod)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void AccountTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab || e.Key == Key.Space)
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
