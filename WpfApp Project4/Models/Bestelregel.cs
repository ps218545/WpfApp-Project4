using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Project4.Models
{
    public class Bestelregel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int bestelRegelId;
        public int BestelRegelId
        {
            get { return bestelRegelId; }
            set { bestelRegelId = value; OnPropertyChanged(); }
        }

        private int bestellingId;
        public int BestellingId
        {
            get { return bestellingId; }
            set { bestellingId = value; OnPropertyChanged(); }
        }

        private Bestelling? bestelling;
        public Bestelling? Bestelling
        {
            get { return bestelling; }
            set { bestelling = value; OnPropertyChanged(); }
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

        private int aantal;
        public int Aantal
        {
            get { return aantal; }
            set { aantal = value; OnPropertyChanged(); }
        }

        private int afmetingId;
        public int AfmetingId
        {
            get { return afmetingId; }
            set { afmetingId = value; OnPropertyChanged(); }
        }
        private Afmeting? afmeting;
        public Afmeting? Afmeting
        {
            get { return afmeting; }
            set { afmeting = value; OnPropertyChanged(); }
        }
    }
}
