using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KipCart.Database.Entities
{
    /// <summary>
    /// Сущность товара в покупке
    /// </summary>
    [PrimaryKey(nameof(PurchaseID), nameof(GoodID))]
    public class PurchaseGood
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
    }
}
