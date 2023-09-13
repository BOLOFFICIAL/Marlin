using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;

namespace Marlin.Models.Main
{
    public class Command
    {
        public List<Trigger> Triggers = new List<Trigger>();
        public int id = 0;
        public string Title = "";
        public string Filepath = "";
        public string FileName = "";
        public string Apppath = "";
        public string AppName = "";
        public string Url = "";
        public bool Checkpuss = false;
        public string ResultCommand = "";
        public string SelectedAction = Program.Actions[0];
        public string SelectedEmbeddedAction = Program.EmbeddedActions[0];
        public string SelectedObject = Program.Objects[0];
        public string SelectedObjectAction = Program.ObjectActions[0];
        public string CmdCommand = "";
        public string PressingKeys = "";
        public bool IsReadyCmdCommand = false;
        public bool IsMultiSymbol = false;
        public string Comment = "";
        public string X = "";
        public string Y = "";
        public GridLength LengthObjectAction = GridLength.Auto;
        public GridLength LengthMultiSymbol = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthSymbolCode = GridLength.Auto;
        public GridLength LengthTextToSpeech = GridLength.Auto;
        public GridLength LengthPressingKeys = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthMovingCursor = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthChoseObject = GridLength.Auto;
        public GridLength LengthChoseApp = GridLength.Auto;
        public GridLength LengthInputUrl = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthEmbeddedActions = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthOwnActions = GridLength.Auto;
        public GridLength LengthReadyCmdCommand = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthCommandConstructor = GridLength.Auto;

        public void AddTrigger(string value, TriggerType triggertype)
        {
            Triggers.Add(new Trigger
            {
                id = Context.Command.Triggers.Count,
                value = value,
                triggertype = triggertype,
            });
        }

        public static Command GetCommand(int Id)
        {
            foreach (var command in ProgramData.Commands)
            {
                if (command.id == Id)
                {
                    return command;
                }
            }
            return null;
        }

        public static Command GetCommand(string Title)
        {
            foreach (var command in ProgramData.Commands)
            {
                if (command.Title == Title)
                {
                    return command;
                }
            }
            return null;
        }

        public static void SetCommand(int id,Command newCommand) 
        {
            for (int i=0;i<ProgramData.Commands.Count;i++) 
            {
                if (ProgramData.Commands[i].id == id)
                {
                    ProgramData.Commands[i] = newCommand;
                    return;
                }
            }
        }

        public static string MakeResultCommand(Command command)
        {
            string result = "";
            if (command.IsReadyCmdCommand)
            {
                result = command.CmdCommand;
            }
            return result;
        }

        public bool Equals(Command otherCommand)
        {
            if (otherCommand is null)
            {
                return false;
            }

            string thiscommand = JsonConvert.SerializeObject(this);
            string othercommand = JsonConvert.SerializeObject(otherCommand);

            if (thiscommand.Length != othercommand.Length)
            {
                return false;
            }
            for (int i = 0; i < thiscommand.Length; i++)
            {
                if (thiscommand[i] != othercommand[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
