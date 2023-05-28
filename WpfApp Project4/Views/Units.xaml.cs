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
    /// Interaction logic for Units.xaml
    /// </summary>
    public partial class Units : Window, INotifyPropertyChanged
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
        #region Properties

        // Units

        private Unit? existingUnit;
        public Unit? ExistingtUnit
        {
            get { return existingUnit; }
            set
            {
                existingIngredientUnit = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Unit> unitss = new();
        public ObservableCollection<Unit> Unitss
        {
            get { return unitss; }
            set { unitss = value; OnPropertyChanged(); }
        }

        private Unit? selectedUnit;
        public Unit? SelectedUnit
        {
            get { return selectedUnit; }
            set { selectedUnit = value; OnPropertyChanged(); }
        }

        private Unit newUnit = new();
        public Unit NewUnit
        {
            get { return newUnit; }
            set
            {
                newUnit = value;
                OnPropertyChanged();
            }
        }



        // ingredients

        private Unit? newIngredientUnit;
        public Unit? NewIngredientUnit
        {
            get { return newIngredientUnit; }
            set
            {
                newIngredientUnit = value;
                OnPropertyChanged();
                NewIngredient.UnitId = value == null ? 0 : value.UnitId;
            }
        }

        private Unit? existingIngredientUnit;
        public Unit? ExistingIngredientUnit
        {
            get { return existingIngredientUnit; }
            set
            {
                existingIngredientUnit = value;
                OnPropertyChanged();
                if (SelectedIngredient != null)
                {
                    SelectedIngredient.UnitId = value == null ? 0 : value.UnitId;
                }
            }
        }


        private ObservableCollection<Ingredient> ingredients = new();
        public ObservableCollection<Ingredient> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; OnPropertyChanged(); }
        }

        private Ingredient? selectedIngredient;
        public Ingredient? SelectedIngredient
        {
            get { return selectedIngredient; }
            set
            {
                selectedIngredient = value;
                // geen ingrediënt geselecteerd of Units property nog niet gevuld?
                if (value == null || Unitss == null)
                {
                    // er is nog geen geselecteerde unit
                    ExistingIngredientUnit = null;
                }
                else
                {
                    // zoek de unit op waarvan de Unit Id gelijk is aan de Unit id van het geselecteerde ingrediënt
                    ExistingIngredientUnit = Unitss.FirstOrDefault(x => x.UnitId == value.UnitId);
                }
                OnPropertyChanged();
            }
        }

        private Ingredient newIngredient = new();
        public Ingredient NewIngredient
        {
            get { return newIngredient; }
            set
            {
                newIngredient = value;
                OnPropertyChanged();
                NewIngredientUnit = null;   // Voor een nieuw ingredient is nog geen unit bekend
            }
        }

        #endregion


        public Units()
        {
            InitializeComponent();
            PopulateUnits();
            PopulateIngredients();
            DataContext = this;
            InitializeMusic();
        }
        #region Muziek
        private void InitializeMusic()
        {
            if ((!PublicMuziek.isPlaying) && (PublicMuziek.isMuted == false))
            {
                PublicMuziek.Initialize(new Muziek2());
                PublicMuziek.Play();
            }
            if (PublicMuziek.isMuted == true)
            {
                Mute.Content = "Unmute";
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (!Application.Current.Windows.OfType<Window>().Any(w => w != this))
            {
                PublicMuziek.Stop();
            }
        }
        #endregion


        private void PopulateUnits()
        {
            Unitss.Clear();
            string dbResult = db.GetUnits(Unitss);
            if (dbResult != Project4db.OK)
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }
        }

        private void PopulateIngredients()
        {
            Ingredients.Clear();
            string dbResult = db.GetIngredients(Ingredients);
            if (dbResult != Project4db.OK)
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }
        }

        private void BtnCreateUnit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewUnit.UnitNaam))
            {
                MessageBox.Show("Vul naam van de unit in");
                return;
            }

            string dbResult = db.CreateUnit(NewUnit);
            if (dbResult == Project4db.OK)
            {
                NewUnit = new();
                PopulateUnits();
            }
            else
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }

        }

        private void BtnUpdateUnit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUnit == null)
            {
                MessageBox.Show("Selecteer eerst de unit die u wil wijzigen.");
                return;
            }
            if (string.IsNullOrEmpty(SelectedUnit.UnitNaam))
            {
                MessageBox.Show("Vul naam van de unit in.");
                return;
            }

            string dbResult = db.UpdateUnit(SelectedUnit.UnitId, SelectedUnit);
            if (dbResult == Project4db.OK)
            {
                PopulateUnits();
            }
            else
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }
        }

        private void BtnDeleteUnit_Click(object sender, RoutedEventArgs e)
        {
            Button btnDel = (Button)sender;
            Unit unit = (Unit)btnDel.DataContext;

            string dbResult = db.DeleteUnit(unit.UnitId);
            if (dbResult == Project4db.OK)
            {
                PopulateUnits();
            }
            else
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }
        }


        private void BtnCreateIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewIngredient.Name))
            {
                MessageBox.Show("Vul naam van het ingrediënt in");
                return;
            }
            if (NewIngredient.UnitId == 0)
            {
                MessageBox.Show("Selecteer een eenheid.");
                return;
            }
            if (NewIngredient.IngredientPrijs < 0)
            {
                MessageBox.Show("Wijzig de prijs. Deze mag niet negatief zijn.");
                return;
            }

            string dbResult = db.CreateIngredient(NewIngredient);
            if (dbResult == Project4db.OK)
            {
                NewIngredient = new();
                PopulateIngredients();
            }
            else
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }

        }

        private void BtnUpdateIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedIngredient == null)
            {
                MessageBox.Show("Selecteer eerst het ingredient dat u wil wijzigen.");
                return;
            }
            if (string.IsNullOrEmpty(SelectedIngredient.Name))
            {
                MessageBox.Show("Vul naam van het ingrediënt in.");
                return;
            }
            if (SelectedIngredient.UnitId == 0)
            {
                MessageBox.Show("Selecteer een eenheid.");
                return;
            }
            if (SelectedIngredient.IngredientPrijs < 0)
            {
                MessageBox.Show("Wijzig de prijs. Deze mag niet negatief zijn.");
                return;
            }

            string dbResult = db.UpdateIngredient(SelectedIngredient.IngredientId, SelectedIngredient);
            if (dbResult == Project4db.OK)
            {
                PopulateIngredients();
            }
            else
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }
        }

        private void BtnDeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            Button btnDel = (Button)sender;
            Ingredient ingredient = (Ingredient)btnDel.DataContext;

            string dbResult = db.DeleteIngredient(ingredient.IngredientId);
            if (dbResult == Project4db.OK)
            {
                PopulateIngredients();
            }
            else
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }
        }



        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            if (PublicMuziek.isMuted == false)
            {
                PublicMuziek.Stop();
                PublicMuziek.isMuted = true;
                Mute.Content = "Unmute";
            }
            else if (PublicMuziek.isMuted == true)
            {
                PublicMuziek.Play();
                PublicMuziek.isMuted = false;
                Mute.Content = "Mute";
            }
        }

        private void Selection_Click(object sender, RoutedEventArgs e)
        {
            new Selection().Show();
            this.Close();
        }


    }
}
