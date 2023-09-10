using Marlin.Commands;
using Marlin.Models.Main;
using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using System.Windows.Input;
using System.Windows.Media;

namespace Marlin.ViewModels.Main
{
    public class CommandPageViewModel : ViewModel
    {
        public ICommand ToMainCommand { get; }
        public ICommand ButtonActionCommand { get; }

        private string title = "";
        private string commandtitle = "";
        private string commandfileputh = "";
        private string commandappputh = "";

        public CommandPageViewModel()
        {
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
            ButtonActionCommand = new LambdaCommand(OnButtonActionCommandExecuted);

            if (Context.SelectedId > -1) 
            {               
                title = Command.Commands[Context.SelectedId].title;
                commandtitle = Command.Commands[Context.SelectedId].title;
                commandfileputh = Command.Commands[Context.SelectedId].fileputh;
                commandappputh = Command.Commands[Context.SelectedId].appputh;
            }
            else
            {
                title = "Создание команды";
            }
        }

        public string ButtonContent
        {
            get 
            {
                if (Context.SelectedId > -1)
                {

                    return "Сохранить";
                }
                else
                {
                    return "Создать";
                }
            } 
        }

        public string Title 
        {
            get => title;
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

        public string ImageScail
        {
            get => Context.Settings.ImageScail;
        }

        public Stretch Stretch
        {
            get => Context.Settings.Stretch;
        }

        public TileMode TileMode
        {
            get => Context.Settings.TileMode;
        }

        public BrushMappingMode ViewportUnits
        {
            get => Context.Settings.ViewportUnits;
        }

        public string CommandTitle
        {
            get 
            {
                return commandtitle;
            }
            set 
            {
                Set(ref commandtitle, value);
            } 
        }

        private void OnToMainCommandExecuted(object p)
        {
            Program.SetPage(new ActionsPage());
        }

        private void OnButtonActionCommandExecuted(object p)
        {
            if (Context.SelectedId > -1) 
            {
                
            }
            else
            {
                Command.AddCommand(CommandTitle, CommandTitle, CommandTitle, true);
            }
            Program.SetPage(new ActionsPage());
        }
    }
}
