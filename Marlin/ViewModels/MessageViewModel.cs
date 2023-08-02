using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Windows;

namespace Marlin.ViewModels
{
    public class MessageViewModel : ViewModel
    {
        public string Message
        {
            get => Context.MessageContext.Text;
            set => Set(ref Context.MessageContext.Text, value);
        }

        public string Title
        {
            get => Context.MessageContext.Title;
            set => Set(ref Context.MessageContext.Title, value);
        }

        public string PageColor
        {
            get => Context.MessageContext.PageColor;
        }

        public string FontColor
        {
            get => Context.MessageContext.FontColor;
        }

        public string BackgroundColor
        {
            get => Context.MessageContext.BackgroundColor;
        }
    }
}
