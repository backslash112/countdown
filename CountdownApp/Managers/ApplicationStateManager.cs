using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CountdownApp.Managers
{
    public class ApplicationStateManager
    {
        readonly IDictionary<string, object> state;

        public ApplicationStateManager()
        {
            state = PhoneApplicationService.Current.State;
        }

        public void Set<T>(string key, T value)
        {
            if (state.ContainsKey(key))
            {
                state[key] = value;
            }
            else
            {
                state.Add(key, value);
            }
        }

        public T Get<T>(string key)
        {
            if (!state.ContainsKey(key))
            {
                state.Add(key, default(T));
            }
            return (T)state[key];
        }
    }
}
