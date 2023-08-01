using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Windows;

namespace Marlin.ViewModels
{
    public class MessageViewModel : ViewModel
    {
        public string Message
        {
            get => ProgramContext.MessageContext.Message;
            set => Set(ref ProgramContext.MessageContext.Message, value);
        }

        public string Title
        {
            get => ProgramContext.MessageContext.Title;
            set => Set(ref ProgramContext.MessageContext.Title, value);
        }

        public string PageColor
        {
            get => ProgramContext.MessageContext.PageColor;
        }

        public string FontColor
        {
            get => ProgramContext.MessageContext.FontColor;
        }

        public string BackgroundColor
        {
            get => ProgramContext.MessageContext.BackgroundColor;
        }
    }
}
