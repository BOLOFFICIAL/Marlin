using Marlin.Commands;
using Marlin.Models.Main;
using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using System;
using System.Windows;
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
                title = Context.Command.Commands[Context.SelectedId].title;
                commandtitle = Context.Command.Commands[Context.SelectedId].title;
                commandfileputh = Context.Command.Commands[Context.SelectedId].fileputh;
                commandappputh = Context.Command.Commands[Context.SelectedId].appputh;
            }
            else
            {
                title = "Создание команды";
                SelectedAction = Actions[0];
                SelectedEmbeddedAction = EmbeddedActions[0];
                SelectedObject = Objects[0];
                SelectedTrigger = Triggers[0];
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
                    return "Добавить";
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

        public GridLength LengthEmbeddedActions
        {
            get => Context.Command.LengthEmbeddedActions;
            set => Set(ref Context.Command.LengthEmbeddedActions, value);
        }

        public GridLength LengthOwnActions
        {
            get => Context.Command.LengthOwnActions;
            set => Set(ref Context.Command.LengthOwnActions, value);
        }

        public GridLength LengthTextToSpeech
        {
            get => Context.Command.LengthTextToSpeech;
            set => Set(ref Context.Command.LengthTextToSpeech, value);
        }

        public GridLength LengthPressingKeys
        {
            get => Context.Command.LengthPressingKeys;
            set => Set(ref Context.Command.LengthPressingKeys, value);
        }

        public GridLength LengthMovingCursor
        {
            get => Context.Command.LengthMovingCursor;
            set => Set(ref Context.Command.LengthMovingCursor, value);
        }

        public GridLength LengthTextTrigger
        {
            get => Context.Command.LengthTextTrigger;
            set => Set(ref Context.Command.LengthTextTrigger, value);
        }

        public GridLength LengthAppTrigger
        {
            get => Context.Command.LengthAppTrigger;
            set => Set(ref Context.Command.LengthAppTrigger, value);
        }

        public string[] Actions
        {
            get => Program.Actions;
        }

        public string[] EmbeddedActions
        {
            get => Program.EmbeddedActions;
        }

        public string[] Objects
        {
            get => Program.Objects;
        }

        public string[] Triggers
        {
            get => Program.Triggers;
        }

        public string SelectedAction
        {
            get => Context.Command.SelectedAction;
            set
            {
                Set(ref Context.Command.SelectedAction, value);
                if (SelectedAction == "Сделать свое действие")
                {
                    LengthOwnActions = GridLength.Auto;
                    LengthEmbeddedActions = new GridLength(0, GridUnitType.Pixel);
                }
                if (SelectedAction == "Встроенные методы")
                {
                    LengthEmbeddedActions = GridLength.Auto;
                    LengthOwnActions = new GridLength(0, GridUnitType.Pixel);
                }
            }
        }

        public string SelectedTrigger
        {
            get => Context.Command.SelectedTrigger;
            set
            {
                Set(ref Context.Command.SelectedTrigger, value);
                if (SelectedTrigger == "Фраза")
                {
                    LengthTextTrigger = GridLength.Auto;
                    LengthAppTrigger = new GridLength(0, GridUnitType.Pixel);
                }
                if (SelectedTrigger == "Время")
                {
                    LengthTextTrigger = GridLength.Auto;
                    LengthAppTrigger = new GridLength(0, GridUnitType.Pixel);
                }
                if (SelectedTrigger == "Запуск Marlin")
                {
                    LengthTextTrigger = new GridLength(0, GridUnitType.Pixel);
                    LengthAppTrigger = new GridLength(0, GridUnitType.Pixel);
                }
                if (SelectedTrigger == "Запуск программы")
                {
                    LengthTextTrigger = new GridLength(0, GridUnitType.Pixel);
                    LengthAppTrigger = GridLength.Auto;
                }
            }
        }

        public string SelectedEmbeddedAction
        {
            get => Context.Command.SelectedEmbeddedAction;
            set
            {
                Set(ref Context.Command.SelectedEmbeddedAction, value);
                if (SelectedEmbeddedAction == "Озвучивание текста")
                {
                    LengthTextToSpeech = GridLength.Auto;
                    LengthPressingKeys = new GridLength(0, GridUnitType.Pixel);
                    LengthMovingCursor = new GridLength(0, GridUnitType.Pixel);
                }
                if (SelectedEmbeddedAction == "Нажатие клавиш")
                {
                    LengthTextToSpeech = new GridLength(0, GridUnitType.Pixel);
                    LengthPressingKeys = GridLength.Auto;
                    LengthMovingCursor = new GridLength(0, GridUnitType.Pixel);
                }
                if (SelectedEmbeddedAction == "Перемещение курсора")
                {
                    LengthTextToSpeech = new GridLength(0, GridUnitType.Pixel);
                    LengthPressingKeys = new GridLength(0, GridUnitType.Pixel);
                    LengthMovingCursor = GridLength.Auto;
                }
            }
        }

        public string SelectedObject
        {
            get => Context.Command.SelectedObject;
            set
            {
                Set(ref Context.Command.SelectedObject, value);
                if (SelectedObject == "Фаил") 
                {
                    LengthChoseObject = GridLength.Auto;
                    LengthChoseApp = GridLength.Auto;
                    LengthInputUrl = new GridLength(0, GridUnitType.Pixel);
                }
                if (SelectedObject == "Папка")
                {
                    LengthChoseObject = GridLength.Auto;
                    LengthChoseApp = new GridLength(0, GridUnitType.Pixel);
                    LengthInputUrl = new GridLength(0, GridUnitType.Pixel);
                }
                if (SelectedObject == "Url")
                {
                    LengthChoseObject = new GridLength(0, GridUnitType.Pixel);
                    LengthChoseApp = GridLength.Auto;
                    LengthInputUrl = GridLength.Auto;
                }
            }
        }

        public bool IsReadyCmdCommand
        {
            get => Context.Command.IsReadyCmdCommand;
            set
            {
                Set(ref Context.Command.IsReadyCmdCommand, value);
                if (IsReadyCmdCommand)
                {
                    LengthReadyCmdCommand = GridLength.Auto;
                    LengthCommandConstructor = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    LengthCommandConstructor = GridLength.Auto;
                    LengthReadyCmdCommand = new GridLength(0, GridUnitType.Pixel);
                }
            }
        }

        public bool IsMultiSymbol
        {
            get => Context.Command.IsMultiSymbol;
            set
            {
                Set(ref Context.Command.IsMultiSymbol, value);
                if (IsMultiSymbol)
                {
                    LengthMultiSymbol = GridLength.Auto;
                    LengthSymbolCode = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    LengthMultiSymbol = new GridLength(0, GridUnitType.Pixel);
                    LengthSymbolCode = GridLength.Auto;
                }
            }
        }

        public GridLength LengthMultiSymbol
        {
            get => Context.Command.LengthMultiSymbol;
            set => Set(ref Context.Command.LengthMultiSymbol, value);
        }

        public GridLength LengthSymbolCode
        {
            get => Context.Command.LengthSymbolCode;
            set => Set(ref Context.Command.LengthSymbolCode, value);
        }

        public GridLength LengthCommandConstructor
        {
            get => Context.Command.LengthCommandConstructor;
            set => Set(ref Context.Command.LengthCommandConstructor, value);
        }

        public GridLength LengthReadyCmdCommand
        {
            get => Context.Command.LengthReadyCmdCommand;
            set => Set(ref Context.Command.LengthReadyCmdCommand, value);
        }

        public GridLength LengthChoseObject
        {
            get => Context.Command.LengthChoseObject;
            set => Set(ref Context.Command.LengthChoseObject, value);
        }

        public GridLength LengthChoseApp
        {
            get => Context.Command.LengthChoseApp;
            set => Set(ref Context.Command.LengthChoseApp, value);
        }

        public GridLength LengthInputUrl
        {
            get => Context.Command.LengthInputUrl;
            set => Set(ref Context.Command.LengthInputUrl, value);
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
