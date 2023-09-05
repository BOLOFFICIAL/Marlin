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
using System.Windows.Media;

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
            get => _length;
            set => Set(ref _length, value);
        }

        public Stretch Stretch
        {
            get => Context.Settings.Stretch;
            set => Set(ref Context.Settings.Stretch, value);
        }

        public TileMode TileMode
        {
            get => Context.Settings.TileMode;
            set => Set(ref Context.Settings.TileMode, value);
        }

        public BrushMappingMode ViewportUnits
        {
            get => Context.Settings.ViewportUnits;
            set => Set(ref Context.Settings.ViewportUnits, value);
        }

        private void OnToSettingsCommandExecuted(object p)
        {
            Context.CopySettings = JsonConvert.DeserializeObject<Settings>(JsonConvert.SerializeObject(Context.Settings));
            Context.CopySettings.NewPassword = "";
            Context.MainWindow.Content = new SettingsPage();
        }

        private bool CanSendCommandExecute(object parameter)
        {
            return Command.Length > 0;
        }

        private void OnSendCommandExecute(object parameter)
        {
            Models.MessageBox.MakeMessage(Command);
            Command = "";
        }

        private void OnMenuCommandExecute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Настройки": Program.SetPage(new SettingsPage()); break;
                case "Команды": Models.MessageBox.MakeMessage("Страница не доступна", MessageType.Error); break;
                case "Скрипты": Models.MessageBox.MakeMessage("Страница не доступна", MessageType.Error); break;
            }
        }

        private void OnOpenMenuCommandExecute(object parameter)
        {
            if (_openmenu)
            {
                Length = new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                Length = GridLength.Auto;
            }
            _openmenu = !_openmenu;
        }
    }
}
