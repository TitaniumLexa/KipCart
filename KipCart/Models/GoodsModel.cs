using KipCart.Database;
using KipCart.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace KipCart.Models
{
    public class GoodsModel : INotifyPropertyChanged
    {
        private readonly KipCartContext _context;
        private ObservableCollection<GoodModel> _goodModels = new ObservableCollection<GoodModel>();
        private Task _initializeTask;
        public ObservableCollection<GoodModel> Goods
        {
            get
            {
                return _goodModels;
            }
            set
            {
                if (_goodModels != value)
                {
                    _goodModels = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public GoodsModel(IDbContextFactory<KipCartContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext(); // Long living context, not good

            _initializeTask = GetGoodModelsAsync();
        }

        public ObservableCollection<GoodModel> GetGoodModels()
        {
            if (_goodModels is null)
            {
                _context.Goods.Load();
                var goodsCollection = _context.Goods.Local.ToObservableCollection();

                _goodModels = new ObservableCollection<GoodModel>(goodsCollection.Select(good => new GoodModel(good)));
                goodsCollection.CollectionChanged += (sender, args) =>
                {
                    if (args.NewItems?.Count == 1)
                    {
                        if (args.NewItems[0] is Good good)
                        {
                            _goodModels.Add(new GoodModel(good));
                        }
                    }

                    if (args.OldItems?.Count == 1)
                    {
                        _goodModels.Remove(_goodModels.First(goodModel => goodModel.Good == args.OldItems[0]));
                    }
                };
            }

            return _goodModels;
        }

        public async Task GetGoodModelsAsync()
        {
            var goodsCollection = _context.Goods.Local.ToObservableCollection();

            Goods = new ObservableCollection<GoodModel>(goodsCollection.Select(good => new GoodModel(good)));
            goodsCollection.CollectionChanged += (sender, args) =>
            {
                if (args.NewItems?.Count == 1)
                {
                    if (args.NewItems[0] is Good good)
                    {
                        Application.Current.Dispatcher.Invoke(() => Goods.Add(new GoodModel(good)));
                    }
                }

                if (args.OldItems?.Count == 1)
                {
                    var goodToRemove = Goods.First(goodModel => goodModel.Good == args.OldItems[0]);

                    Application.Current.Dispatcher.Invoke(() => Goods.Remove(goodToRemove));

                }
            };

            await _context.Goods.LoadAsync();
        }

        public void AddNewGood(string name, bool isShow = true)
        {
            var newGood = new GoodModel(name, isShow);

            _context.Goods.Add(newGood.Good);
            _context.SaveChanges();
        }

        public void Save()
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
