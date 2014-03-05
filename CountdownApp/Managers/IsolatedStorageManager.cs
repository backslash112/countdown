using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CountdownApp.Managers
{
    public class IsolatedStorageManager
    {
        readonly IsolatedStorageSettings settings;

        private static IsolatedStorageManager instance;

        public static IsolatedStorageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IsolatedStorageManager();
                }
                return instance;
            }
        }
        public IsolatedStorageManager()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        public void Set<T>(string key, T value)
        {
            if (settings.Contains(key))
            {
                settings[key] = value;
            }
            else
            {
                settings.Add(key, value);
            }
            settings.Save();
        }
        public T Get<T>(string key)
        {
            if (!settings.Contains(key))
            {
                settings.Add(key, default(T));
            }
            return (T)settings[key];
        }

        public bool Contains(string key)
        {
            return settings.Contains(key);
        }
    }
}
