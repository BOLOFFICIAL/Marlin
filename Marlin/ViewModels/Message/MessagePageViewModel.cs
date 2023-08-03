using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using System.Windows;

namespace Marlin.ViewModels.Message
{
    public class MessagePageViewModel : ViewModel
    {
        public string Message
        {
            get => Context.MessageBox.Text;
            set => Set(ref Context.MessageBox.Text, value);
        }

        public string Symbol
        {
            get => Context.MessageBox.Symbol;
            set => Set(ref Context.MessageBox.Symbol, value);
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

        public Visibility IsOk
        {
            get => Context.MessageBox.isOk;
        }

        public Visibility IsTextQuestion
        {
            get => Context.MessageBox.isTextQuestion;
        }

        public Visibility IsYesNoQuestion
        {
            get => Context.MessageBox.isYesNoQuestion;
        }
    }
}
