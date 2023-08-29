using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Marlin.Views.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Marlin.ViewModels.Main
{
    public class WindowViewModel : ViewModel
    {
        private Page _currentPage = new();

        public WindowViewModel()
        {
            CurrentPage = new RegistrationPage();
        }

        public Page CurrentPage
        {
            get => _currentPage;
            set => Set(ref _currentPage, value);
        }
    }
}
