using Marlin.Commands;
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
                Context.Command = ProgramData.Commands[Context.SelectedId];
                PageTitle = Context.Command.Title;
            }

            Context.CopyCommand = JsonConvert.DeserializeObject<Models.Main.Command>(JsonConvert.SerializeObject(Context.Command));
        }

        public StackPanel StackPanel
        {
            get
            {
                return panel;
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
            set => Set(ref Context.Command.Checkpuss, value);
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
                if (SelectedObjectAction == "Запустить" && SelectedObject != "Папка" && SelectedObject != "Программа")
                {
                    LengthChoseApp = GridLength.Auto;
                }
                else
                {
                    LengthChoseApp = new GridLength(0, GridUnitType.Pixel);
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
                    LengthInputUrl = new GridLength(0, GridUnitType.Pixel);
                    LengthObjectAction = GridLength.Auto;
                    if (SelectedObjectAction == "Запустить")
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
                    LengthChoseObject = GridLength.Auto;
                    LengthInputUrl = new GridLength(0, GridUnitType.Pixel);
                    LengthObjectAction = GridLength.Auto;
                    LengthChoseApp = new GridLength(0, GridUnitType.Pixel);
                }

                if (SelectedObject == "Url")
                {
                    SelectedObjectAction = "Запустить";
                    LengthChoseObject = new GridLength(0, GridUnitType.Pixel);
                    LengthInputUrl = GridLength.Auto;
                    LengthObjectAction = new GridLength(0, GridUnitType.Pixel);
                    if (SelectedObjectAction == "Запустить")
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
            Program.SetPage(new ActionsPage());
        }

        private void OnButtonActionCommandExecuted(object p)
        {
            if (Context.SelectedId > -1)
            {
                ProgramData.Commands[Context.SelectedId] = Context.Command;
            }
            else
            {
                Models.Main.Command.AddCommand(Context.Command);
            }
            Program.SetPage(new ActionsPage());
        }

        private bool CanButtonActionCommandExecute(object p)
        {
            return !Context.Command.Equals(Context.CopyCommand);
        }

        private void OnAddTriggerCommandExecuted(object p)
        {
            Models.Main.Command.AddTrigger("123", SystemFiles.Types.TriggerType.Phrase);
        }

        private void LoadTrigger()
        {
            foreach (var trigger in ProgramData.Commands[Context.SelectedId].Triggers)
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                TextBlock textBlock = new TextBlock
                {
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.FontColor)),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(10, 10, 10, 10),
                    //Text = command.title
                };
                Button buttonDelete = new Button
                {
                    Margin = new Thickness(0, 5, 5, 5),
                    Padding = new Thickness(5, 0, 5, 4),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.FontColor)),
                    BorderBrush = new SolidColorBrush(Colors.Transparent),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor)),
                    Height = 30,
                    //Command = RunActionCommand,
                    //CommandParameter = command.id,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Content = "Удалить"
                };
                Button buttonRun = new Button
                {
                    Margin = new Thickness(0, 5, 5, 5),
                    Padding = new Thickness(5, 0, 5, 4),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.FontColor)), // Установите нужный цвет текста кнопки
                    BorderBrush = new SolidColorBrush(Colors.Transparent), // Установите нужный цвет границы кнопки
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor)), // Установите нужный цвет фона кнопки
                    Height = 30,
                    //Command = RunActionCommand,
                    //CommandParameter = command.id,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Content = "Запустить"
                };
                Button buttonEdit = new Button
                {
                    Margin = new Thickness(0, 5, 20, 5),
                    Padding = new Thickness(0, 0, 0, 4),
                    Width = 33,
                    //Command = EditActionCommand,
                    //CommandParameter = command.id,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.FontColor)), // Установите нужный цвет текста кнопки
                    BorderBrush = new SolidColorBrush(Colors.Transparent), // Установите нужный цвет границы кнопки
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor)), // Установите нужный цвет фона кнопки
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Content = "✏️"
                };
                Grid.SetColumn(textBlock, 0);
                Grid.SetColumn(buttonDelete, 1);
                Grid.SetColumn(buttonRun, 2);
                Grid.SetColumn(buttonEdit, 3);
                grid.Children.Add(textBlock);
                grid.Children.Add(buttonDelete);
                grid.Children.Add(buttonRun);
                grid.Children.Add(buttonEdit);
                panel.Children.Add(grid);
            }
        }
    }
}
