using Marlin.Models;
using Marlin.SystemFiles;
using System.IO;
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
                Context.Settings.Login.Length > 0 &&
                Context.Settings.MainFolder.Length > 0 &&
                Context.Settings.Gender.Length > 0)
            {
                if (Context.RegistrationPage.Аutorun) 
                {
                    Settings.AddAutorun();
                }
                Context.Settings.Password = Context.Settings.NewPassword;
                Context.Settings.NewPassword = "";
                Settings.SaveSettings(false);
                Context.Window.Close();
                Voix.SpeakAsync("Приветствую вас " + Context.Settings.Login);
            }
            else
            {
                string name = Context.Settings.Login.Length > 0 ? Context.Settings.Login : "";
                var message = name.Length > 0 ? $"{name}, " : "";
                message += "Необходимо заполнить все пункты";
                Voix.SpeakAsync(message);
                Models.MessageBox.MakeMessage(message);
            }
        }
    }
}
