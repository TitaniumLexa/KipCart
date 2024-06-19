using KipCart.Database;
using KipCart.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KipCart.Models
{
    public class GoodModel : INotifyPropertyChanged
    {
        private Good _good;
        public Good Good
        {
            get
            {
                return _good;
            }
        }

        public string Name
        {
            get
            {
                return _good.Name;
            }
            set
            {
                if (_good.Name != value)
                {
                    _good.Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Show
        {
            get
            {
                return _good.IsShow;
            }
            set
            {
                if (_good.IsShow != value)
                {
                    _good.IsShow = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public GoodModel(Good good)
        {
            _good = good;
        }

        public GoodModel(string name, bool isShow = true)
        {
            _good = new Good() { 
                Name = name,
                IsShow = isShow
            };
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
