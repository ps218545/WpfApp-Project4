using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Project4.Models
{
    public class BestelStatus: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int statusId;
        public int StatusId
        {
            get { return statusId; }
            set { statusId = value; OnPropertyChanged(); }
        }

        private string? statusNaam;
        public string? StatusNaam
        {
            get { return statusNaam; }
            set { statusNaam = value; OnPropertyChanged(); }
        }
    }
}
