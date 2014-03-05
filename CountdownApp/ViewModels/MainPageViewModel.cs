using CountdownApp.Models;
using CountdownApp.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Private Fileds

        private List<SoundModel> sounds;
        private SoundModel sound;
        SoundEffectInstance effectInstance;

        #endregion

        #region Public Properties

        public List<SoundModel> Sounds
        {
            get
            {
                return SoundModel.Sounds;
            }
            set
            {
                sounds = value;
                NotifyPropertyChanged("Sounds");
            }
        }

        public SoundModel Sound
        {
            get
            {
                if (sound == null)
                {
                    // set the default selected item with alart 1.
                    sound = SoundModel.Sounds[1];
                }
                return sound;
            }
            set
            {
                sound = value;
                NotifyPropertyChanged("Sound");
            }
        }

        private string name;

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = AppResources.ApplicationTitle;
                }
                return name;
            }
            set
            {
                if (name == value)
                {
                    return;
                }
                name = value;
                NotifyPropertyChanged(() => name);
            }
        }
        #endregion

        #region Public Methods

        public void Play(int id)
        {
            if (effectInstance != null && effectInstance.State == SoundState.Playing)
            {
                effectInstance.Stop();
            }
            SoundModel selectedSound = Sounds.First(s => s.ID == id);

            var stream = TitleContainer.OpenStream(selectedSound.Uri);
            effectInstance = SoundEffect.FromStream(stream).CreateInstance();
            FrameworkDispatcher.Update();

            effectInstance.Play();
        }

        public void Stop()
        {
            if (effectInstance != null && effectInstance.State == SoundState.Playing)
            {
                effectInstance.Stop();
            }
        }

        #endregion

        #region Constructor

        public MainPageViewModel()
        {
        }

        #endregion
    }
}
