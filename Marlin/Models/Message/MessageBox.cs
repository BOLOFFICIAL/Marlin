using Marlin.SystemFiles;
using Marlin.Views.Message;
using System.Windows;

namespace Marlin.Models
{
    public class MessageBox
    {
        public string Symbol = "";
        public string Text = "";
        public MessageType Type = 0;

        public Visibility isTextQuestion = Visibility.Hidden;
        public Visibility isYesNoQuestion = Visibility.Hidden;
        public Visibility isOk = Visibility.Hidden;

        public string BackgroundColor = "";
        public string FontColor = "";
        public string PageColor = "";

        public static void MakeMessage(string message, MessageType type)
        {
            Context.MessageBox.Text = message;
            Context.MessageBox.Type = type;
            SetMessageColor();
            Window window = new Window
            {
                Height = 200,
                Width = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                Content = new MessagePage(),
            };

            Context.MessageWindow = window;

            window.ShowDialog();
        }

        public static void SetMessageColor()
        {
            switch (Context.MessageBox.Type)
            {
                case MessageType.Info:
                    Context.MessageBox.Symbol = "!";
                    Context.MessageBox.BackgroundColor = "#2c3e50";
                    Context.MessageBox.FontColor = "#ffffff";
                    Context.MessageBox.PageColor = "#2980b9";
                    Context.MessageBox.isOk = Visibility.Visible;
                    Context.MessageBox.isTextQuestion = Visibility.Hidden;
                    Context.MessageBox.isYesNoQuestion = Visibility.Hidden;
                    break;
                case MessageType.Error:
                    Context.MessageBox.Symbol = "X";
                    Context.MessageBox.BackgroundColor = "#c0392b";
                    Context.MessageBox.FontColor = "#ffffff";
                    Context.MessageBox.PageColor = "#e74c3c";
                    Context.MessageBox.isOk = Visibility.Visible;
                    Context.MessageBox.isTextQuestion = Visibility.Hidden;
                    Context.MessageBox.isYesNoQuestion = Visibility.Hidden;
                    break;
                case MessageType.YesNoQuestion:
                    Context.MessageBox.Symbol = "?";
                    Context.MessageBox.BackgroundColor = "#27ae60";
                    Context.MessageBox.FontColor = "#ffffff";
                    Context.MessageBox.PageColor = "#2ecc71";
                    Context.MessageBox.isOk = Visibility.Hidden;
                    Context.MessageBox.isTextQuestion = Visibility.Hidden;
                    Context.MessageBox.isYesNoQuestion = Visibility.Visible;
                    break;
                case MessageType.TextQuestion:
                    Context.MessageBox.Symbol = "?";
                    Context.MessageBox.BackgroundColor = "#27ae60";
                    Context.MessageBox.FontColor = "#ffffff";
                    Context.MessageBox.PageColor = "#2ecc71";
                    Context.MessageBox.isOk = Visibility.Hidden;
                    Context.MessageBox.isTextQuestion = Visibility.Visible;
                    Context.MessageBox.isYesNoQuestion = Visibility.Hidden;
                    break;
            }
        }
    }
}
