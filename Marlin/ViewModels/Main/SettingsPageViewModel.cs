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

        public int[] Speeds
        {
            get => Context.Settings.Speeds;
        }

        public int Speed
        {
            get => Context.Settings.Speed;
            set
            {
                Set(ref Context.Settings.Speed, value);
                Voix.SpeakAsync($"Установлена скорость озвучивания {value}");
            }
        }

        public string[] Genders
        {
            get => Context.Settings.Genders;
        }

        public string NewGender
        {
            get => Context.Settings.NewGender;
            set => Set(ref Context.Settings.NewGender, value);
        }

        public string NewMainFolder
        {
            get => Context.Settings.NewMainFolder;
            set => Set(ref Context.Settings.NewMainFolder, value);
        }

        public string NewPassword
        {
            get => Context.Settings.NewPassword;
            set => Set(ref Context.Settings.NewPassword, value);
        }

        public string NewLogin
        {
            get => Context.Settings.NewLogin;
            set => Set(ref Context.Settings.NewLogin, value);
        }
    }
}
