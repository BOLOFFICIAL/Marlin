using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Marlin.Models
{
    public class Settings
    {
        public Theme Theme = new Theme();
        public string Password = "";
        public string NewPassword = "";
        public string Login = "";
        public string MainFolder = "";
        public bool IsSay = true;
        public int Speed = 3;
        public string Gender = "";
        public int[] Speeds = Enumerable.Range(-10, 21).ToArray();
        public string[] Genders = { "Мужской", "Женский" };
        public bool IsАutorun = true;

        public static void SaveSettings(bool restart = true)
        {
            Context.Settings.NewPassword = "";
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string filepath = System.IO.Path.Combine(exePath, "Settings.json");
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
                    Context.Settings = JsonConvert.DeserializeObject<Settings>(settings);
                }
                catch (Exception)
                {
                    MessageBox.MakeMessage("Возникла ошибка чтения данных", MessageType.Error);
                }
            }
        }

        public static void RunCmd(string command) 
        {
            Task.Run(() => 
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.FileName = "cmd.exe";                
                startInfo.RedirectStandardInput = true;        
                startInfo.RedirectStandardOutput = true;       
                startInfo.CreateNoWindow = true;               
                startInfo.UseShellExecute = false;             
                startInfo.StandardOutputEncoding = Encoding.Default;

                process.StartInfo = startInfo;
                process.Start();                               

                process.StandardInput.WriteLine(command);       
                process.StandardInput.Flush();
                process.StandardInput.Close();

                string output = process.StandardOutput.ReadToEnd(); 

                process.WaitForExit();
            });
        }

        public static void AddAutorun() 
        {
            string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string command = "reg add \"HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\" /v Marlin /t REG_SZ /d " + $"{path}";

            RunCmd(command);
        }

        public static void RemoveAutorun()
        {
            string command = $"reg delete \"HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\" /v Marlin /f";
            RunCmd(command);
        }

        public bool Equals(Settings otherSettings)
        {
            return
                Theme.ExternalBackgroundColor == otherSettings.Theme.ExternalBackgroundColor &&
                Theme.InternalBackgroundColor == otherSettings.Theme.InternalBackgroundColor &&
                Theme.FontColor == otherSettings.Theme.FontColor &&
                Theme.PageColor == otherSettings.Theme.PageColor &&
                Password == otherSettings.Password &&
                NewPassword == otherSettings.NewPassword &&
                Login == otherSettings.Login &&
                Login == otherSettings.Login &&
                MainFolder == otherSettings.MainFolder &&
                MainFolder == otherSettings.MainFolder &&
                IsSay == otherSettings.IsSay &&
                Speed == otherSettings.Speed &&
                Gender == otherSettings.Gender &&
                Gender == otherSettings.Gender &&
                IsАutorun == otherSettings.IsАutorun;
        }
    }
}
