using System.Windows;
using System.Windows.Controls;
using BAMEX.View;

namespace BAMEX
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
            Loaded += OnMainWindowLoaded;
        }

        public void ChangeView(Page view)
        {
            FrameContent.NavigationService.Navigate(view);
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            ChangeView(new Login());
        }
    }
}
