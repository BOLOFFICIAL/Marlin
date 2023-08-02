using Marlin.Models;
using Marlin.SystemFiles;
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

            ResourceDictionary applicationResources = this.Resources;

            foreach (ResourceDictionary rd in applicationResources.MergedDictionaries)
            {
                if (rd is MaterialDesignThemes.Wpf.CustomColorTheme Theme)
                {
                    (byte alpha, byte red, byte green, byte blue) = Models.ProgramColor.ConvertHexToArgb(Context.Settings.Theme.PageColor);
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
