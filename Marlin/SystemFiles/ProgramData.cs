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
        [JsonIgnore]
        public string Version = "Marlin 25102023";

        public static async Task SaveData()
        {
            string filepath = System.IO.Path.Combine(Context.Settings.MainFolderPath, "MarlinProgramData.json");
            string Decryptprogramdata = JsonConvert.SerializeObject(Context.ProgramData);
            string Encryptprogramdata = Program.EncryptText(Decryptprogramdata, Context.Settings.Password);
            try
            {
                using (var sw = new StreamWriter(filepath))
                {
                    await sw.WriteAsync(Encryptprogramdata);
                    await sw.FlushAsync();
                }
                Sound.PlaySoundAsync(MessageType.Info);
            }
            catch (Exception)
            {
                //MessageBox.MakeMessage("Возникла ошибка сохранения данных", MessageType.Error);
            }
        }

        public static void LoadData()
        {
            string filepath = System.IO.Path.Combine(Context.Settings.MainFolderPath, "MarlinProgramData.json");
            string Encryptprogramdata;
            string Decryptprogramdata;
            if (File.Exists(filepath))
            {
                try
                {
                    using (var sr = new StreamReader(filepath))
                    {
                        Encryptprogramdata = sr.ReadLine();
                    }
                    if (Encryptprogramdata is null || Encryptprogramdata.Length == 0)
                    {
                        return;
                    }
                    Decryptprogramdata = Program.DecryptText(Encryptprogramdata, Context.Settings.Password);
                    Context.ProgramData = JsonConvert.DeserializeObject<ProgramData>(Decryptprogramdata);
                    foreach (var Command in Context.ProgramData.Commands)
                    {
                        Command.isRun = false;
                    }
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
