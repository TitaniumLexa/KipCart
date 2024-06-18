using KipCart.Database.Entities;
using KipCart.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace KipCart.Views
{
    /// <summary>
    /// Логика взаимодействия для PurchaseView.xaml
    /// </summary>
    public partial class PurchaseView : UserControl
    {
        public PurchaseView(PurchaseViewModel purchaseViewModel)
        {
            DataContext = purchaseViewModel;
            InitializeComponent();
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(nameof(Good)))
            {
                Good? good = e.Data.GetData(nameof(Good)) as Good;
                if (good is not null && DataContext is PurchaseViewModel context)
                {
                    context.AddToPurchase(good);
                }
            }
        }
    }
}
