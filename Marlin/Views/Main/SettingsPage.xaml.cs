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
            var editadmin =
                (Context.Settings.NewPassword != Context.Settings.Password && Context.Settings.NewPassword.Length > 0) ||
                Context.Settings.NewLogin != Context.Settings.Login ||
                Context.Settings.NewGender != Context.Settings.Gender ||
                Context.Settings.NewMainFolder != Context.Settings.MainFolder;
            if (editadmin)
            {
                if (Context.Settings.NewLogin.Length == 0)
                {
                    MessageBox.MakeMessage($"Имя администратора не может быть пустым", MessageType.Error);
                    return;
                }
                if (Context.Settings.NewMainFolder.Length == 0)
                {
                    MessageBox.MakeMessage($"Папка для хранения данных не может быть пустой", MessageType.Error);
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
                Settings.SaveSettings();
            }
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Context.Settings = Context.CopySettings;
            NavigationService.Navigate(new MainPage());
        }
    }
}
