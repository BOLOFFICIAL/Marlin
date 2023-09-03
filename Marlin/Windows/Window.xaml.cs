using Marlin.SystemFiles;
using Marlin.Views.Window;
using System.Windows;

namespace Marlin.Windows
{
    public partial class Message : Window
    {
        public Message()
        {
            InitializeComponent();
            Context.Window = this;
            Context.Window.Content = new RegistrationPage();
            Context.Window.WindowStyle = WindowStyle.None;
        }
    }
}
