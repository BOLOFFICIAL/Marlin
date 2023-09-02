using Marlin.SystemFiles;
using Marlin.Views.Main;
using System;
using System.Windows;

namespace Marlin
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Context.MainWindow = this;
            Context.MainWindow.Content = new MainPage();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
