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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menu)
            {
                switch (menu.Header.ToString())
                {
                    case "Настройки": NavigationService.Navigate(new SettingsPage()); break;
                    case "Скрипты": Models.MessageBox.MakeMessage("Страница не доступна", MessageType.Error); break;
                    case "Действия": Models.MessageBox.MakeMessage("Страница не доступна", MessageType.Error); break;
                }
            }

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Context.MainPage.Command.Length > 0)
            {
                Models.MessageBox.MakeMessage(Context.MainPage.Command);
                Context.MainPage.Command = "";
            }
        }
    }
}
