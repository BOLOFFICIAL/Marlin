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
                (Context.CopySettings.NewPassword != Context.Settings.Password && Context.CopySettings.NewPassword.Length > 0) ||
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

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Context.Settings = Context.CopySettings;
            NavigationService.Navigate(new MainPage());
        }
    }
}
