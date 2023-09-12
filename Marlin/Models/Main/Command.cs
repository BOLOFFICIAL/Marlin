using Marlin.SystemFiles;
using System.Collections.Generic;
using System.Windows;

namespace Marlin.Models.Main
{
    public class Command
    {
        public List<Command> Commands = new List<Command>();

        public int id = 0;
        public string title = "";
        public string fileputh = "";
        public string appputh = "";
        public bool checkpuss = false;
        public string command = "";
        public List<Trigger> triggers = new List<Trigger>();
        public string SelectedAction = "";
        public string SelectedEmbeddedAction = "";
        public string SelectedObject = "";
        public string SelectedTrigger = "";
        public bool IsReadyCmdCommand = false;
        public bool IsMultiSymbol = false;

        public GridLength LengthTextTrigger = GridLength.Auto;
        public GridLength LengthAppTrigger = new GridLength(0, GridUnitType.Pixel);

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

        public static void AddCommand(string title, string fileputh, string appputh, bool checkpuss)
        {
            Context.Command.Commands.Add(new Command
            {
                id = Context.Command.Commands.Count,
                title = title,
                fileputh = fileputh,
                appputh = appputh,
                checkpuss = checkpuss
            });
        }
    }
}
