using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Marlin.Views.Window;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Marlin.Views.Main
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            if (Context.Settings.Password.Length == 0)
            {
                Voix.SpeakAsync("Прежде чем приступить к использованию необходимо зарегистрироваться");
                System.Windows.Window window = new System.Windows.Window
                {
                    SizeToContent = System.Windows.SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    WindowStyle = WindowStyle.None,
                    ResizeMode = ResizeMode.NoResize,
                    Content = new RegistrationPage()
                };
                Context.Window = window;
                window.ShowDialog();
            }
            if (Context.Settings.Password.Length == 0)
            {
                Environment.Exit(0);
            }
            InitializeComponent();
        }
    }
}
