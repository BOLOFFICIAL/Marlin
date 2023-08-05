using Marlin.SystemFiles;
using Marlin.ViewModels.Base;

namespace Marlin.ViewModels.Window
{
    class RegistrationPageViewModel : ViewModel
    {
        public string PageColor
        {
            get => Context.Settings.Theme.PageColor;
        }

        public string FontColor
        {
            get => Context.Settings.Theme.FontColor;
        }

        public string BackgroundColor
        {
            get => Context.Settings.Theme.BackgroundColor;
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
