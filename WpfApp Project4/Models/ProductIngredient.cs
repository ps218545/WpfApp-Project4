using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Project4.Models
{
    public class ProductIngredient : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private int productIngredientId;
        public int ProductIngredientId
        {
            get { return productIngredientId; }
            set { productIngredientId = value; OnPropertyChanged(); }
        }

        private int productId;
        public int ProductId
        {
            get { return productId; }
            set { productId = value; OnPropertyChanged(); }
        }

        private Product? product;
        public Product? Product
        {
            get { return product; }
            set { product = value; OnPropertyChanged(); }
        }

        private int ingredientId;
        public int IngredientId
        {
            get { return ingredientId; }
            set { ingredientId = value; OnPropertyChanged(); }
        }

        private Ingredient? ingredient;
        public Ingredient? Ingredient
        {
            get { return ingredient; }
            set { ingredient = value; OnPropertyChanged(); }
        }

        private uint aantalIngr;
        public uint AantalIngr
        {
            get { return aantalIngr; }
            set { aantalIngr = value; OnPropertyChanged(); }
        }

        public decimal Amount { get => Ingredient == null ? 0.0m : AantalIngr * Ingredient.Price; }

    }
}
