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
        public int[] Speeds = Enumerable.Range(-10, 21).ToArray();
        public string[] Genders = { "Мужской", "Женский" };
        public bool IsАutorun = true;
        public string BackgraundImage = "";
        public string BackgraundImagePath = "";
        public string ImageScail = "300";
        public string ImageViewport = "300,300,300,300";


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
