using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace KipCart.Database.Entities
{
    /// <summary>
    /// Сущность товара в покупке
    /// </summary>
    [PrimaryKey(nameof(PurchaseID), nameof(GoodID))]
    public class PurchaseGood : INotifyPropertyChanged
    {
        /// <summary>
        /// Идентификатор покупки
        /// </summary>
        [ForeignKey(nameof(PurchaseID))]
        public int PurchaseID { get; set; }

        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [ForeignKey(nameof(GoodID))]
        public int GoodID { get; set; }

        /// <summary>
        /// Количество товара в покупке
        /// </summary>
        public uint GoodAmount { get; set; }

        /// <summary>
        /// Цена товара в покупке
        /// </summary>
        public uint GoodPrice { get; set; }

        /// <summary>
        /// Навигационное свойство товар
        /// </summary>
        public Good Good { get; set; } = null!;

        /// <summary>
        /// Навигационное свойство покупка
        /// </summary>
        public Purchase Purchase { get; set; } = null!;


        [NotMapped]
        public uint TotalPrice
        {
            get
            {
                return GoodAmount * GoodPrice;
            }
        }

        [NotMapped]
        public uint Amount
        {
            get
            {
                return GoodAmount;
            }
            set
            {
                if (GoodAmount != value)
                {
                    GoodAmount = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        [NotMapped]
        public uint Price
        {
            get
            {
                return GoodPrice;
            }
            set
            {
                if (GoodPrice != value)
                {
                    GoodPrice = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(TotalPrice));
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
