using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Project4.Models
{
    public class Klant : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private ulong klantId2;
        public ulong KlantId2
        {
            get { return klantId2; }
            set { klantId2 = value; OnPropertyChanged(); }
        }

        private string? voorNaam;
        public string? VoorNaam
        {
            get { return voorNaam; }
            set { voorNaam = value; OnPropertyChanged(); }
        }
        private string? achterNaam;
        public string? AchterNaam
        {
            get { return achterNaam; }
            set { achterNaam = value; OnPropertyChanged(); }
        }

        private string? postCode;
        public string? PostCode
        {
            get { return postCode; }
            set { postCode = value; OnPropertyChanged(); }
        }

        private string? adres;
        public string? Adres
        {
            get { return adres; }
            set { adres = value; OnPropertyChanged(); }
        }

        private string? nummer;
        public string? Nummer
        {
            get { return nummer; }
            set { nummer = value; OnPropertyChanged(); }
        }

        private string? mail;
        public string? Mail
        {
            get { return mail; }
            set { mail = value; OnPropertyChanged(); }
        }
    }
}
