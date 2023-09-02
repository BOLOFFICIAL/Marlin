using Marlin.Models;
using Marlin.ViewModels.Main;
using Marlin.ViewModels.Window;
using System.Windows;

namespace Marlin.SystemFiles
{
    public class Context
    {

        public static Models.MessageBox MessageBox = new();
        public static Settings Settings = new();
        public static Settings CopySettings = new();
        public static Window Window = new();

        public static MainPageViewModel MainPageVM = new();
        public static SettingsPageViewModel SettingsPageVM = new();
        public static MessagePageViewModel MessagePageVM = new();
        public static RegistrationPageViewModel RegistrationPageVM = new();

        public static Window MainWindow = new();
    }
}
