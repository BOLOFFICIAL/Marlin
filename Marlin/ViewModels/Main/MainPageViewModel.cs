using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Marlin.ViewModels.Main
{
    public class MainPageViewModel : ViewModel
    {
        private string _command = "";
        private List<Grid> _message = new List<Grid>();

        public MainPageViewModel() 
        {
            Context.MainPage = this;
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

        public string BackgroundColor
        {
            get => Context.Settings.Theme.BackgroundColor;
        }

        public List<Grid> Message 
        {
            get => _message;
            set => Set(ref _message, value);
        }
    }
}
