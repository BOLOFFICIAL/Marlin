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

        public static MainPageViewModel MainPage = new();
        public static SettingsPageViewModel SettingsPage = new();
        public static MessagePageViewModel MessagePage = new();
        public static RegistrationPageViewModel RegistrationPage = new();
    }
}
