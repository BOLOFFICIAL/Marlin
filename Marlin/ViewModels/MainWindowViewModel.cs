using Marlin.SystemFiles;
using Marlin.ViewModels.Base;

namespace Marlin.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public string Command
        {
            get => ProgramContext.Command;
            set => Set(ref ProgramContext.Command, value);
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
