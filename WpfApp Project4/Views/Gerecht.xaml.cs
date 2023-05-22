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
        private Product? selectedProduct;
        public Product? SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                PopulateProductIngredients();
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Product> producten = new();
        public ObservableCollection<Product> Producten
        {
            get { return producten; }
            set { producten = value; OnPropertyChanged(); }
        }

        private ProductIngredient? selectedProductIngredient;
        public ProductIngredient? SelectedProductIngredient
        {
            get { return selectedProductIngredient; }
            set { selectedProductIngredient = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ProductIngredient> productIngredients = new();
        public ObservableCollection<ProductIngredient> ProductIngredients
        {
            get { return productIngredients; }
            set { productIngredients = value; OnPropertyChanged(); }
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

        private ObservableCollection<Ingredient> units = new();
        public ObservableCollection<Ingredient> Units
        {
            get { return units; }
            set { units = value; OnPropertyChanged(); }
        }

        private uint aantalIngr;
        public uint AantalIngr
        {
            get { return aantalIngr; }
            set { aantalIngr = value; OnPropertyChanged(); }
        }
        #endregion

        public Gerecht()
        {
            PopulateProducts();
            PopulateIngredients();
            InitializeComponent();
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




        private void PopulateProducts()
        {
            Producten.Clear();
            string result = db.GetProducten(Producten);
            if (result != Project4db.OK)
            {
                MessageBox.Show(result + serviceDeskBericht);
            }
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

        private void PopulateProductIngredients()
        {
            ProductIngredients.Clear();
            if (SelectedProduct != null)
            {
                string result = db.GetProductIngredientsByProduct(SelectedProduct.ProductId, ProductIngredients);
                if (result != Project4db.OK)
                {
                    MessageBox.Show(result + serviceDeskBericht);
                }
            }
            else
            {
                productIngredients.Clear();
            }
        }

        private void BtnKoppel_Click(object sender, RoutedEventArgs e)
        {
            if (AantalIngr == 0)
            {
                MessageBox.Show("Vul een aantal in dat groter is dan 0.");
                return;
            }
            if (SelectedProduct == null)
            {
                MessageBox.Show("Selecteer de maaltijd waaraan u het ingredient wil toevoegen.");
                return;
            }
            if (SelectedIngredient == null)
            {
                MessageBox.Show("Selecteer het ingrediënt dat u aan de maaltijd wil toevoegen");
                return;
            }
            ProductIngredient productIngredient = new()
            {
                ProductId = SelectedProduct.ProductId,
                IngredientId = SelectedIngredient.IngredientId,
                AantalIngr = this.AantalIngr,
            };

            string result = db.CreateProductIngredient(productIngredient);
            if (result == Project4db.OK)
            {
                AantalIngr = 0;
                PopulateProducts(); // Maaltijd is bijgwerkt, want er is een ingredient aan toegevoegd
                PopulateProductIngredients();
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        private void BtnOntkoppel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ProductIngredient mealIngredient = (ProductIngredient)btn.DataContext;
            string result = db.DeleteProductIngredient(mealIngredient.ProductIngredientId);
            if (result == Project4db.OK)
            {
                PopulateProducts();
                PopulateProductIngredients();
            }
            else
            {
                MessageBox.Show(result);
            }
        }

        private void Selection_Click(object sender, RoutedEventArgs e)
        {
            new Selection().Show();
            this.Close();
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
    }
}
