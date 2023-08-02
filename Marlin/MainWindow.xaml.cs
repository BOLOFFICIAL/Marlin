using Marlin.Models;
using Marlin.SystemFiles;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Marlin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menu)
            {

                switch (menu.Header.ToString())
                {
                    case "Настройки": Message.MakeMessage(menu.Header.ToString(), "123", "Info"); break;
                    case "Скрипты": Message.MakeMessage(menu.Header.ToString(), "234", "Error"); break;
                    case "Действия": Message.MakeMessage(menu.Header.ToString(), "345", "Question"); break;
                }

                Context.Settings.Theme = new ProgramColor
                    (
                    pagecolor: Context.Message.PageColor,
                    fontcolor: Context.Message.FontColor,
                    backgroundcolor: Context.Message.BackgroundColor,
                    buttonfontcolor: Context.Message.BackgroundColor
                    );
            }

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MessageBox.Show(Context.Command);
                Settings.SaveSettings();
            }
        }
    }
}
