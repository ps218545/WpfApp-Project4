using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Numerics;

namespace WpfApp_Project4.Models
{
    public abstract class Muziek
    {
        protected SoundPlayer muziek;
        public abstract void Initialize();
        public abstract void Play();
        public abstract void Stop();
    }

    public class Muziek2 : Muziek
    {
        public override void Initialize()
        {
            muziek = new SoundPlayer(@"Assets\muziek.wav");
        }

        public override void Play()
        {
            muziek.PlayLooping();
        }

        public override void Stop()
        {
            muziek.Stop();
        }
    }

    public class PublicMuziek : Muziek2 
    {
        private static Muziek musicPlayer;
        public static bool isPlaying = false;
        public static bool isMuted = false;

        public static void Initialize(Muziek player)
        {
            musicPlayer = player;
            musicPlayer.Initialize();
        }

        public static void Play()
        {
            if (!isPlaying)
            {
                musicPlayer.Play();
                isPlaying = true;
            }
        }

        public static void Stop()
        {
            musicPlayer.Stop();
            isPlaying = false;
        }
    }

}
