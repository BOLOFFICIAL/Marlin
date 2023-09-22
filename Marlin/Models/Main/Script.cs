using Marlin.SystemFiles;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Marlin.Models.Main
{
    public class Script
    {
        public int id = 1;
        public string Title = "";
        public bool Checkpuss = false;
        public int TimeDelay = 0;
        public string Comment = "";
        public List<int> Commands = new();
        public List<Trigger> Triggers = new List<Trigger>();

        public static Script GetScript(int Id)
        {
            foreach (var script in Context.ProgramData.Scripts)
            {
                if (script.id == Id)
                {
                    return script;
                }
            }
            return null;
        }

        public static Script GetScript(string Title)
        {
            foreach (var script in Context.ProgramData.Scripts)
            {
                if (script.Title == Title)
                {
                    return script;
                }
            }
            return null;
        }

        public static void SetScript(int id, Script newScript)
        {
            for (int i = 0; i < Context.ProgramData.Scripts.Count; i++)
            {
                if (Context.ProgramData.Scripts[i].id == id)
                {
                    Context.ProgramData.Scripts[i] = newScript;
                    return;
                }
            }
        }

        public static void AddScript(Script script)
        {
            if (Context.ProgramData.Scripts.Count > 0)
            {
                script.id = Context.ProgramData.Scripts[Context.ProgramData.Scripts.Count - 1].id + 1;
            }
            else
            {
                script.id = 1;
            }
            Context.ProgramData.Scripts.Add(script);
        }

        public static void RemoveScript(int selectedid)
        {
            Context.ProgramData.Scripts.Remove(Script.GetScript(selectedid));
        }

        public void ExecuteScript()
        {
            int delay = TimeDelay == 0 ? 5 : TimeDelay * 10;

            Task.Run(() =>
            {
                foreach (var commandindex in Commands)
                {
                    var command = Context.ProgramData.Commands[commandindex - 1];
                    if (command != null)
                    {
                        command.ExecuteCommand();
                        Thread.Sleep(delay * 100);
                    }
                }
            });
        }
    }
}
