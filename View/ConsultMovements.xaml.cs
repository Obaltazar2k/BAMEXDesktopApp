using BAMEX.Model;
using BAMEX.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WPFCustomMessageBox;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para ConsultMovements.xaml
    /// </summary>
    public partial class ConsultMovements : Page
    {
        private readonly DateTime thisDay = DateTime.Today;
        private ObservableCollection<AccountStatement> accountStatements;
        private string account = null;

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

        private void GetMovements()
        {
            try
            {
                if (VerificateFields())
                {
                    using (BamexContext context = new BamexContext())
                    {
                        var movementsList = context.Movimiento
                            .Include(m => m.Cargo)
                            .Include(m => m.Abono)
                            .Where(m => m.CuentaID == account && m.Fecha == dpSearchDate.SelectedDate);

                        accountStatements = new ObservableCollection<AccountStatement>();

                        foreach (Movimiento movimiento in movementsList)
                        {
                            var accountStatement = new AccountStatement();
                            accountStatement.Concepto = movimiento.Concepto;
                            accountStatement.Fecha = movimiento.Fecha;
                            if (movimiento.Abono != null)
                            {
                                accountStatement.Monto = movimiento.Abono.Monto;
                                accountStatement.TipoDeMovimiento = "Abono";
                            }
                            else if (movimiento.Cargo != null)
                            {
                                accountStatement.Monto = movimiento.Cargo.Monto;
                                accountStatement.TipoDeMovimiento = "Ingreso";
                            } /*
                            else if(movimiento.Egreso != null)
                            {
                                accountStatement.Monto = movimiento.Egreso.Monto;
                                accountStatement.TipoDeMovimiento = "Egreso";
                            }
                            */
                            accountStatements.Add(accountStatement);
                        }
                        MovementsList.ItemsSource = accountStatements;
                        DataContext = accountStatements;
                    }

                }
            }
            catch (EntityException)
            {
                Restarter.RestarBAMEX();
            }
        }

        private void ConsutlMovementsButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(AccountTextBox.Text.Replace(" ", string.Empty), out _))
            {
                try
                {
                    using (BamexContext context = new BamexContext())
                    {
                        var client = context.Cuenta.Find(AccountTextBox.Text.Replace(" ", string.Empty));
                        if (client != null)
                        {
                            account = AccountTextBox.Text.Replace(" ", string.Empty);
                            ChangeVisibility();
                            GetMovements();
                        }
                        else
                            CustomMessageBox.ShowOK("Número de cuenta no encontrado", "Cuenta no encontrada", "Aceptar");
                    }
                }
                catch (EntityException)
                {
                    Restarter.RestarBAMEX();
                }
            }
            else
                CustomMessageBox.ShowOK("Número de cuenta invalido", "Campos invalidos", "Aceptar");
        }

        private void ChangeVisibility()
        {
            ConsutlMovementsButton.Visibility = Visibility.Collapsed;
            AccountTextBox.Visibility = Visibility.Collapsed;
            BlurEffect.Radius = 0;
        }

        private void AccountTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab || e.Key == Key.Space)
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
