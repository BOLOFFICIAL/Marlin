using Marlin.SystemFiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Documents;
using System.Windows.Media;

namespace Marlin.Models
{
    public class Settings
    {
        public ProgramColor Theme { get; set; } = new ProgramColor();
        public string Password  = "";
        public string MainFolder = "";
        public bool IsSay = true;
        public int Speed = 0;
        public int[] Speeds = { 1, 2, 3, 4, 5, 6 };

        public static void SaveSettings()
        {
            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string filepath = Path.Combine(exePath, "Settings.json");
            string settings = JsonConvert.SerializeObject(Context.Settings);
            try
            {
                using (var sw = new StreamWriter(filepath))
                {
                    sw.WriteAsync(settings);
                }
                MessageBox.MakeMessage("Для обновления всех настроек необходимо перезапустить приложение.\nПерезапустить?",MessageType.YesNoQuestion);
                if (Context.MessageBox.Answer == "Yes") 
                {
                    Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                    Environment.Exit(0);
                }
            }
            catch (Exception)
            {
                MessageBox.MakeMessage("Возникла ошибка сохранения данных", MessageType.Error);
            }
        }

        public static void LoadSettings()
        {
            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string filepath = Path.Combine(exePath, "Settings.json");
            string settings;
            if (File.Exists(filepath))
            {
                try
                {
                    using (var sr = new StreamReader(filepath))
                    {
                        settings = sr.ReadLine();
                    }
                    Context.Settings = JsonConvert.DeserializeObject<Settings>(settings);
                }
                catch (Exception)
                {
                    MessageBox.MakeMessage("Возникла ошибка чтения данных", MessageType.Error);
                }
            }
        }
    }
}
