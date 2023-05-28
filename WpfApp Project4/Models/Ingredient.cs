using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Project4.Models
{
    public class Ingredient : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private int ingredientId;
        public int IngredientId
        {
            get { return ingredientId; }
            set { ingredientId = value; OnPropertyChanged(); }
        }

        private string? name;
        public string? Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        private decimal ingredientPrijs;
        public decimal IngredientPrijs
        {
            get { return ingredientPrijs; }
            set { ingredientPrijs = value; OnPropertyChanged(); }
        }

        private int unitId;
        public int UnitId
        {
            get { return unitId; }
            set { unitId = value; OnPropertyChanged(); }
        }

        private Unit? unit;
        public Unit? Unit
        {
            get { return unit; }
            set { unit = value; OnPropertyChanged(); }
        }
    }
}
