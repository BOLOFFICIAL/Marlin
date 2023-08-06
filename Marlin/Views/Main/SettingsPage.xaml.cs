using Marlin.Models;
using Marlin.SystemFiles;
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
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bool isInvalidColorLength =
                Context.Settings.Theme.PageColor.Length < 7 ||
                Context.Settings.Theme.FontColor.Length < 7 ||
                Context.Settings.Theme.BackgroundColor.Length < 7 ||
                Context.Settings.Theme.PageColor.Length == 8 ||
                Context.Settings.Theme.FontColor.Length == 8 ||
                Context.Settings.Theme.BackgroundColor.Length == 8;

            if (isInvalidColorLength)
            {
                MessageBox.MakeMessage("Значение цвета должно иметь длину 7 или 9 символов", MessageType.Error);
                return;
            }

            bool isSettingsChanged =
                (Context.Settings.NewPassword != Context.Settings.Password && Context.Settings.NewPassword.Length > 0 && Context.Settings.Password.Length > 0) ||
                (Context.Settings.NewLogin != Context.Settings.Login && Context.Settings.Login.Length > 0) ||
                (Context.Settings.NewGender != Context.Settings.Gender && Context.Settings.Gender.Length > 0) ||
                (Context.Settings.NewMainFolder != Context.Settings.MainFolder && Context.Settings.MainFolder.Length > 0);

            if (isSettingsChanged)
            {
                if (Context.Settings.NewLogin.Length < 1 || Context.Settings.NewMainFolder.Length < 1)
                {
                    MessageBox.MakeMessage("Блок администрирования должен быть заполнен полностью", MessageType.Error);
                    return;
                }

                string oldpass = Context.Settings.NewPassword.Length > 0 ? " старый" : "";
                MessageBox.MakeMessage($"Были изменены настройки администрирования.\nДля сохранения введите{oldpass} пароль администратора.", MessageType.TextQuestion);

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
            else if (Context.Settings.Password.Length > 0 ||
                     (Context.Settings.NewPassword.Length > 0 &&
                      Context.Settings.NewLogin.Length > 0 &&
                      Context.Settings.NewMainFolder.Length > 0 &&
                      Context.Settings.NewGender.Length > 0))
            {
                Settings.SaveSettings();
            }
            else
            {
                MessageBox.MakeMessage("Блок администрирования должен быть заполнен", MessageType.Error);
            }
        }


        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Context.Settings = Context.CopySettings;
            NavigationService.Navigate(new MainPage());
        }
    }
}
