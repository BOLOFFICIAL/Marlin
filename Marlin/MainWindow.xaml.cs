using Marlin.Models;
using Marlin.SystemFiles;
using System;
using System.Diagnostics;
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
                Context.Settings.Theme = new ProgramColor
            (
            pagecolor: Color.FromRgb(255, 12, 255).ToString(),
            fontcolor: Color.FromRgb(255, 12, 255).ToString(),
            backgroundcolor: Color.FromRgb(255, 0, 102).ToString(),
            buttonfontcolor: Color.FromRgb(255, 0, 102).ToString()
            );
                switch (menu.Header.ToString())
                {
                    case "Настройки": Message.MakeMessage(menu.Header.ToString(), "123", "Info"); break;
                    case "Скрипты": Message.MakeMessage(menu.Header.ToString(), "234", "Error"); break;
                    case "Действия": Message.MakeMessage(menu.Header.ToString(), "345", "Question"); break;
                }
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
