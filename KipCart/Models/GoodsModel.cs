using KipCart.Database;
using KipCart.Database.Entities;
using KipCart.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace KipCart.Models
{
    // To-Do: требуется скорректировать название, т.к. это скорее репозиторий, а не модель
    public class GoodsModel
    {
        private readonly KipCartContext _context;
        private ObservableCollection<GoodModel> _goodModels;

        public GoodsModel(KipCartContext context)
        {
            _context = context;
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
    }
}
