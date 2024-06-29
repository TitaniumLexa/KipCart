using KipCart.Models;
using KipCart.Services;
using KipCart.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace KipCart.ViewModels
{
    public class GoodsViewModel : INotifyPropertyChanged
    {
        private readonly GoodsModel _goodsModel;
        private readonly IMessagesService _messagesService;
        private CatalogWindow _catalogWindow;

        public ObservableCollection<GoodModel> Goods
        {
            get
            {
                return _goodsModel.Goods;
            }
            set
            {
                if (_goodsModel.Goods != value)
                {
                    _goodsModel.Goods = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ICollectionView _filteredView;
        public ICollectionView FilteredView
        {
            get
            {
                return _filteredView;
            }
            set
            {
                if (_filteredView != value)
                {
                    _filteredView = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public GoodModel? SelectedGood { get; set; }

        private readonly ICommand _addToPurchaseCommand;
        public ICommand AddToPurchaseCommand
        {
            get
            {
                return _addToPurchaseCommand;
            }
        }

        private readonly ICommand _openCatalogWindowCommand;
        public ICommand OpenCatalogWindowCommand
        {
            get
            {
                return _openCatalogWindowCommand;
            }
        }

        public GoodsViewModel(GoodsModel goodsModel, IMessagesService messagesService)
        {
            _goodsModel = goodsModel;
            _messagesService = messagesService;

            _goodsModel.PropertyChanged += GoodsModel_PropertyChanged;

            _addToPurchaseCommand = new RelayCommand(AddToPurchase);
            _openCatalogWindowCommand = new RelayCommand(OpenCatalogWindow);

            InitializeFilteredView();
        }

        private void GoodsModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_goodsModel.Goods):
                    InitializeFilteredView();
                    break;
            }
        }

        private void InitializeFilteredView()
        {
            var collectionViewsSource = new CollectionViewSource { Source = Goods, IsLiveFilteringRequested = true };
            collectionViewsSource.LiveFilteringProperties.Add(nameof(GoodModel.Show));

            FilteredView = collectionViewsSource.View;
            FilteredView.Filter = element =>
            {
                var good = element as GoodModel;
                return good?.Show ?? false;
            };
        }

        private void AddToPurchase(object? parameter)
        {
            if (SelectedGood != null)
            {
                _messagesService.SendMessage("AddToPurchase", SelectedGood);
            }
        }
        private void OpenCatalogWindow(object? parameter)
        {
            _catalogWindow ??= new CatalogWindow(new CatalogWindowViewModel(_goodsModel));
            _catalogWindow.Closed += CatalogWindow_Closed;
            _catalogWindow.Show();
        }

        private void CatalogWindow_Closed(object? sender, EventArgs e)
        {
            _catalogWindow.Closed -= CatalogWindow_Closed;
            _catalogWindow = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
