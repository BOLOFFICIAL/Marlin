using Marlin.SystemFiles;
using Marlin.Views.Window;
using System;
using System.Windows;

namespace Marlin.Windows
{
    public partial class Message : Window
    {
        public Message()
        {
            InitializeComponent();
            this.Topmost = true;
            Context.Window = this;
            Context.Window.Content = new RegistrationPage();
            Context.Window.WindowStyle = WindowStyle.None;
        }
    }
}
