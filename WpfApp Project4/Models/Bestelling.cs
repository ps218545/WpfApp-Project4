using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp_Project4.Views;

namespace WpfApp_Project4.Models
{
    public class Bestelling: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private int bestellingId;
        public int BestellingId
        {
            get { return bestellingId; }
            set { bestellingId = value; OnPropertyChanged(); }
        }
        private ulong klantId;
        public ulong KlantId
        {
            get { return klantId; }
            set { klantId = value; OnPropertyChanged(); }
        }

        private DateTime datum;
        public DateTime Datum
        {
             get { return datum; }
             set { datum = value; OnPropertyChanged(); }
        }

        // bestelstatus enum call


        private ulong klantId2;
        public ulong KlantId2
        {
            get { return klantId2; }
            set { klantId2 = value; OnPropertyChanged(); }
        }

        private Klant? klant;
        public Klant? Klant
        {
            get { return klant; }
            set { klant = value; OnPropertyChanged(); }
        }
    }
}
