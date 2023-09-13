using Marlin.Commands;
using Marlin.Models.Main;
using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Marlin.ViewModels.Main
{
    public class CommandPageViewModel : ViewModel
    {
        public ICommand ToMainCommand { get; }
        public ICommand ButtonActionCommand { get; }
        public ICommand AddTriggerCommand { get; }

        private StackPanel panel = new StackPanel();

        private string _pagetitle = "Создание команды";

        public CommandPageViewModel()
        {
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
            ButtonActionCommand = new LambdaCommand(OnButtonActionCommandExecuted, CanButtonActionCommandExecute);
            AddTriggerCommand = new LambdaCommand(OnAddTriggerCommandExecuted);

            Context.Command = new Models.Main.Command();

            if (Context.SelectedId > -1)
            {
                Context.Command = Command.GetCommand(Context.SelectedId);
                PageTitle = Context.Command.Title;
            }
            Context.CopyCommand = JsonConvert.DeserializeObject<Models.Main.Command>(JsonConvert.SerializeObject(Context.Command));
            if (Context.SelectedId == -1)
            {
                if (ProgramData.Commands.Count > 0)
                {
                    Title = "Команда " + (ProgramData.Commands[ProgramData.Commands.Count - 1].id + 1).ToString();
                }
                else
                {
                    Title = "Команда " + Context.Command.id.ToString();
                }
            }
        }

        public StackPanel StackPanel
        {
            get => panel;
            set => Set(ref panel, value);
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

        public string PageTitle
        {
            get => _pagetitle;
            set => Set(ref _pagetitle, value);
        }

        public string Title
        {
            get => Context.Command.Title;
            set => Set(ref Context.Command.Title, value);
        }

        public string FilePath
        {
            get => Context.Command.Filepath;
            set => Set(ref Context.Command.Filepath, value);
        }

        public string AppPuth
        {
            get => Context.Command.Appputh;
            set => Set(ref Context.Command.Appputh, value);
        }

        public string Url
        {
            get => Context.Command.Url;
            set => Set(ref Context.Command.Url, value);
        }

        public string PressingKeys
        {
            get => Context.Command.PressingKeys;
            set => Set(ref Context.Command.PressingKeys, value);
        }

        public string X
        {
            get => Context.Command.X;
            set => Set(ref Context.Command.X, value);
        }

        public string Y
        {
            get => Context.Command.Y;
            set => Set(ref Context.Command.Y, value);
        }

        public bool Checkpuss
        {
            get => Context.Command.Checkpuss;
            set
            {
                if (!value)
                {
                    if (!Program.Authentication("Для снятия защиты подтвердте пароль", check: true))
                    {
                        return;
                    }
                }
                Set(ref Context.Command.Checkpuss, value);
            }
        }

        public string SelectedAction
        {
            get => Context.Command.SelectedAction;
            set
            {
                Set(ref Context.Command.SelectedAction, value);
                if (Context.Command.SelectedAction == "Сделать свое действие")
                {
                    LengthOwnActions = GridLength.Auto;
                    LengthEmbeddedActions = new GridLength(0, GridUnitType.Pixel);
                }
                if (Context.Command.SelectedAction == "Встроенные методы")
                {
                    LengthEmbeddedActions = GridLength.Auto;
                    LengthOwnActions = new GridLength(0, GridUnitType.Pixel);
                }
            }
        }

        public bool IsReadyCmdCommand
        {
            get => Context.Command.IsReadyCmdCommand;
            set
            {
                Set(ref Context.Command.IsReadyCmdCommand, value);
                if (Context.Command.IsReadyCmdCommand)
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

        public string Comment
        {
            get => Context.Command.Comment;
            set => Set(ref Context.Command.Comment, value);
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

        public string[] Actions
        {
            get => Program.Actions;
        }

        public string[] ObjectActions
        {
            get => Program.ObjectActions;
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

        public string SelectedObjectAction
        {
            get => Context.Command.SelectedObjectAction;
            set
            {
                Set(ref Context.Command.SelectedObjectAction, value);
                if (SelectedObjectAction == "Открыть" && SelectedObject != "Папка" && SelectedObject != "Программа")
                {
                    LengthChoseApp = GridLength.Auto;
                }
                else
                {
                    LengthChoseApp = new GridLength(0, GridUnitType.Pixel);
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
                    LengthInputUrl = new GridLength(0, GridUnitType.Pixel);
                    LengthObjectAction = GridLength.Auto;
                    if (SelectedObjectAction == "Открыть")
                    {
                        LengthChoseApp = GridLength.Auto;
                    }
                    else
                    {
                        LengthChoseApp = new GridLength(0, GridUnitType.Pixel);
                    }
                }

                if (SelectedObject == "Папка")
                {
                    SelectedObjectAction = Program.ObjectActions[0];
                    LengthChoseObject = GridLength.Auto;
                    LengthInputUrl = new GridLength(0, GridUnitType.Pixel);
                    LengthObjectAction = new GridLength(0, GridUnitType.Pixel);
                    LengthChoseApp = new GridLength(0, GridUnitType.Pixel); 
                }

                if (SelectedObject == "Url")
                {
                    SelectedObjectAction = Program.ObjectActions[0];
                    LengthChoseObject = new GridLength(0, GridUnitType.Pixel);
                    LengthInputUrl = GridLength.Auto;
                    LengthObjectAction = new GridLength(0, GridUnitType.Pixel);
                    if (SelectedObjectAction == Program.ObjectActions[0])
                    {
                        LengthChoseApp = GridLength.Auto;
                    }
                    else
                    {
                        LengthChoseApp = new GridLength(0, GridUnitType.Pixel);
                    }
                }

                if (SelectedObject == "Программа")
                {
                    LengthChoseObject = GridLength.Auto;
                    LengthInputUrl = new GridLength(0, GridUnitType.Pixel);
                    LengthObjectAction = GridLength.Auto;
                    LengthChoseApp = new GridLength(0, GridUnitType.Pixel);
                }
            }
        }

        public string CmdCommand
        {
            get => Context.Command.CmdCommand;
            set => Set(ref Context.Command.CmdCommand, value);
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

        public GridLength LengthObjectAction
        {
            get => Context.Command.LengthObjectAction;
            set => Set(ref Context.Command.LengthObjectAction, value);
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
            if (Context.SelectedId > -1) 
            {
                Context.Command = JsonConvert.DeserializeObject<Marlin.Models.Main.Command>(JsonConvert.SerializeObject(Context.CopyCommand));
            }
            Program.SetPage(new ActionsPage());
        }

        private void OnButtonActionCommandExecuted(object p)
        {
            if (Context.Command.Title.Length == 0)
            {
                Models.MessageBox.MakeMessage("Название команды не должно быть пустым");
                return;
            }
            if (Context.SelectedId > -1)
            {
                var command = Command.GetCommand(Context.SelectedId);
                command = Context.Command;
            }
            else
            {
                var uniq = true;
                var uniqname = true;
                foreach (var command in ProgramData.Commands)
                {
                    if (command.Equals(Context.Command))
                    {
                        uniq = false;
                    }
                    if (command.Title == Context.Command.Title) // переделать на проверку и среди скриптов
                    {
                        uniqname = false;
                    }
                }
                if (uniq && uniqname)
                {
                    Program.AddCommand(Context.Command);
                }
                else if (!uniq)
                {
                    Models.MessageBox.MakeMessage("Такая команда уже существует", SystemFiles.Types.MessageType.Error);
                    return;
                }
                else if (!uniqname)
                {
                    Models.MessageBox.MakeMessage("Команда с таким именем уже существует", SystemFiles.Types.MessageType.Error);
                    return;
                }
            }
            Program.SetPage(new ActionsPage());
        }

        private bool CanButtonActionCommandExecute(object p)
        {
            return !Context.Command.Equals(Context.CopyCommand);
        }

        private void OnAddTriggerCommandExecuted(object p)
        {
            //Context.Command.AddTrigger("", SystemFiles.Types.TriggerType.Phrase);
            //StackPanel.Children.Add(AddTrigger(Context.Command.Triggers[Context.Command.Triggers.Count - 1]));
            //Реализовать переход на страницу на которой будет действие 
            Models.MessageBox.MakeMessage("Странице не доступна", SystemFiles.Types.MessageType.Error);
        }
    }
}
