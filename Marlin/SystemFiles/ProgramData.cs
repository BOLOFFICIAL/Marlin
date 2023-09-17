using Marlin.Models;
using Marlin.Models.Main;
using Marlin.SystemFiles.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Marlin.SystemFiles
{
    public class ProgramData
    {
        public List<Command> Commands = new();
        public List<Script> Scripts = new();

        public static async Task SaveData()
        {
            string filepath = System.IO.Path.Combine(Context.Settings.MainFolderPath, "MarlinProgramData.json");
            string programdata = JsonConvert.SerializeObject(Context.ProgramData);
            try
            {
                using (var sw = new StreamWriter(filepath))
                {
                    await sw.WriteAsync(programdata);
                    await sw.FlushAsync();
                }
                Sound.PlaySoundAsync(MessageType.Info);
            }
            catch (Exception)
            {
                MessageBox.MakeMessage("Возникла ошибка сохранения данных", MessageType.Error);
            }
        }

        public static void LoadData()
        {
            string filepath = System.IO.Path.Combine(Context.Settings.MainFolderPath, "MarlinProgramData.json");
            string programdata;
            if (File.Exists(filepath))
            {
                try
                {
                    using (var sr = new StreamReader(filepath))
                    {
                        programdata = sr.ReadLine();
                    }
                    if (programdata is null || programdata.Length == 0)
                    {
                        return;
                    }
                    Context.ProgramData = JsonConvert.DeserializeObject<ProgramData>(programdata);
                }
                catch (Exception)
                {
                    MessageBox.MakeMessage("Возникла ошибка чтения данных", MessageType.Error);
                }
            }
            else
            {
                Context.ProgramData = new();
            }
        }

        public static void MoveData(string oldFolderPath, string newFolderPath)
        {
            try
            {
                string fileName = "MarlinProgramData.json";
                string sourcePath = Path.Combine(oldFolderPath, fileName);
                string destinationPath = Path.Combine(newFolderPath, fileName);

                if (File.Exists(sourcePath))
                {
                    Task.Run(() => File.Move(sourcePath, destinationPath));
                }
                else
                {
                    Models.MessageBox.MakeMessage("Не удалось найти фаил", MessageType.Error);
                }
            }
            catch
            {
                Models.MessageBox.MakeMessage("Возникла ошибка при перемещении", MessageType.Error);
            }
        }
    }
}
