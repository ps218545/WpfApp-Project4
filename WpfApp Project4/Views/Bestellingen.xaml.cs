using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using WpfApp_Project4.Views;


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
                selectedBestelling = value;
                PopulateBestelRegels();
                PopulateStatus();
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Bestelling> bestellingenn = new();
        public ObservableCollection<Bestelling> Bestellingenn
        {
            get { return bestellingenn; }
            set { bestellingenn = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Bestelregel> bestelRegels = new();
        public ObservableCollection<Bestelregel> BestelRegels
        {
            get { return bestelRegels; }
            set { bestelRegels = value; OnPropertyChanged(); }
        }


        private ObservableCollection<BestelStatus> statuss = new();
        public ObservableCollection<BestelStatus> Statuss
        {
            get { return statuss; }
            set { statuss = value; OnPropertyChanged(); }
        }

        private BestelStatus? existingBestellingStatus;
        public BestelStatus? ExistingBestellingStatus
        {
            get { return existingBestellingStatus; }
            set
            {
                existingBestellingStatus = value;
                OnPropertyChanged();
                if (SelectedBestelling != null)
                {
                    SelectedBestelling.StatusId = value == null ? 0 : value.StatusId;
                }
            }
        }

        private BestelStatus? selectedStatus;
        public BestelStatus? SelectedStatus
        {
            get { return selectedStatus; }
            set { selectedStatus = value; OnPropertyChanged(); }
        }
        #endregion


        public Bestellingen()
        {
            PopulateBestellingen();
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

        private void PopulateBestellingen()
        {
            Bestellingenn.Clear();
            string result = db.GetBestellingen(Bestellingenn);
            if (result != Project4db.OK)
            {
                MessageBox.Show(result + serviceDeskBericht);
            }
        }
        private void PopulateBestelRegels()
        {
            BestelRegels.Clear();
            if (SelectedBestelling != null)
            {
                string result = db.GetBestelRegelsByBestelling(SelectedBestelling.BestellingId, BestelRegels);
                if (result != Project4db.OK)
                {
                    MessageBox.Show(result + serviceDeskBericht);
                }
            }
            else
            {
                bestelRegels.Clear();
            }
        }

        private void PopulateStatus()
        {
            Statuss.Clear();
            string dbResult = db.GetStatuses(Statuss);
            if (dbResult != Project4db.OK)
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }
        }


        private void UpdateStatus(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            bool doUpdate = true;
            //BestelStatus selectedStatus = (BestelStatus)comboBox.SelectedItem;


            //if (SelectedStatus == null)
            //{
            //    MessageBox.Show("Selecteer eerst de status die u wil wijzigen.");
            //    return;
            //}
            if (SelectedBestelling != null && SelectedStatus != null) 
            {
                SelectedBestelling.StatusId = SelectedStatus.StatusId;


                if (SelectedStatus.StatusId == 6) // Delete
                {
                    MessageBoxResult result = MessageBox.Show("Weet u zeker dat de bestelling compleet is en verwijderd moet worden?", "Bevestiging", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        string dbResult3 = db.DeleteBestelRegels(SelectedBestelling.BestellingId);
                        string dbResult2 = db.DeleteBestelling(SelectedBestelling.BestellingId);
                        if (dbResult2 == Project4db.OK && dbResult3 == Project4db.OK)
                        {
                            PopulateStatus();
                            PopulateBestellingen();
                            doUpdate = false;
                        }
                        else
                        {
                            MessageBox.Show(dbResult2 + serviceDeskBericht);
                        }
                    }
                    else 
                    {
                        SelectedStatus.StatusId = 5;
                    }
                }

                if (doUpdate == true) {
                    string dbResult = db.UpdateStatus(SelectedBestelling.BestellingId, SelectedStatus.StatusId, SelectedStatus); // update
                    if (dbResult == Project4db.OK)
                    {
                        PopulateBestellingen();
                        PopulateStatus();
                    }
                    else
                    {
                        MessageBox.Show(dbResult + serviceDeskBericht);
                    }
                }
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
