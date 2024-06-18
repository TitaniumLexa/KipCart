using KipCart.Database;
using KipCart.Database.Entities;
using KipCart.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

namespace KipCart.ViewModels
{
    public class PurchaseHistoryViewModel : INotifyPropertyChanged
    {
        private readonly KipCartContext _context;
        private readonly IMessagesService _messagesService;

        public ObservableCollection<Purchase> Purchases { get; set; }

        private Purchase? _selectedPurchase;
        public Purchase? SelectedPurchase
        {
            get
            {
                return _selectedPurchase;
            }
            set
            {
                if (_selectedPurchase != value)
                {
                    _selectedPurchase = value;
                    NotifyPropertyChanged();
                    _messagesService.SendMessage<Purchase?>("PurchaseHistorySelected", _selectedPurchase);
                }
            }
        }

        private readonly ICommand _newPurchaseCommand;
        public ICommand NewPurchaseCommand
        {
            get
            {
                return _newPurchaseCommand;
            }
        }

        public PurchaseHistoryViewModel(KipCartContext cartContext, IMessagesService messagesService)
        {
            _context = cartContext;
            _messagesService = messagesService;

            _newPurchaseCommand = new RelayCommand(NewPurchase);

            _context.Purchases
                .Include(p => p.PurchaseGoods)
                .Load();
            Purchases = _context.Purchases.Local.ToObservableCollection();

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Purchases);
            collectionView.SortDescriptions.Add(new SortDescription(nameof(Purchase.Date), ListSortDirection.Descending));
        }
        private void NewPurchase(object? _)
        {
            SelectedPurchase = null;
            _messagesService.SendMessage<Purchase?>("NewPurchase", null);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
