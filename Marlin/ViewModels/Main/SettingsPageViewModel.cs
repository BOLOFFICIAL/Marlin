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

        public bool IsSay
        {
            get => Context.Settings.IsSay;
            set => Set(ref Context.Settings.IsSay, value);
        }
    }
}
