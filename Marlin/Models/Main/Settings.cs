using Marlin.SystemFiles;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Marlin.Models
{
    public class Settings
    {
        public Theme Theme = new Theme();
        public string Password = "";
        public string NewPassword = "";
        public string Login = "";
        public string NewLogin = "";
        public string MainFolder = "";
        public string NewMainFolder = "";
        public bool IsSay = true;
        public int Speed = 3;
        public string Gender = "";
        public string NewGender = "";
        public int[] Speeds = Enumerable.Range(-10, 21).ToArray();
        public string[] Genders = { "Мужской", "Женский" };

        public static void SaveSettings(bool restart = true)
        {
            Context.Settings.NewPassword = "";
            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string filepath = Path.Combine(exePath, "Settings.json");
            string settings = JsonConvert.SerializeObject(Context.Settings);
            try
            {
                using (var sw = new StreamWriter(filepath))
                {
                    sw.WriteAsync(settings);
                }
                if (restart)
                {
                    MessageBox.MakeMessage("Для обновления всех параметров рекомендую перезапустить приложение.\nПерезапустить?", MessageType.YesNoQuestion);
                    if (Context.MessageBox.Answer == "Yes")
                    {
                        Voix.SpeakAsync("Перезапускаю");
                        Thread.Sleep(1000);
                        Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                        Environment.Exit(0);
                    }
                }
                Context.CopySettings = Context.Settings;
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

        public bool Equals(Settings otherSettings)
        {
            return 
                Theme.ExternalBackgroundColor == otherSettings.Theme.ExternalBackgroundColor &&
                Theme.FontColor == otherSettings.Theme.FontColor &&
                Theme.PageColor == otherSettings.Theme.PageColor &&
                Password == otherSettings.Password &&
                NewPassword == otherSettings.NewPassword &&
                Login == otherSettings.Login &&
                NewLogin == otherSettings.NewLogin &&
                MainFolder == otherSettings.MainFolder &&
                NewMainFolder == otherSettings.NewMainFolder &&
                IsSay == otherSettings.IsSay &&
                Speed == otherSettings.Speed &&
                Gender == otherSettings.Gender &&
                NewGender == otherSettings.NewGender;
        }
    }
}
