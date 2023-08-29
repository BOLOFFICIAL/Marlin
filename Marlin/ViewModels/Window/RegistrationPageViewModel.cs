using Marlin.SystemFiles;
using Marlin.ViewModels.Base;

namespace Marlin.ViewModels.Window
{
    public class RegistrationPageViewModel : ViewModel
    {
        public string PageColor
        {
            get => Context.Settings.Theme.PageColor;
        }

        public bool Аutorun
        {
            get => Context.Settings.IsАutorun;
            set => Set(ref Context.Settings.IsАutorun, value);
        }

        public string FontColor
        {
            get => Context.Settings.Theme.FontColor;
        }

        public string BackgroundColor
        {
            get => Context.Settings.Theme.ExternalBackgroundColor;
        }

        public string[] Genders
        {
            get => Context.Settings.Genders;
        }

        public string NewGender
        {
            get => Context.Settings.Gender;
            set => Set(ref Context.Settings.Gender, value);
        }

        public string NewMainFolder
        {
            get => Context.Settings.MainFolder;
            set => Set(ref Context.Settings.MainFolder, value);
        }

        public string NewPassword
        {
            get => Context.Settings.NewPassword;
            set => Set(ref Context.Settings.NewPassword, value);
        }

        public string NewLogin
        {
            get => Context.Settings.Login;
            set => Set(ref Context.Settings.Login, value);
        }
    }
}
