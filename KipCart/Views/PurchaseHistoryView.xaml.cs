using KipCart.ViewModels;
using System.Windows.Controls;

namespace KipCart.Views
{
    /// <summary>
    /// Логика взаимодействия для PurchaseHistoryView.xaml
    /// </summary>
    public partial class PurchaseHistoryView : UserControl
    {
        public PurchaseHistoryView(PurchaseHistoryViewModel purchaseHistoryViewModel)
        {
            DataContext = purchaseHistoryViewModel;
            InitializeComponent();
        }
    }
}
