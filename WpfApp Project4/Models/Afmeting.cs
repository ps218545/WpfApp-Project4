using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Project4.Models
{
    public class Afmeting: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int afmetingId;
        public int AfmetingId
        {
            get { return afmetingId; }
            set { afmetingId = value; OnPropertyChanged(); }
        }

        private string? afmetingNaam;
        public string? AfmetingNaam
        {
            get { return afmetingNaam; }
            set { afmetingNaam = value; OnPropertyChanged(); }
        }
    }
}
