using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp_Project4.Models;

namespace WpfApp_Project4.Views
{
    /// <summary>
    /// Interaction logic for Gerecht.xaml
    /// </summary>
    public partial class Gerecht : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region fields
        private readonly Project4db db = new Project4db();
        private readonly string serviceDeskBericht = "\n\nNeem contact op met de service desk";
        #endregion

        #region properties
        private Product? selectedGerecht;
        public Product? SelectedGerecht
        {
            get { return selectedGerecht; }
            set
            {
                selectedGerecht = value;
                PopulateMealIngredients();
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Product> gerechten = new();
        public ObservableCollection<Product> Gerechten
        {
            get { return gerechten; }
            set { gerechten = value; OnPropertyChanged(); }
        }

        private ProductIngredient? selectedGerechtIngredient;
        public ProductIngredient? SelectedGerechtIngredient
        {
            get { return selectedGerechtIngredient; }
            set { selectedGerechtIngredient = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ProductIngredient> gerechtIngredients = new();
        public ObservableCollection<ProductIngredient> GerechtIngredients
        {
            get { return gerechtIngredients; }
            set { gerechtIngredients = value; OnPropertyChanged(); }
        }

        private Ingredient? selectedIngredient;
        public Ingredient? SelectedIngredient
        {
            get { return selectedIngredient; }
            set { selectedIngredient = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Ingredient> ingredients = new();
        public ObservableCollection<Ingredient> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; OnPropertyChanged(); }
        }


        private uint quantity;
        public uint Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(); }
        }
        #endregion

        public Gerecht()
        {
            PopulateMeals();
            PopulateIngredients();

            InitializeComponent();

            DataContext = this;
        }

        private void PopulateMeals()
        {
            Gerechten.Clear();
            //string result = db.GetGerechten(Gerechten);
            //if (result != Project4db.OK)
            //{
            //    MessageBox.Show(result + serviceDeskBericht);
            //}
        }

        private void PopulateIngredients()
        {
            Ingredients.Clear();
            string result = db.GetIngredients(Ingredients);
            if (result != Project4db.OK)
            {
                MessageBox.Show(result + serviceDeskBericht);
            }
        }

        private void PopulateMealIngredients()
        {
            //GerechtIngredients.Clear();
            //if (SelectedGerecht != null)
            //{
            //    string result = db.GetgerechtIngredientsBygerecht(SelectedGerecht.ProductId, GerechtIngredients);
            //    if (result != Project4db.OK)
            //    {
            //        MessageBox.Show(result + serviceDeskBericht);
            //    }
            //}
            //else
            //{
            //    gerechtIngredients.Clear();
            //}
        }

        private void BtnKoppel_Click(object sender, RoutedEventArgs e)
        {
            if (Quantity == 0)
            {
                MessageBox.Show("Vul een aantal in dat groter is dan 0.");
                return;
            }
            if (SelectedGerecht == null)
            {
                MessageBox.Show("Selecteer de maaltijd waaraan u het ingredient wil toevoegen.");
                return;
            }
            if (SelectedIngredient == null)
            {
                MessageBox.Show("Selecteer het ingrediënt dat u aan de maaltijd wil toevoegen");
                return;
            }
            ProductIngredient mealIngredient = new()
            {
                //MealId = SelectedGerecht.MealId,
                //IngredientId = SelectedIngredient.IngredientId,
                //Quantity = this.Quantity,
            };

            //string result = db.CreateGerechtIngredient(mealIngredient);
            //if (result == Project4db.OK)
            //{
            //    Quantity = 0;
            //    PopulateMeals(); // Maaltijd is bijgwerkt, want er is een ingredient aan toegevoegd
            //    PopulateMealIngredients();
            //}
            //else
            //{
            //    MessageBox.Show(result);
            //}
        }

        private void BtnOntkoppel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ProductIngredient mealIngredient = (ProductIngredient)btn.DataContext;
            //string result = db.DeleteMealIngredient(mealIngredient.GerechtIngredientId);
            //if (result == Project4db.OK)
            //{
            //    PopulateMeals();
            //    PopulateMealIngredients();
            //}
            //else
            //{
            //    MessageBox.Show(result);
            //}
        }

        private void Selection_Click(object sender, RoutedEventArgs e)
        {
            new Selection().Show();
            this.Close();
        }
        private void Mute_Click(object sender, RoutedEventArgs e)
        { }
    }
}
