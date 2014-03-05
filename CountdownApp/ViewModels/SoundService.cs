using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownApp.ViewModels
{
    /// <summary>
    /// 提供播放声音的服务。
    /// </summary>
    public class SoundService
    {
        SoundEffectInstance effectInstance;

        public SoundService(string soundFileUri)
        {
            var stream = TitleContainer.OpenStream(soundFileUri);
            effectInstance = SoundEffect.FromStream(stream).CreateInstance();
        }

        public void Play()
        {
            if (effectInstance != null && effectInstance.State == SoundState.Playing)
            {
                effectInstance.Stop();
            }
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

    }
}
