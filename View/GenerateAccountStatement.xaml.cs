using BAMEX.Model;
using BAMEX.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPFCustomMessageBox;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para GenerateAccountStatement.xaml
    /// </summary>
    public partial class GenerateAccountStatement : Page
    {
        private readonly DateTime thisDay = DateTime.Today;
        private ObservableCollection<AccountStatement> accountStatements;
        private int account = 0;

        public GenerateAccountStatement(int numberAccount)
        {
            account = numberAccount;
            InitializeComponent();
            SetDates();
            GetMovements();
        }

        private void SetDates()
        {
            dpInitialDate.SelectedDate = thisDay.AddMonths(-1);
            dpInitialDate.SelectedDate = thisDay;
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
                            .Where(m => m.CuentaID == account && m.Fecha >= dpInitialDate.SelectedDate && m.Fecha <= dpFinalDate.SelectedDate);

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
                            else if(movimiento.Cargo != null)
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

        private void BackIcon_Clicked(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                CustomMessageBox.ShowOK("No hay entrada a la cual volver.", "Error al navegar hacía atrás", "Aceptar");
        }

        private void DownloadButton_Clicked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private bool VerificateFields()
        {
            return FieldsVerificator.VerificateDate(dpInitialDate.Text, "Fecha de inicio")
            && FieldsVerificator.VerificateDate(dpFinalDate.Text, "Fecha de fin");
        }

        private void dpInitialDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            accountStatements.Clear();
            GetMovements();

        }

        private void dpFinalDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            accountStatements.Clear();
            GetMovements();
        }
    }
}
