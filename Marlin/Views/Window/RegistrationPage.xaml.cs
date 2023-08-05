using Marlin.Models;
using Marlin.SystemFiles;
using System.Windows;
using System.Windows.Controls;

namespace Marlin.Views.Window
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Context.Settings.NewPassword.Length > 0 &&
                Context.Settings.NewLogin.Length > 0 &&
                Context.Settings.NewMainFolder.Length > 0 &&
                Context.Settings.NewGender.Length > 0)
            {
                Context.Settings.Password = Context.Settings.NewPassword;
                Context.Settings.NewPassword = "";
                Context.Settings.Login = Context.Settings.NewLogin;
                Context.Settings.MainFolder = Context.Settings.NewMainFolder;
                Context.Settings.Gender = Context.Settings.NewGender;
                Settings.SaveSettings(false);
                Context.Window.Close();
                Voix.SpeakAsync("Приветствую вас " + Context.Settings.NewLogin);
            }
            else
            {
                string name = Context.Settings.NewLogin.Length > 0 ? Context.Settings.NewLogin : "";
                Voix.SpeakAsync(name + " Необходимо заполнить все пункты");
            }
        }
    }
}
