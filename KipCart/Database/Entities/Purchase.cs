using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace KipCart.Database.Entities
{
    /// <summary>
    /// Сущность покупки
    /// </summary>
    [PrimaryKey(nameof(ID))]
    public class Purchase
    {
        /// <summary>
        /// Идентификатор покупки
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Дата покупки
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Навигационное свойство список товаров в покупке
        /// </summary>
        public IEnumerable<PurchaseGood> PurchaseGoods { get; } = new List<PurchaseGood>();

        [NotMapped]
        public string DateString { get
            {
                return Date.ToShortDateString();
            }
        }

        [NotMapped]
        public uint TotalPrice
        {
            get
            {
                uint sum = 0;
                return PurchaseGoods.Aggregate(sum, (acc, good) => good.TotalPrice + acc);
            }
        }
    }
}
