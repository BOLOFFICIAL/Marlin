using Marlin.Models;
using Marlin.SystemFiles;
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
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var heckpassowd =
                (Context.Settings.NewPassword != Context.Settings.Password && Context.Settings.NewPassword.Length > 0) ||
                Context.Settings.NewLogin != Context.Settings.Login ||
                Context.Settings.NewGender != Context.Settings.Gender ||
                Context.Settings.NewMainFolder != Context.Settings.MainFolder;
            if (heckpassowd)
            {
                if (Context.Settings.Password.Length > 0)
                {
                    string oldpass = (Context.Settings.NewPassword != Context.Settings.Password && Context.Settings.NewPassword.Length > 0) ? "старый" : "";
                    MessageBox.MakeMessage($"Были изменены настройки администрирования.\nДля сохранения введите {oldpass} пароль администпратора.", MessageType.TextQuestion);
                    if (Context.MessageBox.Answer == Context.Settings.Password)
                    {
                        if (Context.Settings.NewPassword.Length > 0)
                        {
                            Context.Settings.Password = Context.Settings.NewPassword;
                            Context.Settings.NewPassword = "";
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
                    Context.Settings.NewPassword = "";

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
            Context.Settings.NewPassword = "";
            Context.Settings.NewLogin = Context.Settings.Login;
            Context.Settings.NewGender = Context.Settings.Gender;
            Context.Settings.NewMainFolder = Context.Settings.MainFolder;
            NavigationService.Navigate(new MainPage());
        }
    }
}
