using Marlin.SystemFiles;
using Marlin.ViewModels.Base;

namespace Marlin.ViewModels.Main
{
    public class MainPageViewModel : ViewModel
    {
        public string Command
        {
            get => Context.Command;
            set => Set(ref Context.Command, value);
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

        public string ButtonfontColor
        {
            get => Context.Settings.Theme.ButtonfontColor;
        }
    }
}
