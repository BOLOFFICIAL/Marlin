using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace Marlin.Models.Main
{
    public class Command
    {
        public int id = 1;
        public string Title = "";
        public string Filepath = "";
        public string FileName = "";
        public string Apppath = "";
        public string AppName = "";
        public string Url = "";
        public bool isrun = false;
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
        public bool IsExe = false;
        public string Comment = "";
        public string X = "";
        public string Y = "";
        public List<Trigger> Triggers = new List<Trigger>();
        public GridLength LengthObjectAction = GridLength.Auto;
        public GridLength LengthMultiSymbol = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthFileName = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthAppName = new GridLength(0, GridUnitType.Pixel);
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

        public static Command GetCommand(int Id)
        {
            foreach (var command in Context.ProgramData.Commands)
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
            foreach (var command in Context.ProgramData.Commands)
            {
                if (command.Title == Title)
                {
                    return command;
                }
            }
            return null;
        }

        public static void SetCommand(int id, Command newCommand)
        {
            for (int i = 0; i < Context.ProgramData.Commands.Count; i++)
            {
                if (Context.ProgramData.Commands[i].id == id)
                {
                    Context.ProgramData.Commands[i] = newCommand;
                    return;
                }
            }
        }

        public static void AddCommand(Command command)
        {
            if (Context.ProgramData.Commands.Count > 0)
            {
                command.id = Context.ProgramData.Commands[Context.ProgramData.Commands.Count - 1].id + 1;
            }
            else
            {
                command.id = 1;
            }
            Context.ProgramData.Commands.Add(command);
        }

        public void ExecuteCommand()
        {
            if (!isrun)
            {
                isrun = true;
                if (SelectedAction == Program.Actions[(int)ActionsType.ownaction])
                {
                    if (IsReadyCmdCommand)
                    {
                        WinSystem.RunCmd(CmdCommand);
                    }
                    else
                    {
                        if (SelectedObjectAction == Program.ObjectActions[(int)ObjectActionsType.Open])
                        {
                            if (Apppath.Length > 0)
                            {
                                Process.Start(Apppath, Filepath);
                            }
                            else
                            {
                                if (SelectedObject == Program.Objects[(int)ObjectsType.Url])
                                {
                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = Url,
                                        UseShellExecute = true
                                    });
                                }
                                else
                                {
                                    Process.Start("explorer.exe", Filepath);
                                }
                            }
                        }
                        if (SelectedObjectAction == Program.ObjectActions[(int)ObjectActionsType.Close])
                        {
                            Models.MessageBox.MakeMessage("В разработке");
                        }
                        if (SelectedObjectAction == Program.ObjectActions[(int)ObjectActionsType.Delete])
                        {
                            if (SelectedObject == Program.Objects[(int)ObjectsType.File])
                            {
                                if (File.Exists(Filepath))
                                {
                                    File.Delete(Filepath);
                                }
                                else
                                {
                                    Console.WriteLine("Файл не существует.");
                                }
                            }
                            else if (SelectedObject == Program.Objects[(int)ObjectsType.Folder])
                            {
                                if (Directory.Exists(Filepath))
                                {
                                    Directory.Delete(Filepath, true);
                                }
                                else
                                {
                                    Console.WriteLine("Папка не существует.");
                                }
                            }
                        }

                    }
                }
                if (SelectedAction == Program.Actions[(int)ActionsType.builtinmethods])
                {
                    if (SelectedEmbeddedAction == Program.EmbeddedActions[(int)EmbeddedActionsType.movingcursor]) 
                    {
                        if (int.TryParse(X, out int x)&& int.TryParse(Y, out int y)) 
                        {
                            BuiltinMethod.MovingCursor(x, y);
                        }
                    }
                    if (SelectedEmbeddedAction == Program.EmbeddedActions[(int)EmbeddedActionsType.pressingkeys]) 
                    {
                        if (IsMultiSymbol)
                        {
                            BuiltinMethod.PressingKeys(PressingKeys);
                        }
                        else 
                        {
                            if (PressingKeys.Contains(','))
                            {
                                var stringkeys = PressingKeys.Split(",");
                                List<int> intkeys = new List<int>();
                                foreach (var key in stringkeys)
                                {
                                    if (int.TryParse(key,out int k)) 
                                    {
                                        intkeys.Add(k);
                                    }
                                }
                                if (intkeys.Count>0) 
                                {
                                    BuiltinMethod.PressingKeys(intkeys.ToArray());
                                }
                            }
                            else 
                            {
                                if (int.TryParse(PressingKeys,out int key)) 
                                {
                                    BuiltinMethod.PressingKeys(key);
                                }
                            }
                        }
                    }
                }
                isrun = false;
            }
        }

        public override string ToString()
        {
            return Title;
        }

        public static void RemoveCommand(int selectedid)
        {
            Context.ProgramData.Commands.Remove(Models.Main.Command.GetCommand(selectedid));
            foreach (var script in Context.ProgramData.Scripts)
            {
                script.Commands.RemoveAll(id => id == selectedid);
            }
            ProgramData.SaveData();
        }

        public static void CheckCommands()
        {
            var filteredCommandsFile = Context.ProgramData.Commands
                .Where(command =>
                command.SelectedAction == Program.Actions[(int)SystemFiles.Types.ActionsType.ownaction] &&
                !command.IsReadyCmdCommand &&
                (command.SelectedObject == Program.Objects[(int)SystemFiles.Types.ObjectsType.File]) &&
                !File.Exists(command.Filepath))
                .Select(command => command.id)
                .ToList();

            var filteredCommandsFolder = Context.ProgramData.Commands
                .Where(command =>
                command.SelectedAction == Program.Actions[(int)SystemFiles.Types.ActionsType.ownaction] &&
                !command.IsReadyCmdCommand &&
                (command.SelectedObject == Program.Objects[(int)SystemFiles.Types.ObjectsType.Folder]) &&
                !Directory.Exists(command.Filepath))
                .Select(command => command.id)
                .ToList();

            foreach (var id in filteredCommandsFile)
            {
                RemoveCommand(id);
            }

            foreach (var id in filteredCommandsFolder)
            {
                RemoveCommand(id);
            }
        }
    }
}
