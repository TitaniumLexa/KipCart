using KipCart.Database;
using KipCart.Database.Entities;
using KipCart.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace KipCart.ViewModels
{
    public class PurchaseViewModel : INotifyPropertyChanged
    {
        private readonly KipCartContext _context;
        private readonly IMessagesService _messagesService;

        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return _saveCommand; } }

        private readonly ICommand _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand;
            }
        }

        private Purchase? _purchase;
        public ObservableCollection<PurchaseGood> PurchaseGoods { get; set; }

        private DateTime? _dateInput;
        public DateTime? DateInput
        {
            get
            {
                return _dateInput;
            }
            set
            {
                if (_dateInput != value)
                {
                    _dateInput = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public PurchaseViewModel(KipCartContext context, IMessagesService messagesService)
        {
            _context = context;
            _messagesService = messagesService;

            _messagesService.Register<Good>("AddToPurchase", this, AddToPurchase);
            _messagesService.Register<Purchase?>("PurchaseHistorySelected", this, SelectPurchase);
            _messagesService.Register<Purchase?>("NewPurchase", this, (_) => ResetPurchase());

            _resetCommand = new RelayCommand((_) => Reset());
            _saveCommand = new RelayCommand((_) => Save());

            PurchaseGoods = new ObservableCollection<PurchaseGood>();
        }

        public void AddToPurchase(Good good)
        {
            var purchaseGood = new PurchaseGood { Good = good, GoodAmount = 0, GoodPrice = 0, GoodID = good.ID };
            PurchaseGoods.Add(purchaseGood);
        }

        private void SelectPurchase(Purchase? purchase)
        {
            if (purchase == null)
            {
                ResetPurchase();
            }
            else if (_purchase != null && _purchase.ID != purchase.ID || _purchase == null)
            {
                _purchase = purchase;
                PurchaseGoods = new ObservableCollection<PurchaseGood>(purchase.PurchaseGoods);
                NotifyPropertyChanged(nameof(PurchaseGoods));
                DateInput = purchase.Date;
            }

        }

        private void ResetPurchase()
        {
            DateInput = null;
            _purchase = null;
            PurchaseGoods.Clear();
        }

        public void Save()
        {
            if (DateInput == null)
                return;

            if (_purchase == null)
            {
                _purchase = new Purchase() { Date = (DateTime)DateInput };

                using var transaction = _context.Database.BeginTransaction();

                _context.Purchases.Add(_purchase);
                _context.SaveChanges();

                _context.PurchasesGoods.AddRange(PurchaseGoods.Select((purchaseGood) => { purchaseGood.Purchase = _purchase; return purchaseGood; }));
                _context.SaveChanges();

                transaction.Commit();
            }
            else
            {
                _purchase.Date = (DateTime)DateInput;
                _context.SaveChanges();
            }
        }

        public void Reset()
        {
            if (_purchase == null)
            {
                ResetPurchase();
            }
            else
            {
                _context.Entry(_purchase).Reload();
                foreach (var purchaseGood in PurchaseGoods)
                {
                    _context.Entry(purchaseGood).Reload();
                }                
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
