using Marlin.Models;
using Marlin.SystemFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

            CheckRun();

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

        private void CheckRun()
        {
            [DllImport("user32.dll")]
            static extern bool SetForegroundWindow(IntPtr hWnd);

            string appName = Process.GetCurrentProcess().ProcessName;
            int currentProcessId = Process.GetCurrentProcess().Id;

            List<int> processIds = GetProcessIdsByName(appName);
            processIds.Remove(currentProcessId);

            if (processIds.Count > 0)
            {
                int targetProcessId = processIds[0];

                try
                {
                    Process targetProcess = Process.GetProcessById(targetProcessId);
                    SetForegroundWindow(targetProcess.MainWindowHandle);
                    Process currentProcess = Process.GetCurrentProcess();
                    currentProcess.Kill();
                }
                catch (ArgumentException)
                {
                    Models.MessageBox.MakeMessage($"Процесс с ID {targetProcessId} не найден.");
                }
            }
        }

        static List<int> GetProcessIdsByName(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Select(process => process.Id).ToList();
        }
    }
}
