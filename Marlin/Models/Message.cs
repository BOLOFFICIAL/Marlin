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
            Context.MessageContext.Title = title;
            Context.MessageContext.Text = message;
            Context.MessageContext.Type = type;
            SetMessageColor();
            new Windows.Message().ShowDialog();
        }

        public static void SetMessageColor() 
        {
            switch (Context.MessageContext.Type)
            {
                case "Info":
                    Context.MessageContext.BackgroundColor = "#2c3e50";
                    Context.MessageContext.FontColor = "#ffffff";
                    Context.MessageContext.PageColor = "#2980b9";
                    break;
                case "Error":
                    Context.MessageContext.BackgroundColor = "#c0392b";
                    Context.MessageContext.FontColor = "#ffffff";
                    Context.MessageContext.PageColor = "#e74c3c";
                    break;
                case "Question":
                    Context.MessageContext.BackgroundColor = "#27ae60";
                    Context.MessageContext.FontColor = "#ffffff";
                    Context.MessageContext.PageColor = "#2ecc71";
                    break;
            }

        }
    }
}
