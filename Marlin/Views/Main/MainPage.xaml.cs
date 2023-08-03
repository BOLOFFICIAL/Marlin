using Marlin.Models;
using Marlin.SystemFiles;
using Marlin.Views.Main;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Marlin.Views.Main
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menu)
            {

                switch (menu.Header.ToString())
                {
                    case "Настройки": NavigationService.Navigate(new SettingsPage()); break;
                    case "Скрипты": Models.MessageBox.MakeMessage(menu.Header.ToString(), "234", "Error"); break;
                    case "Действия": Models.MessageBox.MakeMessage(menu.Header.ToString(), "345", "Question"); break;
                }

                Context.Settings.Theme = new ProgramColor
                    (
                    pagecolor: Context.MessageBox.PageColor,
                    fontcolor: Context.MessageBox.FontColor,
                    backgroundcolor: Context.MessageBox.BackgroundColor,
                    buttonfontcolor: Context.MessageBox.BackgroundColor
                    );
            }

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Windows.MessageBox.Show(Context.Command);
                Settings.SaveSettings();
            }
        }
    }
}
