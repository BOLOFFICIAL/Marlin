using Marlin.Models;
using System.Windows;

namespace Marlin.SystemFiles
{
    public class Context
    {
        public static string Command = "";
        public static Models.MessageBox MessageBox = new Models.MessageBox();
        public static Settings Settings = new Settings();
        public static Window MessageWindow = new Window();
    }
}
