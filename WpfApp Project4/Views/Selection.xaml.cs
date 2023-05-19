using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp_Project4.Models;
using WpfApp_Project4.Views;

namespace WpfApp_Project4
{
    /// <summary>
    /// Interaction logic for Selection.xaml
    /// </summary>
    public partial class Selection : Window
    {
        //SoundPlayer music = new SoundPlayer(@"Assets\muziek.wav");
        //bool muted = false;

        public Selection()
        {
            InitializeComponent();
            //music.Load();
            //music.PlayLooping();
            InitializeMusic();
        }


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

        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            if (PublicMuziek.isMuted == false)
            {
                PublicMuziek.Stop();
                PublicMuziek.isMuted = true;
                Mute.Content = "Unmute";
            }
            else if (PublicMuziek.isMuted == true) {
                PublicMuziek.Play();
                PublicMuziek.isMuted = false;
                Mute.Content = "Mute";
            }
        }



        private void Bestellingen_Click(object sender, RoutedEventArgs e)
        {

            Bestellingen bestellingen = new Bestellingen();
            bestellingen.Show();
            this.Close();
        }

        private void Units_Click(object sender, RoutedEventArgs e)
        {
            new Units().Show();
            this.Close();
        }

        private void Gerecht_Click(object sender, RoutedEventArgs e)
        {

            new Gerecht().Show();
            this.Close();
        }

        private void Legal_Click(object sender, RoutedEventArgs e)
        {

            new Disclaimer().Show();
            this.Close();
        }
    }
}
