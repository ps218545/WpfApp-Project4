using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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


namespace WpfApp_Project4
{
    /// <summary>
    /// Interaction logic for Selection.xaml
    /// </summary>
    public partial class Selection : Window
    {
        SoundPlayer music = new SoundPlayer(@"Assets\muziek.wav");
        bool muted = false;
        public Selection()
        {
            InitializeComponent();
            music.Load();
            music.PlayLooping();
        }

        private void Mute_Click(object sender, RoutedEventArgs e)
        {

            if (muted == false)
            {
                music.Stop();
                muted = true;
                Mute.Content = "Unmute";
            }
            else if (muted == true) {
                music.PlayLooping();
                muted = false;
                Mute.Content = "Mute";
            }


        }

        private void Bestellingen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Units_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Manager_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
