using KipCart.ViewModels;
using System.Windows;

namespace KipCart.Views
{
    /// <summary>
    /// Логика взаимодействия для CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        public CatalogWindow(CatalogWindowViewModel catalogWindowViewModel)
        {
            DataContext = catalogWindowViewModel;
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
