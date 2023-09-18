using Marlin.SystemFiles;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
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
                            Process.Start("explorer.exe", Filepath);
                        }
                    }
                    if (SelectedObjectAction == "Закрыть")
                    {

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

        public override string ToString()
        {
            return Title;
        }
    }
}
