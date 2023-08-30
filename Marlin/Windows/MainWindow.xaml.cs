using Marlin.SystemFiles;
using Marlin.Views.Main;
using Marlin.Views.Window;
using System;
using System.Windows;
using System.Windows.Controls;

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
            SetPage(new MainPage());
        }

        public void SetPage(Page page) 
        {
            this.Content = page;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
