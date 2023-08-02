using Marlin.SystemFiles;
using Marlin.Views;
using System.Windows;

namespace Marlin.Models
{
    public class MessageBox
    {
        public string Title = "";
        public string Text = "";
        public string Type = "";

        public string BackgroundColor = "";
        public string FontColor = "";
        public string PageColor = "";

        public static void MakeMessage(string title, string message, string type)
        {
            Context.MessageBox.Title = title;
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

            window.ShowDialog();
        }

        public static void SetMessageColor()
        {
            switch (Context.MessageBox.Type)
            {
                case "Info":
                    Context.MessageBox.BackgroundColor = "#2c3e50";
                    Context.MessageBox.FontColor = "#ffffff";
                    Context.MessageBox.PageColor = "#2980b9";
                    break;
                case "Error":
                    Context.MessageBox.BackgroundColor = "#c0392b";
                    Context.MessageBox.FontColor = "#ffffff";
                    Context.MessageBox.PageColor = "#e74c3c";
                    break;
                case "Question":
                    Context.MessageBox.BackgroundColor = "#27ae60";
                    Context.MessageBox.FontColor = "#ffffff";
                    Context.MessageBox.PageColor = "#2ecc71";
                    break;
            }

        }
    }
}
