using Marlin.SystemFiles;
using Marlin.ViewModels.Base;

namespace Marlin.ViewModels.Main
{
    public class SettingsPageViewModel : ViewModel
    {
        public string PageColor
        {
            get => Context.Settings.Theme.PageColor;
            set
            {
                if (!value.Contains("#"))
                {
                    value = "#" + value;
                }
                Set(ref Context.Settings.Theme.PageColor, value);
            }
        }

        public string FontColor
        {
            get => Context.Settings.Theme.FontColor;
            set
            {
                if (!value.Contains("#"))
                {
                    value = "#" + value;
                }
                Set(ref Context.Settings.Theme.FontColor, value);
            }
        }

        public string ExternalBackgroundColor
        {
            get => Context.Settings.Theme.ExternalBackgroundColor;
            set
            {
                if (!value.Contains("#"))
                {
                    value = "#" + value;
                }
                Set(ref Context.Settings.Theme.ExternalBackgroundColor, value);
            }
        }

        public string InternalBackgroundColor
        {
            get => Context.Settings.Theme.InternalBackgroundColor;
            set
            {
                if (!value.Contains("#"))
                {
                    value = "#" + value;
                }
                Set(ref Context.Settings.Theme.InternalBackgroundColor, value);
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
