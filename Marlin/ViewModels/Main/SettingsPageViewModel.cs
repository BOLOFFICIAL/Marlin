using Marlin.SystemFiles;
using Marlin.ViewModels.Base;

namespace Marlin.ViewModels.Main
{
    class SettingsPageViewModel : ViewModel
    {
        public string PageColor
        {
            get => Context.Settings.Theme.PageColor;
            set => Set(ref Context.Settings.Theme.PageColor, value);
        }

        public string FontColor
        {
            get => Context.Settings.Theme.FontColor;
            set => Set(ref Context.Settings.Theme.FontColor, value);
        }

        public string BackgroundColor
        {
            get => Context.Settings.Theme.BackgroundColor;
            set => Set(ref Context.Settings.Theme.BackgroundColor, value);
        }

        public string ButtonfontColor
        {
            get => Context.Settings.Theme.ButtonfontColor;
            set => Set(ref Context.Settings.Theme.ButtonfontColor, value);
        }
    }
}
