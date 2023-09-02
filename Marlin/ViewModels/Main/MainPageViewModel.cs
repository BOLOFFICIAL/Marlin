using Marlin.Commands;
using Marlin.Models;
using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Marlin.ViewModels.Main
{
    public class MainPageViewModel : ViewModel
    {
        private bool _openmenu = false;
        private GridLength _length = new GridLength(0, GridUnitType.Pixel);
        private string _command = "";
        private List<Grid> _message = new List<Grid>();
        public ICommand ToSettingsCommand { get; }
        public ICommand SendCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand OpenMenuCommand { get; }


        public MainPageViewModel()
        {
            Context.MainPageVM = this;
            ToSettingsCommand = new LambdaCommand(OnToSettingsCommandExecuted);
            SendCommand = new LambdaCommand(OnSendCommandExecute, CanSendCommandExecute);
            MenuCommand = new LambdaCommand(OnMenuCommandExecute);
            OpenMenuCommand = new LambdaCommand(OnOpenMenuCommandExecute);
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

        public string BackgraundImagePath
        {
            get => Context.Settings.BackgraundImagePath;
        }

        public string ImageViewport
        {
            get => Context.Settings.ImageViewport;
        }

        

        public List<Grid> Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        public GridLength Length
        {
            get => Context.MainPageVM._length;
            set => Set(ref Context.MainPageVM._length, value);
        }

        private void OnToSettingsCommandExecuted(object p)
        {
            Context.CopySettings = JsonConvert.DeserializeObject<Settings>(JsonConvert.SerializeObject(Context.Settings));
            Context.CopySettings.NewPassword = "";
            Context.MainWindow.Content = new SettingsPage();
        }

        private bool CanSendCommandExecute(object parameter)
        {
            return Context.MainPageVM.Command.Length > 0;
        }

        private void OnSendCommandExecute(object parameter)
        {
            Models.MessageBox.MakeMessage(Context.MainPageVM.Command);
            Context.MainPageVM.Command = "";
        }

        private void OnMenuCommandExecute(object parameter)
        {
            Context.MainPageVM.Length = new GridLength(0, GridUnitType.Pixel);
            _openmenu = false;

            switch (parameter.ToString())
            {
                case "Настройки": SystemFiles.System.SetPage(new SettingsPage()); break;
                case "Скрипты": Models.MessageBox.MakeMessage("Страница не доступна", MessageType.Error); break;
                case "Команды": Models.MessageBox.MakeMessage("Страница не доступна", MessageType.YesNoQuestion); break;
            }
        }

        private void OnOpenMenuCommandExecute(object parameter) 
        {
            if (_openmenu)
            {
                Context.MainPageVM.Length = new GridLength(0, GridUnitType.Pixel);
            }
            else 
            {
                Context.MainPageVM.Length = GridLength.Auto;
            }
            _openmenu = !_openmenu;
        }
    }
}
