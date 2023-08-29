using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Marlin.Views.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Marlin.ViewModels.Main
{
    public class MainWindowViewModel : ViewModel
    {
        private Page _currentPage;

        public MainWindowViewModel() 
        {
            CurrentPage = new MainPage();
        }

        public Page CurrentPage
        {
            get => _currentPage;
            set => Set(ref _currentPage, value);
        }

        public void SetPage(Page page) 
        {
            CurrentPage = new SettingsPage();
        }
    }
}
