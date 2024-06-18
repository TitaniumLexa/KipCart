using KipCart.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace KipCart.Views
{
    /// <summary>
    /// Логика взаимодействия для GoodsView.xaml
    /// </summary>
    public partial class GoodsView : UserControl
    {
        public GoodsView(GoodsViewModel goodsViewModel)
        {
            DataContext = goodsViewModel;
            InitializeComponent();
        }

        private void ListElement_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
