using Marlin.SystemFiles;
using Marlin.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marlin.Models
{
    public class Message
    {
        public string Title = "";
        public string Text = "";
        public string Type = "";

        public string BackgroundColor = "";
        public string FontColor = "";
        public string PageColor = "";

        public static void MakeMessage(string title, string message,string type)
        {
            Context.Message.Title = title;
            Context.Message.Text = message;
            Context.Message.Type = type;
            SetMessageColor();
            new Windows.Message().ShowDialog();
        }

        public static void SetMessageColor() 
        {
            switch (Context.Message.Type)
            {
                case "Info":
                    Context.Message.BackgroundColor = "#2c3e50";
                    Context.Message.FontColor = "#ffffff";
                    Context.Message.PageColor = "#2980b9";
                    break;
                case "Error":
                    Context.Message.BackgroundColor = "#c0392b";
                    Context.Message.FontColor = "#ffffff";
                    Context.Message.PageColor = "#e74c3c";
                    break;
                case "Question":
                    Context.Message.BackgroundColor = "#27ae60";
                    Context.Message.FontColor = "#ffffff";
                    Context.Message.PageColor = "#2ecc71";
                    break;
            }

        }
    }
}
