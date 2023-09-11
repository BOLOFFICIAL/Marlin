using System.Collections.Generic;

namespace Marlin.Models.Main
{
    public class Command
    {
        public static List<Command> Commands = new List<Command>();

        public int id = 0;
        public string title = "";
        public string fileputh = "";
        public string appputh = "";
        public bool checkpuss = false;
        public string command = "";
        public List<Trigger> triggers = new List<Trigger>();

        public static void AddCommand(string title, string fileputh, string appputh, bool checkpuss)
        {
            Commands.Add(new Command { id = Commands.Count, title = title, fileputh = fileputh, appputh = appputh, checkpuss = checkpuss });

        }
    }
}
