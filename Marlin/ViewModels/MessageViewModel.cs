using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Windows;

namespace Marlin.ViewModels
{
    public class MessageViewModel : ViewModel
    {
        public string Message
        {
            get => Context.Message.Text;
            set => Set(ref Context.Message.Text, value);
        }

        public string Title
        {
            get => Context.Message.Title;
            set => Set(ref Context.Message.Title, value);
        }

        public string PageColor
        {
            get => Context.Message.PageColor;
        }

        public string FontColor
        {
            get => Context.Message.FontColor;
        }

        public string BackgroundColor
        {
            get => Context.Message.BackgroundColor;
        }
    }
}
