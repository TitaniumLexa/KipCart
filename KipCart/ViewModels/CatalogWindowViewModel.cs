using KipCart.Database;
using KipCart.Database.Entities;
using KipCart.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace KipCart.ViewModels
{
    public class CatalogWindowViewModel: INotifyPropertyChanged
    {
        private readonly KipCartContext _context;

        private string _goodNameInput;
        public string GoodNameInput
        {
            get
            {
                return _goodNameInput;
            }
            set
            {
                if (_goodNameInput != value)
                {
                    _goodNameInput = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private readonly ICommand _addGoodCommand;
        public ICommand AddGoodCommand
        {
            get
            {
                return _addGoodCommand;
            }
        }

        private readonly ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                return _backCommand;
            }
        }

        public ObservableCollection<Good> Goods { get; set; }

        public CatalogWindowViewModel(KipCartContext context)
        {
            _context = context;

            _addGoodCommand = new RelayCommand(AddGood);
            _backCommand = new RelayCommand(Save);

            _context.Goods.Load();
            Goods = _context.Goods.Local.ToObservableCollection();

        }

        public void AddGood(object? parameter)
        {
            if (GoodNameInput.Length > 0)
            {
                if (Goods.FirstOrDefault((good) => good.Name == GoodNameInput) is null)
                {
                    Good good = new Good()
                    {
                        Name = GoodNameInput,
                        IsShow = true
                    };
                    Goods.Add(good);

                    _context.SaveChanges();
                }
            }
        }

        public void Save(object? parameter)
        {
            _context.SaveChanges();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
