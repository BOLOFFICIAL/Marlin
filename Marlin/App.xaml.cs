using Marlin.Models;
using Marlin.Models.Main;
using Marlin.SystemFiles;
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
            ProgramData.LoadData();

            Thread.Sleep(1000);
            WinSystem.CheckRunProcess();

            if (Context.Settings.Password.Length > 0)
            {
                Voix.SpeakAsync($"С возвращением {Context.Settings.Login}");
                Command.CheckCommands();
                foreach (var command in Context.ProgramData.Commands)
                {
                    foreach (var trigger in command.Triggers)
                    {
                        if (trigger.triggertype == SystemFiles.Types.TriggersType.StartMarlin)
                        {
                            command.ExecuteCommand();
                        }
                    }
                }
                foreach (var script in Context.ProgramData.Scripts)
                {
                    foreach (var trigger in script.Triggers)
                    {
                        if (trigger.triggertype == SystemFiles.Types.TriggersType.StartMarlin)
                        {
                            script.ExecuteScript();
                        }
                    }
                }
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
    }
}
