using Marlin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marlin.SystemFiles
{
    public class Context
    {
        public static string Command = "";
        public static Message MessageContext = new Message();
        public static Settings Settings = new Settings();
    }
}
