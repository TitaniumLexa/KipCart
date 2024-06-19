using KipCart.Models;
using KipCart.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace KipCart.ViewModels
{
    public class CatalogWindowViewModel: INotifyPropertyChanged
    {
        private readonly GoodsModel _goodsModel;

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

        public ObservableCollection<GoodModel> Goods { get; set; }

        public CatalogWindowViewModel(GoodsModel goodsModel)
        {
            _goodsModel = goodsModel;

            _addGoodCommand = new RelayCommand(AddGood);
            _backCommand = new RelayCommand(Save);

            Goods = _goodsModel.GetGoodModels();

        }

        public void AddGood(object? parameter)
        {
            if (GoodNameInput.Length > 0)
            {
                if (Goods.FirstOrDefault((good) => good.Name == GoodNameInput) is null)
                {
                    _goodsModel.AddNewGood(GoodNameInput);
                }
            }
        }

        public void Save(object? parameter)
        {
            _goodsModel.Save();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
