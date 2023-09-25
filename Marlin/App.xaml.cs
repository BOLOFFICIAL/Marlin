using Marlin.Models;
using Marlin.SystemFiles;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Marlin
{
    public partial class App : Application
    {
        private void StartMarlin()
        {
            var checkpass = false;
            var failauthentication = false;
            foreach (var command in Context.ProgramData.Commands)
            {
                foreach (var trigger in command.Triggers)
                {
                    if (trigger.triggertype == SystemFiles.Types.TriggersType.StartMarlin)
                    {
                        if (command.Checkpuss && failauthentication)
                        {
                            continue;
                        }
                        if (command.Checkpuss && !checkpass)
                        {
                            if (Program.Authentication("Для запуска команды подтвердите пароль"))
                            {
                                checkpass = true;
                            }
                            else
                            {
                                failauthentication = true;
                                continue;
                            }
                        }
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
                        if (script.Checkpuss && failauthentication)
                        {
                            continue;
                        }
                        if (script.Checkpuss && !checkpass)
                        {
                            if (Program.Authentication("Для запуска скрипта подтвердите пароль"))
                            {
                                checkpass = true;
                            }
                            else
                            {
                                return;
                            }
                        }
                        script.ExecuteScript();
                    }
                }
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Settings.LoadSettings();
            ProgramData.LoadData();

            Thread.Sleep(1000);
            WinSystem.CheckRunProcess();

            StartMarlin();
            Voix.SpeakAsync($"С возвращением {Context.Settings.Login}");
            Models.Main.Command.CheckCommands();

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
