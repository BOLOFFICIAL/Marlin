using Marlin.Commands;
using Marlin.Models;
using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows.Input;

namespace Marlin.ViewModels.Window
{
    public class RegistrationPageViewModel : ViewModel
    {
        public ICommand RegistrationCommand { get; }
        public ICommand ChoseCommand { get; }

        public RegistrationPageViewModel()
        {
            RegistrationCommand = new LambdaCommand(OnRegistrationCommandExecute, CanRegistrationCommandExecute);
            ChoseCommand = new LambdaCommand(OnChoseCommandExecuted);
        }

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
            get => Program.Genders;
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

        private bool CanRegistrationCommandExecute(object parameter)
        {
            return
                Context.Settings.NewPassword.Length > 0 &&
                Context.Settings.Login.Length > 0 &&
                Context.Settings.MainFolder.Length > 0 &&
                Context.Settings.Gender.Length > 0;
        }

        private void OnRegistrationCommandExecute(object parameter)
        {
            if (Аutorun)
            {
                Program.AddAutorun();
            }
            else
            {
                Program.RemoveAutorun();
            }
            Context.Settings.Password = Context.Settings.NewPassword;
            Context.Settings.NewPassword = "";
            Settings.SaveSettings(false);
            Context.Window.Close();
            Voix.SpeakAsync("Приветствую вас " + Context.Settings.Login);
        }

        private void OnChoseCommandExecuted(object p)
        {
            CommonOpenFileDialog folderPicker = new CommonOpenFileDialog();

            folderPicker.IsFolderPicker = true;
            folderPicker.Title = "Выбор папки для хранения данных";

            CommonFileDialogResult dialogResult = folderPicker.ShowDialog();

            if (dialogResult == CommonFileDialogResult.Ok)
            {
                string selectedFolderPath = folderPicker.FileName;
                Context.Settings.MainFolderPath = selectedFolderPath;
                NewMainFolder = Path.GetFileName(selectedFolderPath);
            }
        }
    }
}
