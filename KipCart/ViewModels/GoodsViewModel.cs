using KipCart.Database;
using KipCart.Database.Entities;
using KipCart.Services;
using KipCart.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace KipCart.ViewModels
{
    public class GoodsViewModel
    {
        private readonly KipCartContext _context;
        private readonly IMessagesService _messagesService;
        private CatalogWindow _catalogWindow;

        public ObservableCollection<Good> Goods { get; set; }
        public ICollectionView FilteredView { get; set; }
        public Good? SelectedGood { get; set; }

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

        public GoodsViewModel(KipCartContext context, IMessagesService messagesService)
        {
            _context = context;
            _messagesService = messagesService;

            _addToPurchaseCommand = new RelayCommand(AddToPurchase);
            _openCatalogWindowCommand = new RelayCommand(OpenCatalogWindow);

            _context.Goods.Load();
            Goods = _context.Goods.Local.ToObservableCollection();

            var collectionViewsSource = new CollectionViewSource { Source = Goods, IsLiveFilteringRequested = true };
            collectionViewsSource.LiveFilteringProperties.Add(nameof(Good.Show));

            FilteredView = collectionViewsSource.View;
            FilteredView.Filter = element =>
            {
                var good = element as Good;
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
            _catalogWindow ??= new CatalogWindow(new CatalogWindowViewModel(_context));
            _catalogWindow.Closed += _catalogWindow_Closed;
            _catalogWindow.Show();
        }

        private void _catalogWindow_Closed(object? sender, EventArgs e)
        {
            _catalogWindow.Closed -= _catalogWindow_Closed;
            _catalogWindow = null;
        }
    }
}
