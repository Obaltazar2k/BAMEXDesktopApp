using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WPFCustomMessageBox;
using System.Data.Entity.Core;
using BAMEX.Utilities;
using BAMEX.Model;
using System.Linq;

namespace BAMEX.View
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private static int ALLOWEDATTEMTPS = 5;
        private int _attempts = 0;
        private bool _lockedLogin = false;
        private string _user, _password;
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FieldsValidation() && _lockedLogin == false)
                {
                    GetDataFromFields();
                    var client = IsClient();
                    if (client != null)
                    {
                        SetSesion(client.ClienteID, true);
                        var mainWindow = (MainWindow)Application.Current.MainWindow;
                        mainWindow?.ChangeView(new SelectAccount());
                        return;
                    }
                    else
                    {
                        var manager = IsManager();
                        if (manager != null)
                        {
                            SetSesion(manager.GerenteID, false);
                            var mainWindow = (MainWindow)Application.Current.MainWindow;
                            mainWindow?.ChangeView(new MainMenu());
                            return;
                        }
                        else
                        {
                            var cashier = IsCashier();
                            if(cashier != null)
                            {
                                SetSesion(cashier.CajeroID, false);
                                var mainWindow = (MainWindow)Application.Current.MainWindow;
                                mainWindow?.ChangeView(new RegisterMovement());
                                return;
                            }
                            else
                            {
                                FailedAttempt();
                            }
                        }
                    }
                }
            }
            catch (EntityException)
            {
                Restarter.RestarBAMEX();
            }
        }

        private bool FieldsValidation()
        {
            if (!string.IsNullOrEmpty(UserTextBox.Text) && !string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                Regex rgx = new Regex(@"^[a-zA-Z0-9]+$");
                if (rgx.IsMatch(UserTextBox.Text))
                    return true;
                else
                {
                    CustomMessageBox.Show("Campos erróneos. Por favor asegurese de introducir datos alfanumericos en usuario.");
                    UserTextBox.Clear();
                    PasswordTextBox.Clear();
                    return false;
                }
            }
            else
            {
                CustomMessageBox.Show("Campos incompletos. Por favor asegurese de no dejar campos vacíos.");
                return false;
            }
        }

        private void GetDataFromFields()
        {
            _user = UserTextBox.Text;
            _password = Encrypt.GetSHA256(PasswordTextBox.Password);
        }

        private Cliente IsClient()
        {
            using (BamexContext context = new BamexContext())
            {
                var client = context.Cliente.FirstOrDefault(c => c.ClienteID == _user);
                if ((client != null) && client.Contrasenia == _password)
                {
                    return client;
                }
                else
                    return null;
            }
        }

        private Gerente IsManager()
        {
            using (BamexContext context = new BamexContext())
            {
                var manager = context.Gerente.FirstOrDefault(c => c.GerenteID == _user);
                if ((manager != null) && manager.Contrasenia == _password)
                {
                    return manager;
                }
                else
                    return null;
            }
        }

        private Cajero IsCashier()
        {
            using (BamexContext context = new BamexContext())
            {
                var cashier = context.Cajero.FirstOrDefault(c => c.CajeroID == _user);
                if ((cashier != null) && cashier.Contrasenia == _password)
                {
                    return cashier;
                }
                else
                    return null;
            }
        }

        private void SetSesion(string user, bool isClient)
        {
            Sesion userSesion = Sesion.GetSesion;
            userSesion.Username = user;
            userSesion.IsClient = isClient;
        }

        private void FailedAttempt()
        {
            CustomMessageBox.Show("Usuario no encontrado. Por favor verifique que sus datos sean correctos.");
            UserTextBox.Clear();
            PasswordTextBox.Clear();
            _attempts++;
            if (_attempts == ALLOWEDATTEMTPS)
            {
                _lockedLogin = true;
                LoginButton.IsEnabled = false;
                CustomMessageBox.Show("Ah sobrepasado el numero de intentos disponibles, intente mas tarde");
            }
        }
    }
}
