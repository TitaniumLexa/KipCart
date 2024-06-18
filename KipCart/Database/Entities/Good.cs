using Microsoft.EntityFrameworkCore;

namespace KipCart.Database.Entities
{
    /// <summary>
    /// Сущность товара
    /// </summary>
    [PrimaryKey(nameof(ID))]
    public class Good
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Название товара
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Показывать в списке товаров
        /// </summary>
        public bool IsShow { get; set; }
    }
}
