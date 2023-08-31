using Marlin.Commands;
using Marlin.Models;
using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using System.Windows.Input;

namespace Marlin.ViewModels.Main
{
    public class SettingsPageViewModel : ViewModel
    {
        public ICommand ToMainCommand { get; }
        public ICommand SaveSettingsCommand { get; }

        public SettingsPageViewModel()
        {
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
            SaveSettingsCommand = new LambdaCommand(OnSaveSettingsCommandExecuted);
        }

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

        public bool Аutorun
        {
            get => Context.Settings.IsАutorun;
            set => Set(ref Context.Settings.IsАutorun, value);
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

        public string BackgraundImage
        {
            get => Context.Settings.BackgraundImage;
            set => Set(ref Context.Settings.BackgraundImage, value);
        }

        public string ImageViewport 
        {
            get => Context.Settings.ImageViewport;
            set => Set(ref Context.Settings.ImageViewport, value);
        }

        public string ImageScail
        {
            get => Context.Settings.ImageScail;
            set 
            {
                ImageViewport = $"0,0,{value},{value}";
                Set(ref Context.Settings.ImageScail, value);
            } 
        }

        private void OnToMainCommandExecuted(object p)
        {
            Context.Settings = Context.CopySettings;
            Context.MainWindow.Content = new MainPage();
        }

        private void OnSaveSettingsCommandExecuted(object p)
        {
            if (Context.CopySettings.Equals(Context.Settings))
            {
                MessageBox.MakeMessage("Не обнаружено  изменений в настройках");
                return;
            }

            if (Context.Settings.Theme.PageColor.Length < 7 ||
                    Context.Settings.Theme.FontColor.Length < 7 ||
                    Context.Settings.Theme.ExternalBackgroundColor.Length < 7 ||
                    Context.Settings.Theme.PageColor.Length == 8 ||
                    Context.Settings.Theme.FontColor.Length == 8 ||
                    Context.Settings.Theme.ExternalBackgroundColor.Length == 8)
            {
                MessageBox.MakeMessage("Значение цвета должно иметь длину 7 или 9 символов", MessageType.Error);
                return;
            }
            var editadmin =
                (Context.Settings.NewPassword != Context.Settings.Password && Context.Settings.NewPassword.Length > 0) ||
                Context.CopySettings.Login != Context.Settings.Login ||
                Context.CopySettings.Gender != Context.Settings.Gender ||
                Context.CopySettings.MainFolder != Context.Settings.MainFolder ||
                Context.CopySettings.IsАutorun != Context.Settings.IsАutorun;
            if (editadmin)
            {
                if (Context.Settings.Login.Length < 1 ||
                    Context.Settings.MainFolder.Length < 1 ||
                    Context.Settings.Gender.Length < 1)
                {
                    MessageBox.MakeMessage("Блок администрирования должен быть заполнен", MessageType.Error);
                    return;
                }
                if (Context.Settings.Password.Length > 0)
                {
                    string oldpass = (Context.Settings.NewPassword != Context.Settings.Password && Context.Settings.NewPassword.Length > 0) ? "старый" : "";
                    MessageBox.MakeMessage($"Были изменены настройки администрирования.\nДля сохранения введите {oldpass} пароль администпратора.", MessageType.TextQuestion);
                    if (Context.MessageBox.Answer == Context.Settings.Password)
                    {
                        if (Context.Settings.NewPassword.Length > 0)
                        {
                            Context.Settings.Password = Context.Settings.NewPassword;
                        }
                        if (Context.Settings.IsАutorun)
                        {
                            Settings.AddAutorun();
                        }
                        else
                        {
                            Settings.RemoveAutorun();
                        }
                        Settings.SaveSettings();
                    }
                    else
                    {
                        MessageBox.MakeMessage($"Введен неправильный пароль", MessageType.Error);
                    }
                }
                else
                {
                    Context.Settings.Password = Context.Settings.NewPassword;
                    Settings.SaveSettings();
                }
            }
            else
            {
                if (Context.Settings.Password.Length > 0 &&
                    Context.Settings.Login.Length > 0 &&
                    Context.Settings.MainFolder.Length > 0 &&
                    Context.Settings.Gender.Length > 0)
                {
                    Settings.SaveSettings();
                }
                else
                {
                    MessageBox.MakeMessage("Блок администрирования должен быть заполнен", MessageType.Error);
                }
            }
        }
    }
}
