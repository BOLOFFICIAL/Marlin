using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marlin.SystemFiles
{
    public class BackgroundService
    {
        public static void StartServise() 
        {
            Task.Run(() => { Servise(); });
        }

        private static void Servise() 
        {
            while (true) 
            {
                
            }
        }
    }
}
