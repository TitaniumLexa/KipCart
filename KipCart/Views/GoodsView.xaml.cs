using KipCart.Database.Entities;
using KipCart.ViewModels;
using System.Windows;
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
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.Source is FrameworkElement element && element.DataContext is Good good)
                {
                    var data = new DataObject();
                    data.SetData(nameof(Good), good);
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
        }
    }
}
