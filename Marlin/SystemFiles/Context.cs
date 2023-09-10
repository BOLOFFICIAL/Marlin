using Marlin.Models;
using Marlin.SystemFiles.Types;
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

        public static ActionType Action;
    }
}
