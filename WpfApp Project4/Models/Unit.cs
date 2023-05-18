using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Project4.Models
{
    public class Unit : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int unitId;
        public int UnitId
        {
            get { return unitId; }
            set { unitId = value; OnPropertyChanged(); }
        }

        private string? unitNaam;
        public string? UnitNaam
        {
            get { return unitNaam; }
            set { unitNaam = value; OnPropertyChanged(); }
        }
    }
}
