using Marlin.SystemFiles;
using Marlin.ViewModels;
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
                MessageBox.Show(ProgramData.Theme.FontColor+"\t\t\t"+menu.Header.ToString());
            }   
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
            {
                MessageBox.Show(ProgramData.Context.command);
            }
        }
    }
}
