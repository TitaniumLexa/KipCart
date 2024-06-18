using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace KipCart.Database.Entities
{
    /// <summary>
    /// Сущность товара
    /// </summary>
    [PrimaryKey(nameof(ID))]
    public class Good:INotifyPropertyChanged
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

        [NotMapped]
        public bool Show
        {
            get
            {
                return IsShow;
            }
            set
            {
                if (IsShow != value)
                {
                    IsShow = value;
                    NotifyPropertyChanged();
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
