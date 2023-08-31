using Marlin.SystemFiles;
using Marlin.Views.Main;
using System;
using System.Windows;

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
            Context.MainWindow = this;
            Context.MainWindow.MinWidth = 800;
            Context.MainWindow.MinHeight = 450;
            Context.MainWindow.Content = new MainPage();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
