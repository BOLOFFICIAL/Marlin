using Marlin.SystemFiles;
using System.Windows.Controls;

namespace Marlin.Views.Message
{
    public partial class MessagePage : Page
    {
        public MessagePage()
        {
            InitializeComponent();
            Context.MainWindow.Topmost = false;
            Context.Window.Topmost = true;
        }
    }
}
