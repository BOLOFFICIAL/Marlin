using Marlin.ViewModels.Base;

namespace Marlin.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private string _command = "";

        public string Command
        {
            get => _command;
            set => Set(ref _command, value);
        }
    }
}
