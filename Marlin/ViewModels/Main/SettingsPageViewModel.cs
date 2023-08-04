using Marlin.SystemFiles;
using Marlin.ViewModels.Base;

namespace Marlin.ViewModels.Main
{
    class SettingsPageViewModel : ViewModel
    {
        public string PageColor
        {
            get => Context.Settings.Theme.PageColor;
            set
            {
                if (value[0] == '#' && (value.Length == 9 || value.Length == 7))
                {
                    Set(ref Context.Settings.Theme.PageColor, value);
                }
            }
        }

        public string FontColor
        {
            get => Context.Settings.Theme.FontColor;
            set
            {
                if (value[0] == '#' && (value.Length == 9 || value.Length == 7))
                {
                    Set(ref Context.Settings.Theme.FontColor, value);
                }
            }
        }

        public string BackgroundColor
        {
            get => Context.Settings.Theme.BackgroundColor;
            set
            {
                if (value[0] == '#' && (value.Length == 9 || value.Length == 7))
                {
                    Set(ref Context.Settings.Theme.BackgroundColor, value);
                }
            }
        }

        public bool IsSay
        {
            get => Context.Settings.IsSay;
            set => Set(ref Context.Settings.IsSay, value);
        }

        public int[] Speeds
        {
            get => Context.Settings.Speeds;
        }

        public int Speed
        {
            get => Context.Settings.Speed;
            set => Set(ref Context.Settings.Speed, value);
        }
    }
}
