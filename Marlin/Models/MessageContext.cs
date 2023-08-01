using Marlin.SystemFiles;
using Marlin.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marlin.Models
{
    public class MessageContext
    {
        public string Title = "";
        public string Message = "";
        public string Type = "";

        public string BackgroundColor = "";
        public string FontColor = "";
        public string PageColor = "";

        public static void MakeMessage(string title, string message,string type)
        {
            ProgramContext.MessageContext.Title = title;
            ProgramContext.MessageContext.Message = message;
            ProgramContext.MessageContext.Type = type;
            SetMessageColor();
            new Message().ShowDialog();
        }

        public static void SetMessageColor() 
        {
            switch (ProgramContext.MessageContext.Type)
            {
                case "Info":
                    ProgramContext.MessageContext.BackgroundColor = "#2c3e50";
                    ProgramContext.MessageContext.FontColor = "#ffffff";
                    ProgramContext.MessageContext.PageColor = "#2980b9";
                    break;
                case "Error":
                    ProgramContext.MessageContext.BackgroundColor = "#c0392b";
                    ProgramContext.MessageContext.FontColor = "#ffffff";
                    ProgramContext.MessageContext.PageColor = "#e74c3c";
                    break;
                case "Question":
                    ProgramContext.MessageContext.BackgroundColor = "#27ae60";
                    ProgramContext.MessageContext.FontColor = "#ffffff";
                    ProgramContext.MessageContext.PageColor = "#2ecc71";
                    break;
            }

        }
    }
}
