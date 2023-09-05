using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Marlin.Models
{
    public class Settings
    {
        public Theme Theme = new Theme();
        public string Password = "";
        public string NewPassword = "";
        public string Login = "";
        public string MainFolder = "";
        public string MainFolderPath = "";
        public bool IsSay = true;
        public int Speed = 3;
        public string Gender = "";
        public int[] Speeds = Enumerable.Range(-5, 16).ToArray();
        public string[] Genders = { "Мужской", "Женский" };
        public bool IsАutorun = true;
        public string BackgraundImage = "";
        public string BackgraundImagePath = "";
        public string ImageScail = "300";
        public string ImageViewport = "0,0,1,1";
        public GridLength LengthImage = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthSay = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthScale = new GridLength(0, GridUnitType.Pixel);
        public Stretch Stretch = (Stretch)3;
        public TileMode TileMode = (TileMode)0;
        public BrushMappingMode ViewportUnits = (BrushMappingMode)1;
        public bool Seamless = false;

        public static async Task SaveSettings(bool restart = true)
        {
            Context.Settings.NewPassword = "";
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string filepath = System.IO.Path.Combine(exePath, "Settings.json");
            string settings = JsonConvert.SerializeObject(Context.Settings);
            try
            {
                Sound.PlaySoundAsync(MessageType.Info);
                using (var sw = new StreamWriter(filepath))
                {
                    await sw.WriteAsync(settings);
                    await sw.FlushAsync();
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
                Context.CopySettings = JsonConvert.DeserializeObject<Settings>(JsonConvert.SerializeObject(Context.Settings));
            }
            catch (Exception)
            {
                MessageBox.MakeMessage("Возникла ошибка сохранения данных", MessageType.Error);
            }
        }

        public static void LoadSettings()
        {
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string filepath = System.IO.Path.Combine(exePath, "Settings.json");
            string settings;
            if (File.Exists(filepath))
            {
                try
                {
                    using (var sr = new StreamReader(filepath))
                    {
                        settings = sr.ReadLine();
                    }
                    if (settings is null || settings.Length == 0)
                    {
                        return;
                    }
                    Context.Settings = JsonConvert.DeserializeObject<Settings>(settings);
                }
                catch (Exception)
                {
                    MessageBox.MakeMessage("Возникла ошибка чтения данных", MessageType.Error);
                }
            }
            else
            {
                Context.Settings = new Settings();
            }
        }

        public bool Equals(Settings otherSettings)
        {
            if (otherSettings is null)
            {
                return false;
            }

            string thissettings = JsonConvert.SerializeObject(this);
            string othersettings = JsonConvert.SerializeObject(otherSettings);

            if (thissettings.Length != othersettings.Length)
            {
                return false;
            }
            for (int i = 0; i < thissettings.Length; i++)
            {
                if (thissettings[i] != othersettings[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
