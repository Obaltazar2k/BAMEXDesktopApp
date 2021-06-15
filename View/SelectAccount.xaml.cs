using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPFCustomMessageBox;
using System.Data.Entity;
using System.Linq;
using BAMEX.Model;
using BAMEX.Utilities;
using System.Collections.ObjectModel;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para SelectAccount.xaml
    /// </summary>
    public partial class SelectAccount : Page
    {
        private ObservableCollection<AccountInfoForClient> clientAccounts;
        private AccountInfoForClient accountSelected = null;
        Sesion userSesion = Sesion.GetSesion;
        public SelectAccount()
        {
            InitializeComponent();
            GetAccounts();
        }

        private void BackIcon_Clicked(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                CustomMessageBox.ShowOK("No hay entrada a la cual volver.", "Error al navegar hacía atrás", "Aceptar");
        }

        private void GeneratAccountStatementButton_Clicked(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow?.ChangeView(new GenerateAccountStatement(accountSelected.AccountNumber));
            return;

        }

        private void GetAccounts()
        {          
            using (BamexContext context = new BamexContext())
            {
                var accounstList = context.Cuenta
                    .Include(a => a.Tarjeta)
                    .Where(a => a.ClienteID.Equals(userSesion.Username));
                    

                clientAccounts = new ObservableCollection<AccountInfoForClient>();
                foreach (Cuenta account in accounstList)
                {
                    var tarjeta = account.Tarjeta.First();
                    var accountInfo = new AccountInfoForClient
                    {
                        AccountNumber = account.CuentaID,
                        Balance = account.Saldo,
                        CardNumber = tarjeta.TarjetaID,
                        CutDate = account.Fechacorte
                    };

                    clientAccounts.Add(accountInfo);
                }

                AccountsList.ItemsSource = clientAccounts;
                DataContext = clientAccounts;
            }
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            accountSelected = (AccountInfoForClient)dataGrid.SelectedItems[0];
            if (accountSelected != null)
            {
                GeneratAccountStatementButton.IsEnabled = true;
            }
        }
    }
}
