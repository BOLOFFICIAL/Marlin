using Marlin.Models;
using Marlin.SystemFiles;
using Marlin.ViewModels;
using Marlin.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                    case "Настройки": MessageContext.MakeMessage(menu.Header.ToString(),"123","Info"); break;
                    case "Скрипты": MessageContext.MakeMessage(menu.Header.ToString(), "234", "Error"); break;
                    case "Действия": MessageContext.MakeMessage(menu.Header.ToString(), "345", "Question"); break;
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
            {
                MessageBox.Show(ProgramContext.Command);
            }
        }
    }
}
