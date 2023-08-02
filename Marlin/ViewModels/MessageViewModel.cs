using Marlin.SystemFiles;
using Marlin.ViewModels.Base;

namespace Marlin.ViewModels
{
    public class MessageViewModel : ViewModel
    {
        public string Message
        {
            get => Context.MessageBox.Text;
            set => Set(ref Context.MessageBox.Text, value);
        }

        public string Title
        {
            get => Context.MessageBox.Title;
            set => Set(ref Context.MessageBox.Title, value);
        }

        public string PageColor
        {
            get => Context.MessageBox.PageColor;
        }

        public string FontColor
        {
            get => Context.MessageBox.FontColor;
        }

        public string BackgroundColor
        {
            get => Context.MessageBox.BackgroundColor;
        }
    }
}
