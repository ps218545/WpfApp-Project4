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
    /// Interaction logic for Bestellingen.xaml
    /// </summary>
    public partial class Bestellingen : Window, INotifyPropertyChanged
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
        private Bestelling? selectedBestelling;
        public Bestelling? SelectedBestelling
        {
            get { return selectedBestelling; }
            set
            {
                SelectedBestelling = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Bestelling> bestellingenn = new();
        public ObservableCollection<Bestelling> Bestellingenn
        {
            get { return bestellingenn; }
            set { bestellingenn = value; OnPropertyChanged(); }
        }

        private uint aantal;
        public uint Aantal
        {
            get { return aantal; }
            set { aantal = value; OnPropertyChanged(); }
        }
        #endregion


        public Bestellingen()
        {
            PopulateBestellingen();

            InitializeComponent();
            DataContext = this;
        }

        private void PopulateBestellingen()
        {
            Bestellingenn.Clear();
            string result = db.GetBestellingen(Bestellingenn);
            if (result != Project4db.OK)
            {
                MessageBox.Show(result + serviceDeskBericht);
            }
        }




        private void Selection_Click(object sender, RoutedEventArgs e)
        {
            new Selection().Show();
            this.Close();
        }
    }
}
