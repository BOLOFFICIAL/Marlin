using Marlin.Commands;
using Marlin.Models;
using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Marlin.ViewModels.Main
{
    public class MainPageViewModel : ViewModel
    {
        private string _command = "";
        private List<Grid> _message = new List<Grid>();
        public ICommand ToSettingsCommand { get; }
        public ICommand SendCommand { get; }
        public ICommand MenuCommand { get; }


        public MainPageViewModel()
        {
            Context.MainPage = this;
            ToSettingsCommand = new LambdaCommand(OnToSettingsCommandExecuted);
            SendCommand = new LambdaCommand(OnSendCommandExecute, CanSendCommandExecute);
            MenuCommand = new LambdaCommand(OnMenuCommandExecute);

        }

        public string Command
        {
            get => _command;
            set => Set(ref _command, value);
        }

        public string Author
        {
            get => Context.Settings.Login;
        }

        public string PageColor
        {
            get => Context.Settings.Theme.PageColor;
        }

        public string FontColor
        {
            get => Context.Settings.Theme.FontColor;
        }

        public string ExternalBackgroundColor
        {
            get => Context.Settings.Theme.ExternalBackgroundColor;
        }

        public string InternalBackgroundColor
        {
            get => Context.Settings.Theme.InternalBackgroundColor;
        }

        public string BackgraundImage
        {
            get => Context.Settings.BackgraundImage;
        }

        public List<Grid> Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        private void OnToSettingsCommandExecuted(object p)
        {
            Context.CopySettings = JsonConvert.DeserializeObject<Settings>(JsonConvert.SerializeObject(Context.Settings));
            Context.CopySettings.NewPassword = "";
            Context.MainWindow.Content = new SettingsPage();
        }

        private bool CanSendCommandExecute(object parameter)
        {
            return Context.MainPage.Command.Length > 0;
        }

        private void OnSendCommandExecute(object parameter)
        {
            Models.MessageBox.MakeMessage(Context.MainPage.Command);
            Context.MainPage.Command = "";
        }

        private void OnMenuCommandExecute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Настройки": Context.MainWindow.Content = new SettingsPage(); break;
                case "Скрипты": Models.MessageBox.MakeMessage("Страница не доступна", MessageType.Error); break;
                case "Действия": Models.MessageBox.MakeMessage("Страница не доступна", MessageType.YesNoQuestion); break;
            }
        }
    }
}
