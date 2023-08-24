using Marlin.Models;
using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Newtonsoft.Json;
using System.Windows.Controls;

namespace Marlin.Views.Main
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            Context.Settings.NewLogin = Context.Settings.Login;
            Context.Settings.NewGender = Context.Settings.Gender;
            Context.Settings.NewMainFolder = Context.Settings.MainFolder;
            InitializeComponent();
            Context.CopySettings = JsonConvert.DeserializeObject<Settings>(JsonConvert.SerializeObject(Context.Settings));
            Context.CopySettings.NewPassword = "";
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
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
                Context.Settings.NewLogin != Context.Settings.Login ||
                Context.Settings.NewGender != Context.Settings.Gender ||
                Context.Settings.NewMainFolder != Context.Settings.MainFolder;
            if (editadmin)
            {
                if (Context.Settings.NewLogin.Length < 1 ||
                    Context.Settings.NewMainFolder.Length < 1 ||
                    Context.Settings.NewGender.Length < 1)
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
                        Context.Settings.Login = Context.Settings.NewLogin;
                        Context.Settings.Gender = Context.Settings.NewGender;
                        Context.Settings.MainFolder = Context.Settings.NewMainFolder;
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
                    Context.Settings.Login = Context.Settings.NewLogin;
                    Context.Settings.Gender = Context.Settings.NewGender;
                    Context.Settings.MainFolder = Context.Settings.NewMainFolder;
                    Settings.SaveSettings();
                }
            }
            else
            {
                if (Context.Settings.Password.Length > 0 &&
                    Context.Settings.NewLogin.Length > 0 &&
                    Context.Settings.NewMainFolder.Length > 0 &&
                    Context.Settings.NewGender.Length > 0)
                {
                    Settings.SaveSettings();
                }
                else
                {
                    MessageBox.MakeMessage("Блок администрирования должен быть заполнен", MessageType.Error);
                }
            }
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Context.Settings = Context.CopySettings;
            NavigationService.Navigate(new MainPage());
        }
    }
}
