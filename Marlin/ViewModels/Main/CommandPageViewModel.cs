using Marlin.Commands;
using Marlin.Models.Main;
using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Marlin.ViewModels.Main
{
    public class CommandPageViewModel : ViewModel
    {
        public ICommand ToMainCommand { get; }
        public ICommand ButtonActionCommand { get; }
        public ICommand AddTriggerCommand { get; }

        public ICommand SelectFileCommand { get; }
        public ICommand SelectAppCommand { get; }
        public ICommand DeleteAppCommand { get; }

        public ICommand AppTriggerCommand { get; }
        public ICommand RemoveTriggerCommand { get; }

        private StackPanel triggerpanel = new StackPanel();
        private string selectedtrigger;

        private string triggervalue;
        private string apptrigger;

        private string[] _objectActions = Program.ObjectActions;

        private GridLength texttriggerlength = new GridLength(0, GridUnitType.Pixel);
        private GridLength marlintriggerlength = GridLength.Auto;
        private GridLength apptriggerlength = new GridLength(0, GridUnitType.Pixel);

        private string _pagetitle = "Новая команда";

        public CommandPageViewModel()
        {
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
            ButtonActionCommand = new LambdaCommand(OnButtonActionCommandExecuted, CanButtonActionCommandExecute);
            AddTriggerCommand = new LambdaCommand(OnAddTriggerCommandExecuted, CanAddTriggerCommandExecute);
            SelectFileCommand = new LambdaCommand(OnSelectFileCommandExecuted);
            SelectAppCommand = new LambdaCommand(OnSelectAppCommandExecuted);
            DeleteAppCommand = new LambdaCommand(OnDeleteAppCommandExecuted);
            AppTriggerCommand = new LambdaCommand(OnAppTriggerCommandExecuted);
            RemoveTriggerCommand = new LambdaCommand(OnRemoveTriggerCommandExecuted);
            SelectedTrigger = Program.Triggers[0];



            Context.Command = new Command();
            Context.CopyCommand = JsonConvert.DeserializeObject<Command>(JsonConvert.SerializeObject(Context.Command));

            if (Context.SelectedId > -1)
            {
                Context.Command = JsonConvert.DeserializeObject<Command>(JsonConvert.SerializeObject(Command.GetCommand(Context.SelectedId)));
                PageTitle = Context.Command.Title;
                SelectedObject = Context.Command.SelectedObject;
                Context.CopyCommand = JsonConvert.DeserializeObject<Command>(JsonConvert.SerializeObject(Command.GetCommand(Context.SelectedId)));
                LoadTrigger();
            }

            else
            {
                if (Context.ProgramData.Commands.Count > 0)
                {
                    Title = "Команда" + (Context.ProgramData.Commands[Context.ProgramData.Commands.Count - 1].id + 1).ToString(); //lоделать чтоб он ориентировлся на id последнего а не на колличество
                }
                else
                {
                    Title = "Команда" + Context.Command.id.ToString();
                }
            }
        }

        public StackPanel TriggerPanel
        {
            get => triggerpanel;
            set => Set(ref triggerpanel, value);
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

        public string FileName
        {
            get => Context.Command.FileName;
            set
            {
                Set(ref Context.Command.FileName, value);
                if (FileName.Length == 0)
                {
                    LengthFileName = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    LengthFileName = GridLength.Auto;
                }
            }
        }

        public string AppName
        {
            get => Context.Command.AppName;
            set
            {
                Set(ref Context.Command.AppName, value);
                if (AppName.Length == 0)
                {
                    LengthAppName = new GridLength(0, GridUnitType.Pixel);
                }
                else
                {
                    LengthAppName = GridLength.Auto;
                }
            }
        }

        public string Url
        {
            get => Context.Command.Url;
            set
            {
                Set(ref Context.Command.Url, value);
                Context.Command.Filepath = Url;
            }
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

        public GridLength TextTriggerLength
        {
            get => texttriggerlength;
            set => Set(ref texttriggerlength, value);
        }

        public GridLength AppTriggerLength
        {
            get => apptriggerlength;
            set => Set(ref apptriggerlength, value);
        }

        public GridLength MarlinTrigger
        {
            get => marlintriggerlength;
            set => Set(ref marlintriggerlength, value);
        }

        public string TextTrigger
        {
            get => triggervalue;
            set => Set(ref triggervalue, value);
        }

        public string AppTrigger
        {
            get => apptrigger;
            set => Set(ref apptrigger, value);
        }

        public string[] Actions
        {
            get => Program.Actions;
        }

        public string[] ObjectActions
        {
            get => _objectActions;
            set => Set(ref _objectActions, value);
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

        public string SelectedTrigger
        {
            get => selectedtrigger;
            set
            {
                if (Set(ref selectedtrigger, value))
                {
                    TextTrigger = "";
                    AppTrigger = "";
                    if (SelectedTrigger == "Фраза" || SelectedTrigger == "Время")
                    {
                        TextTriggerLength = GridLength.Auto;
                        MarlinTrigger = GridLength.Auto;
                        AppTriggerLength = new GridLength(0, GridUnitType.Pixel);
                    }
                    if (SelectedTrigger == "Запуск Marlin")
                    {
                        MarlinTrigger = new GridLength(0, GridUnitType.Pixel);
                    }
                    if (SelectedTrigger == "Запуск программы")
                    {
                        TextTriggerLength = new GridLength(0, GridUnitType.Pixel);
                        AppTriggerLength = GridLength.Auto;
                        MarlinTrigger = GridLength.Auto;
                    }
                }
            }
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
                if (Set(ref Context.Command.SelectedObject, value))
                {
                    Context.Command.Filepath = "";
                    FileName = "";
                }

                if (SelectedObject == "Фаил")
                {
                    ObjectActions = Program.ObjectActions;
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
                    ObjectActions = Program.ObjectActionsSimple;
                    SelectedObjectAction = Program.ObjectActions[0];
                    LengthChoseObject = GridLength.Auto;
                    LengthInputUrl = new GridLength(0, GridUnitType.Pixel);
                    LengthObjectAction = GridLength.Auto;
                    LengthChoseApp = new GridLength(0, GridUnitType.Pixel);
                }

                if (SelectedObject == "Url")
                {
                    ObjectActions = Program.ObjectActions;
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

        public GridLength LengthAppName
        {
            get => Context.Command.LengthAppName;
            set => Set(ref Context.Command.LengthAppName, value);
        }

        public GridLength LengthFileName
        {
            get => Context.Command.LengthFileName;
            set => Set(ref Context.Command.LengthFileName, value);
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
            Program.SetPage(new ActionsPage());
        }

        private void OnButtonActionCommandExecuted(object p)
        {
            if (ValidationCommand())
            {
                if (Context.SelectedId > -1)
                {
                    Command.SetCommand(Context.SelectedId, Context.Command);
                }

                else
                {
                    Command.AddCommand(Context.Command);
                }

                ProgramData.SaveData();

                Program.SetPage(new ActionsPage());
            }
        }

        private bool ValidationCommand()
        {
            if (string.IsNullOrWhiteSpace(Context.Command.Title))
            {
                Models.MessageBox.MakeMessage("Название команды не должно быть пустым", SystemFiles.Types.MessageType.Error);
                return false;
            }

            bool isDuplicate = Context.ProgramData.Commands.Any(command => command.Equals(Context.Command));
            bool isDuplicateName = Context.ProgramData.Commands.Any(command => command.Title == Context.Command.Title);

            if (isDuplicate)
            {
                Models.MessageBox.MakeMessage("Такая команда уже существует", SystemFiles.Types.MessageType.Error);
                return false;
            }

            if (isDuplicateName && Context.Command.Title != Context.CopyCommand.Title)
            {
                Models.MessageBox.MakeMessage("Команда с таким именем уже существует", SystemFiles.Types.MessageType.Error);
                return false;
            }

            if (!HasAction())
            {
                Models.MessageBox.MakeMessage("Команда ничего не выполняет", SystemFiles.Types.MessageType.Error);
                return false;
            }

            return true;
        }

        private bool CanButtonActionCommandExecute(object p)
        {
            return !Context.Command.Equals(Context.CopyCommand);
        }

        private bool CanAddTriggerCommandExecute(object p)
        {
            if (SelectedTrigger == "Запуск Marlin")
            {
                return true;
            }
            return TextTrigger.Length > 0;
        }

        private void OnAddTriggerCommandExecuted(object p)
        {
            var trigger = new Models.Main.Trigger();
            string value = "";
            if (SelectedTrigger == "Фраза")
            {
                trigger.textvalue = TextTrigger;
                trigger.triggertype = TriggerType.Phrase;
                trigger.appvalue = "";
                value += "Фраза: " + TextTrigger;
            }
            if (SelectedTrigger == "Время")
            {
                trigger.textvalue = TextTrigger;
                trigger.triggertype = TriggerType.Time;
                trigger.appvalue = "";
                value += "Время: " + TextTrigger;
            }
            if (SelectedTrigger == "Запуск Marlin")
            {
                trigger.textvalue = "";
                trigger.triggertype = TriggerType.StartMarlin;
                trigger.appvalue = "";
                value += "Запуск Marlin";
            }
            if (SelectedTrigger == "Запуск программы")
            {
                trigger.textvalue = TextTrigger;
                trigger.triggertype = TriggerType.StartApp;
                trigger.appvalue = AppTrigger;
                value += "Программа: " + AppTrigger;
            }
            if (ValidationTrigger(trigger))
            {
                TriggerPanel.Children.Add(CreateCommand(value, Context.Command.Triggers.Count));
                Context.Command.Triggers.Add(trigger);

                TextTrigger = "";
                AppTrigger = "";
            }
        }

        private bool ValidationTrigger(Models.Main.Trigger trigger)
        {
            foreach (var trg in Context.Command.Triggers)
            {
                if (trg.Equals(trigger))
                {
                    Models.MessageBox.MakeMessage("У элемента уже присутствует такой триггер", MessageType.Error);
                    return false;
                }
            }
            if (trigger.triggertype == TriggerType.Time)
            {
                if (!trigger.textvalue.Contains(".") && !trigger.textvalue.Contains(":"))
                {
                    Models.MessageBox.MakeMessage("Некорректная запись времени", MessageType.Error);
                    return false;
                }
                if (!DateTime.TryParse(trigger.textvalue, out DateTime time))
                {
                    Models.MessageBox.MakeMessage("Некорректная запись времени", MessageType.Error);
                    return false;
                }
            }
            return true;
        }

        private void OnRemoveTriggerCommandExecuted(object p)
        {
            Context.Command.Triggers.RemoveAt((int)p);
            LoadTrigger();
        }

        private void OnSelectFileCommandExecuted(object p)
        {
            if (SelectedObject == "Фаил")
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Title = "Выбор файла";
                openFileDialog.Filter = "Все файлы (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    string path = openFileDialog.FileName;
                    Context.Command.Filepath = path;
                    FileName = System.IO.Path.GetFileName(path);
                    SelectedObjectAction = Program.ObjectActions[0];

                    if (Path.GetExtension(path) == ".exe")
                    {
                        Context.Command.AppName = "";
                        Context.Command.Apppath = "";
                        Context.Command.IsExe = true;
                        LengthChoseObject = GridLength.Auto;
                        LengthInputUrl = new GridLength(0, GridUnitType.Pixel);
                        LengthObjectAction = GridLength.Auto;
                        LengthChoseApp = new GridLength(0, GridUnitType.Pixel);

                        ObjectActions = Program.ObjectActions;
                    }
                    else
                    {
                        Context.Command.IsExe = false;
                        SelectedObject = "Фаил";
                        ObjectActions = Program.ObjectActionsSimple;
                    }
                }
            }
            if (SelectedObject == "Папка")
            {
                CommonOpenFileDialog folderPicker = new CommonOpenFileDialog();

                ObjectActions = Program.ObjectActionsSimple;

                folderPicker.IsFolderPicker = true;
                folderPicker.Title = "Выбор папки для хранения данных";

                CommonFileDialogResult dialogResult = folderPicker.ShowDialog();

                if (dialogResult == CommonFileDialogResult.Ok)
                {
                    string selectedFolderPath = folderPicker.FileName;

                    if (selectedFolderPath == Context.Settings.MainFolderPath)
                    {
                        Models.MessageBox.MakeMessage("Вы уверены что хотите выбрать папку которая является папкой для хранения данных?", MessageType.YesNoQuestion);
                        if (Context.MessageBox.Answer == "No")
                        {
                            return;
                        }
                    }

                    Context.Command.Filepath = selectedFolderPath;
                    FileName = System.IO.Path.GetFileName(selectedFolderPath);
                }
            }
        }

        private void OnSelectAppCommandExecuted(object p)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Выбор программы для запуска объекта";
            openFileDialog.Filter = "Программы (*.exe)|*.exe";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string path = openFileDialog.FileName;
                Context.Command.Apppath = path;
                AppName = System.IO.Path.GetFileName(path);
            }
        }

        public void OnDeleteAppCommandExecuted(object p)
        {
            AppName = "";
            Context.Command.Apppath = "";
        }

        public void OnAppTriggerCommandExecuted(object p)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Выбор программы для триггера";
            openFileDialog.Filter = "Программы (*.exe)|*.exe";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                TextTrigger = openFileDialog.FileName;
                AppTrigger = System.IO.Path.GetFileName(TextTrigger);
            }
        }

        private bool HasAction()
        {
            if (SelectedAction == "Сделать свое действие")
            {
                if (Context.Command.IsReadyCmdCommand)
                {
                    return Context.Command.CmdCommand.Length > 0;
                }
                else
                {
                    if (SelectedObject == "Фаил")
                    {
                        return Context.Command.Filepath.Length > 0;
                    }
                    if (SelectedObject == "Папка")
                    {
                        return Context.Command.Filepath.Length > 0;
                    }
                    if (SelectedObject == "Url")
                    {
                        return Context.Command.Url.Length > 0;
                    }
                }
            }
            if (SelectedAction == "Встроенные методы")
            {
                return false;
            }
            return false;
        }

        private void LoadTrigger()
        {
            TriggerPanel.Children.Clear();
            for (int i = 0; i < Context.Command.Triggers.Count; i++)
            {
                string value = "";
                if (Context.Command.Triggers[i].triggertype == TriggerType.Phrase)
                {
                    value += "Фраза: " + Context.Command.Triggers[i].textvalue;
                }
                if (Context.Command.Triggers[i].triggertype == TriggerType.Time)
                {
                    value += "Время: " + Context.Command.Triggers[i].textvalue;
                }
                if (Context.Command.Triggers[i].triggertype == TriggerType.StartMarlin)
                {
                    value += "Запуск Marlin";
                }
                if (Context.Command.Triggers[i].triggertype == TriggerType.StartApp)
                {
                    value += "Программа: " + Context.Command.Triggers[i].appvalue;
                }
                TriggerPanel.Children.Add(CreateCommand(value, i));
            }
        }

        private Border CreateCommand(string command, int number)
        {
            var border = new Border
            {
                Margin = new Thickness(0, 10, 0, 0),
                CornerRadius = new CornerRadius(25),
                HorizontalAlignment = HorizontalAlignment.Left,
                BorderThickness = new Thickness(2),
            };

            BindingOperations.SetBinding(border, Border.BackgroundProperty, new Binding("ExternalBackgroundColor")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            BindingOperations.SetBinding(border, Border.BorderBrushProperty, new Binding("PageColor")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            var grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var textBlock = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(15, 5, 0, 5),
                Text = command
            };

            Grid.SetRow(textBlock, 0);
            Grid.SetColumn(textBlock, 0);

            var button = new Button
            {
                Content = "✖",
                Padding = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold,
                Command = RemoveTriggerCommand,
                CommandParameter = number,
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                BorderBrush = Brushes.Transparent,
                Margin = new Thickness(10, 5, 10, 5)
            };

            Grid.SetRow(button, 0);
            Grid.SetColumn(button, 1);

            BindingOperations.SetBinding(button, Button.ForegroundProperty, new Binding("PageColor")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            grid.Children.Add(textBlock);
            grid.Children.Add(button);

            border.Child = grid;

            return border;
        }
    }
}
