using Marlin.Commands;
using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Marlin.ViewModels.Window
{
    public class MessagePageViewModel : ViewModel
    {
        public ICommand SendAnswerCommand { get; }

        public MessagePageViewModel()
        {
            SendAnswerCommand = new LambdaCommand(OnSendAnswerCommandExecute, CanSendAnswerCommandExecute);
        }

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

        public string Answer
        {
            get => Context.MessageBox.Answer;
            set => Set(ref Context.MessageBox.Answer, value);
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

        private bool CanSendAnswerCommandExecute(object parameter)
        {
            return true;
        }

        private void OnSendAnswerCommandExecute(object parameter)
        {
            if (parameter.ToString() != "➤")
            {
                Context.MessageBox.Answer = parameter.ToString();
            }
            Context.Window.Close();
            Voix.SpeakAsync("");
        }
    }
}
