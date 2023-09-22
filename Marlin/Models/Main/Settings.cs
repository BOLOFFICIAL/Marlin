using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
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
        public string Login = Environment.UserName;
        public string MainFolder = "";
        public string MainFolderPath = "";
        public bool IsSay = true;
        public int Speed = 3;
        public string Gender = "";
        public bool IsАutorun = true;
        public string BackgraundImage = "";
        public string BackgraundImagePath = "";
        public string ImageScail = "300";
        public string ImageViewport = "0,0,1,1";
        public GridLength LengthImage = new GridLength(0, GridUnitType.Pixel);
        public GridLength LengthSay = GridLength.Auto;
        public GridLength LengthScale = new GridLength(0, GridUnitType.Pixel);
        public Stretch Stretch = (Stretch)3;
        public TileMode TileMode = (TileMode)0;
        public BrushMappingMode ViewportUnits = (BrushMappingMode)1;
        public bool Seamless = false;
        public int TimeCheckPassword = 0;

        public static async Task SaveSettings(bool restart = true)
        {
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string filepath = System.IO.Path.Combine(exePath, "Settings.json");
            string Decryptsettings = JsonConvert.SerializeObject(Context.Settings);
            string Encryptsettings = Program.EncryptText(Decryptsettings);
            try
            {
                using (var sw = new StreamWriter(filepath))
                {
                    await sw.WriteAsync(Encryptsettings);
                    await sw.FlushAsync();
                }
                if (restart)
                {
                    MessageBox.MakeMessage("Для обновления всех параметров рекомендую перезапустить приложение.\nПерезапустить?", MessageType.YesNoQuestion);
                    if (Context.MessageBox.Answer == "Yes")
                    {
                        Voix.SpeakAsync("Перезапускаю");
                        Sound.PlaySoundAsync(MessageType.Info);
                        Thread.Sleep(1000);
                        Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                        Environment.Exit(0);
                    }
                }
                Sound.PlaySoundAsync(MessageType.Info);
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
            string Encryptsettings;
            string Decryptsettings;
            if (File.Exists(filepath))
            {
                try
                {
                    using (var sr = new StreamReader(filepath))
                    {
                        Encryptsettings = sr.ReadLine();
                    }
                    if (Encryptsettings is null || Encryptsettings.Length == 0)
                    {
                        return;
                    }
                    Decryptsettings = Program.DecryptText(Encryptsettings);
                    Context.Settings = JsonConvert.DeserializeObject<Settings>(Decryptsettings);
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
    }
}
