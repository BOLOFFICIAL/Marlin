using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using System.Windows.Media;

namespace Marlin.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public string Command
        {
            get => ProgramData.Context.command;
            set => Set(ref ProgramData.Context.command, value);
        }

        public string PageColor
        {
            get => ProgramData.Theme.PageColor;
        }

        public string FontColor
        {
            get => ProgramData.Theme.FontColor;
        }

        public string BackgroundColor
        {
            get => ProgramData.Theme.BackgroundColor;
        }

        public string ButtonfontColor
        {
            get => ProgramData.Theme.ButtonfontColor;
        }
    }
}
