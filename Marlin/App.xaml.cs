using Marlin.Models;
using Marlin.SystemFiles;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Marlin
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Settings.LoadSettings();

            Thread.Sleep(300);
            if (CheckRun())
            {
                Models.MessageBox.MakeMessage("Копия Marlin уже запущена");
                Environment.Exit(0);
            }

            if (Context.Settings.Password.Length > 0)
            {
                Voix.SpeakAsync($"С возвращением {Context.Settings.Login}");
            }

            ResourceDictionary applicationResources = this.Resources;

            foreach (ResourceDictionary rd in applicationResources.MergedDictionaries)
            {
                if (rd is MaterialDesignThemes.Wpf.CustomColorTheme Theme)
                {
                    (byte alpha, byte red, byte green, byte blue) = Models.Theme.ConvertHexToArgb(Context.Settings.Theme.PageColor);
                    Color pagecolor = Color.FromArgb(alpha, red, green, blue);
                    Theme.PrimaryColor = pagecolor;
                    Color.FromRgb(140, 0, 0).ToString();
                    Theme.SecondaryColor = pagecolor;

                    break;
                }
            }
        }

        private bool CheckRun()
        {
            string appname = Process.GetCurrentProcess().ProcessName;
            int count = Process.GetProcesses().Count(process => process.ProcessName == appname);
            return count > 1;
        }
    }
}
