using Marlin.SystemFiles;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace Marlin.Models
{
    public class Settings
    {
        public ProgramColor Theme { get; set; } = new ProgramColor
            (
            pagecolor: Color.FromRgb(255, 255, 255).ToString(),
            fontcolor: Color.FromRgb(255, 255, 255).ToString(),
            backgroundcolor: Color.FromRgb(255, 0, 102).ToString(),
            buttonfontcolor: Color.FromRgb(255, 0, 102).ToString()
            );
        public string Password { get; set; } = "";
        public string MainFolder { get; set; } = "";
        public Voice Voice { get; set; } = new Voice();
        public bool IsSay { get; set; } = true;

        public void SetTheme(Color pagecolor, Color fontcolor, Color backgroundcolor, Color buttonfontcolor)
        {
            Theme = new ProgramColor(pagecolor.ToString(), fontcolor.ToString(), backgroundcolor.ToString(), buttonfontcolor.ToString());
        }
        public void SetTheme(ProgramColor newtheme)
        {
            Theme = newtheme;
        }

        public void SetVoise(string voise, int speed)
        {
            Voice = new Voice()
            {
                Voise = voise,
                Speed = speed
            };
        }

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
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Environment.Exit(0);
            }
            catch (Exception)
            {
                throw;
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

                    throw;
                }
            }
        }
    }
}
