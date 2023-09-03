using Marlin.Models;
using System.Windows;

namespace Marlin.SystemFiles
{
    public class Context
    {
        public static Models.MessageBox MessageBox = new();

        public static Settings Settings = new();
        public static Settings CopySettings = new();

        public static Window Window = new();
        public static Window MainWindow = new();
    }
}
