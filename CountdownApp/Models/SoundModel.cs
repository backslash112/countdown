using CountdownApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownApp.Models
{
    public class SoundModel
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string uri;

        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        public override string ToString()
        {
            return name;
        }

        public static List<SoundModel> Sounds
        {
            get;
            set;
        }

        static SoundModel()
        {
            InitializeSounds();
        }

        public static void InitializeSounds()
        {
            Sounds = new List<SoundModel>();
            Sounds.Add(new SoundModel() { ID = -1, Name = AppResources.SoundVibrateText });
            for (int i = 1; i < 9; i++)
            {
                Sounds.Add(new SoundModel() { ID = i, Name = AppResources.AlarmText + " " + i, Uri = "Sounds/Alarm" + i + ".wav" });
            }
        }

        public static string GetSoundUriByID(int id)
        {
            return Sounds.First(s => s.ID.Equals(id)).Uri;
        }
    }
}
