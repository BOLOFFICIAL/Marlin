using Marlin.SystemFiles;
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
                    case "Скрипты": Models.MessageBox.MakeMessage("На данный момент страница не доступна", MessageType.Error); break;
                    case "Действия": Models.MessageBox.MakeMessage("На данный момент страница не доступна", MessageType.Error); break;
                }
            }

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Models.MessageBox.MakeMessage(Context.Command, MessageType.Info);
            }
        }
    }
}
