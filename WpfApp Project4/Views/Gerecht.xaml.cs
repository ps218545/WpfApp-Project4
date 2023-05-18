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
        private readonly LosPollosHermanosDb db = new LosPollosHermanosDb();
        private readonly string serviceDeskBericht = "\n\nNeem contact op met de service desk";
        #endregion

        #region properties
        private Meal? selectedMeal;
        public Meal? SelectedMeal
        {
            get { return selectedMeal; }
            set
            {
                selectedMeal = value;
                PopulateMealIngredients();
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Meal> meals = new();
        public ObservableCollection<Meal> Meals
        {
            get { return meals; }
            set { meals = value; OnPropertyChanged(); }
        }

        private MealIngredient? selectedMealIngredient;
        public MealIngredient? SelectedMealIngredient
        {
            get { return selectedMealIngredient; }
            set { selectedMealIngredient = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MealIngredient> mealIngredients = new();
        public ObservableCollection<MealIngredient> MealIngredients
        {
            get { return mealIngredients; }
            set { mealIngredients = value; OnPropertyChanged(); }
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
            Meals.Clear();
            string result = db.GetMeals(Meals);
            if (result != LosPollosHermanosDb.OK)
            {
                MessageBox.Show(result + serviceDeskBericht);
            }
        }

        private void PopulateIngredients()
        {
            Ingredients.Clear();
            string result = db.GetIngredients(Ingredients);
            if (result != LosPollosHermanosDb.OK)
            {
                MessageBox.Show(result + serviceDeskBericht);
            }
        }

        private void PopulateMealIngredients()
        {
            MealIngredients.Clear();
            if (SelectedMeal != null)
            {
                string result = db.GetMealIngredientsByMeal(SelectedMeal.MealId, MealIngredients);
                if (result != LosPollosHermanosDb.OK)
                {
                    MessageBox.Show(result + serviceDeskBericht);
                }
            }
            else
            {
                mealIngredients.Clear();
            }
        }

        private void BtnKoppel_Click(object sender, RoutedEventArgs e)
        {
            if (Quantity == 0)
            {
                MessageBox.Show("Vul een aantal in dat groter is dan 0.");
                return;
            }
            if (SelectedMeal == null)
            {
                MessageBox.Show("Selecteer de maaltijd waaraan u het ingredient wil toevoegen.");
                return;
            }
            if (SelectedIngredient == null)
            {
                MessageBox.Show("Selecteer het ingrediënt dat u aan de maaltijd wil toevoegen");
                return;
            }
            MealIngredient mealIngredient = new()
            {
                MealId = SelectedMeal.MealId,
                IngredientId = SelectedIngredient.IngredientId,
                Quantity = this.Quantity,
            };

            string result = db.CreateMealIngredient(mealIngredient);
            if (result == LosPollosHermanosDb.OK)
            {
                Quantity = 0;
                PopulateMeals(); // Maaltijd is bijgwerkt, want er is een ingredient aan toegevoegd
                PopulateMealIngredients();
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        private void BtnOntkoppel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            MealIngredient mealIngredient = (MealIngredient)btn.DataContext;
            string result = db.DeleteMealIngredient(mealIngredient.MealIngredientId);
            if (result == LosPollosHermanosDb.OK)
            {
                PopulateMeals();
                PopulateMealIngredients();
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            new Selection().Show();
        }
    }
}
