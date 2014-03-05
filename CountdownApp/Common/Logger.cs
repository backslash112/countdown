using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownApp.Common
{
    public class Logger
    {
        public static void Log(string str)
        {
            Debug.WriteLine(DateTime.Now + ": " + str);
        }

        public static void Log(object o)
        {
            Debug.WriteLine(DateTime.Now + ": " + o);
        }
    }
}
