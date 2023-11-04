using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
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
        public bool execute = true;
        public bool isrun = false;
        public List<int> Commands = new();
        public List<int> Scripts = new();
        public List<int> Actions = new();
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
            foreach (var script in Context.ProgramData.Scripts)
            {
                var resactions = new List<int>();
                var rescommands = new List<int>();
                var resscripts = new List<int>();

                var commandindex = 0;
                var scriptindex = 0;

                for (int i = 0; i < script.Actions.Count; i++)
                {
                    if (script.Actions[i] == 0)
                    {
                        rescommands.Add(script.Commands[commandindex]);
                        resactions.Add(0);
                        commandindex++;
                    }
                    if (script.Actions[i] == 1)
                    {
                        if (script.Scripts[scriptindex] != selectedid)
                        {
                            resscripts.Add(script.Scripts[scriptindex]);
                            resactions.Add(1);
                        }
                        scriptindex++;
                    }
                }

                script.Commands = rescommands;
                script.Actions = resactions;
                script.Scripts = resscripts;
            }
            ProgramData.SaveData();
        }

        public override string ToString()
        {
            return Title;
        }

        public static List<Script> ClearFromZero()
        {
            var deletescript = new List<Script>();
            var resscripts = new List<Script>();

            foreach (var script in Context.ProgramData.Scripts)
            {
                if (script.Actions.Count != 0)
                {
                    resscripts.Add(script);
                }
                else
                {
                    deletescript.Add(script);
                }
            }
            foreach (var script in deletescript)
            {
                Script.RemoveScript(script.id);
            }
            if (deletescript.Count > 0)
            {
                resscripts = ClearFromZero();
            }
            return resscripts;
        }

        public void ExecuteScript()
        {
            int delay = TimeDelay == 0 ? 5 : TimeDelay * 10;
            isrun = true;
            var commandindex = 0;
            var scriptindex = 0;
            foreach (var action in Actions)
            {
                if (action == 0)
                {
                    var command = Command.GetCommand(Commands[commandindex]);
                    if (command != null)
                    {
                        if (execute)
                        {
                            command.ExecuteCommand();
                            if (!(command.SelectedAction == Program.Actions[(int)SystemFiles.Types.ActionsType.builtinmethods] &&
                            command.SelectedEmbeddedAction == Program.EmbeddedActions[(int)EmbeddedActionsType.textspeech]))
                            {
                                Thread.Sleep(delay * 100);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    commandindex++;
                }
                if (action == 1)
                {
                    var script = Script.GetScript(Scripts[scriptindex]);
                    if (script != null)
                    {
                        if (execute)
                        {
                            script.ExecuteScript();
                            Thread.Sleep(delay * 100);
                        }
                        else
                        {
                            return;
                        }
                    }
                    scriptindex++;
                }
            }
            isrun = false;
        }

        public void ExecuteScriptAsync()
        {
            //if (!isrun)
            {
                Task.Run(() =>
                {
                    ExecuteScript();
                });
            }
        }
    }
}
