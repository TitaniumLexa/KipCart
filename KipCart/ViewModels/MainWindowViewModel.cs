using KipCart.Views;

namespace KipCart.ViewModels
{
    public class MainWindowViewModel
    {
        /// <summary>
        /// Представление товаров
        /// </summary>
        public GoodsView GoodsView { get; private set; }

        /// <summary>
        /// Представление покупки
        /// </summary>
        public PurchaseView PurchaseView { get; private set; }

        /// <summary>
        /// Представление истории покупок
        /// </summary>
        public PurchaseHistoryView PurchaseHistoryView { get; private set; }

        public MainWindowViewModel(GoodsView goodsView, PurchaseView purchaseView, PurchaseHistoryView purchaseHistoryView)
        {
            GoodsView = goodsView;
            PurchaseView = purchaseView;
            PurchaseHistoryView = purchaseHistoryView;
        }
    }
}
