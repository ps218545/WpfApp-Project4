using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Project4.Models
{
    public class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private int productId;
        public int ProductId
        {
            get { return productId; }
            set { productId = value; OnPropertyChanged(); }
        }

        private string? productName;
        public string? ProductName
        {
            get { return productName; }
            set { productName = value; OnPropertyChanged(); }
        }

        private decimal productPrijs;
        public decimal ProductPrijs
        {
            get { return productPrijs; }
            set { productPrijs = value; OnPropertyChanged(); }
        }

        public ICollection<ProductIngredient>? ProductIngredients { get; set; }

        public decimal GerechtenPrijs { get => ProductIngredients == null ? 0 : ProductIngredients.Sum(x => x.Amount); }
    }
}
