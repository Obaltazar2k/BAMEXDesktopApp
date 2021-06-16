using BAMEX.Model;
using BAMEX.Utilities;
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
    /// Lógica de interacción para AccountsConsult.xaml
    /// </summary>
    public partial class AccountsConsult : Page
    {
        private ObservableCollection<AccountConsult> accountsConsult;
        private AccountConsult accountSelected = null;
        public AccountsConsult()
        {
            try
            {
                InitializeComponent();
                GetAccounts();
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

        private void ConsutlMovementsButton_Clicked(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow?.ChangeView(new ConsultMovements(accountSelected.AccountNumber));
            return;
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            accountSelected = (AccountConsult)dataGrid.SelectedItems[0];
            if (accountSelected != null)
            {
                ConsutlMovementsButton.IsEnabled = true;
            }
        }

        private void GetAccounts()
        {
            using (BamexContext context = new BamexContext())
            {
                var accounstList = context.Cuenta
                    .Include(a => a.Cliente).ToList();

                accountsConsult = new ObservableCollection<AccountConsult>();
                foreach(Cuenta account in accounstList)
                {
                    var accountConsult = new AccountConsult
                    {
                        ClientNumber = account.ClienteID,
                        Name = account.Cliente.Nombre,
                        Sournames = account.Cliente.Apellidos,
                        AccountNumber = account.CuentaID,
                        CutDate = account.Fechacorte
                    };

                    accountsConsult.Add(accountConsult);
                }

                AccountsList.ItemsSource = accountsConsult;
                DataContext = accountsConsult;
            }
        }
    }
}
