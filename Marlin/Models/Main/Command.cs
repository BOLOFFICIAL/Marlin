using Marlin.SystemFiles;
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
            if (SelectedAction == "Сделать свое действие")
            {
                if (IsReadyCmdCommand)
                {
                    WinSystem.RunCmd(CmdCommand);
                }
                else
                {
                    if (SelectedObjectAction == "Открыть")
                    {
                        if (Apppath.Length > 0)
                        {
                            Process.Start(Apppath, Filepath);
                        }
                        else
                        {
                            if (SelectedObject == "Url")
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
                    if (SelectedObjectAction == "Закрыть")
                    {
                        Models.MessageBox.MakeMessage("В разработке");
                    }
                    if (SelectedObjectAction == "Удалить")
                    {
                        if (SelectedObject == "Файл")
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
                        else if (SelectedObject == "Папка")
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
            else
            {
                Models.MessageBox.MakeMessage("В разработке");
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
        }

        public static void CheckCommands()
        {
            var filteredCommands =
                Context.ProgramData.Commands
                .Where(command => command.SelectedAction == "Сделать свое действие" &&
                !command.IsReadyCmdCommand &&
                ((command.SelectedObject == "Файл" &&
                !File.Exists(command.Filepath)) ||
                (command.SelectedObject == "Папка" &&
                !Directory.Exists(command.Filepath))))
                .Select(command => command.id)
                .ToList();

            foreach (var id in filteredCommands)
            {
                RemoveCommand(id);
            }
        }
    }
}
