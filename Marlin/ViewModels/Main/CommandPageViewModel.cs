using Marlin.Commands;
using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Newtonsoft.Json;
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
                LoadTrigger();
            }

            Context.CopyCommand = JsonConvert.DeserializeObject<Models.Main.Command>(JsonConvert.SerializeObject(Context.Command));
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
                var uniq = true;
                var uniqname = true;
                foreach (var command in ProgramData.Commands)
                {
                    if (command.Equals(Context.Command))
                    {
                        uniq = false;
                    }
                    if (command.Title== Context.Command.Title) // переделать на проверку и среди скриптов
                    {
                        uniqname = false;
                    }
                }
                if (uniq && uniqname) 
                {
                    Program.AddCommand(Context.Command);
                }
                else if(!uniq)
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
            if (Context.SelectedId > -1)
            {
                return false;
            }
            else
            { 
                return !Context.Command.Equals(Context.CopyCommand);
            }
        }

        private void OnAddTriggerCommandExecuted(object p)
        {
            Context.Command.AddTrigger("", SystemFiles.Types.TriggerType.Phrase);
            StackPanel.Children.Add(AddTrigger(Context.Command.Triggers[Context.Command.Triggers.Count - 1]));
        }

        private Grid AddTrigger(Marlin.Models.Main.Trigger trigger)
        {
            Grid mainGrid = new Grid();
            mainGrid.Margin = new Thickness(0, 10, 0, 10);
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            ComboBox comboBox = new ComboBox
            {
                FontSize = 15,
                Margin = new Thickness(0, 0, 10, 0),
                VerticalAlignment = VerticalAlignment.Top,
                SelectedIndex = (int)trigger.triggertype,
                MinWidth = 50
            };
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            comboBox.SetBinding(ComboBox.SelectedValueProperty, new System.Windows.Data.Binding("SelectedTrigger") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            comboBox.SetBinding(ComboBox.ItemsSourceProperty, new System.Windows.Data.Binding("Triggers") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            Grid.SetColumn(comboBox, 0);
            Grid innerGrid = new Grid();
            innerGrid.Margin = new Thickness(0, 0, 10, 0);
            innerGrid.RowDefinitions.Add(new RowDefinition { Height = LengthTextTrigger });
            innerGrid.RowDefinitions.Add(new RowDefinition { Height = LengthAppTrigger });
            Grid.SetColumn(innerGrid, 1);
            TextBox textBox = new TextBox
            {
                Margin = new Thickness(0, 8, 0, 0),
                MinWidth = 100,
                FontSize = 15,
                Padding = new Thickness(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Text = trigger.value
            };
            Grid.SetRow(textBox, 0);
            Grid innerInnerGrid = new Grid();
            innerInnerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            innerInnerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            Button button = new Button
            {
                FontSize = 15,
                Margin = new Thickness(0, 0, 10, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Content = "Выберите программу"
            };
            button.SetBinding(Button.ForegroundProperty, new System.Windows.Data.Binding("ExternalBackgroundColor") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            Grid.SetColumn(button, 0);
            TextBox programTextBox = new TextBox
            {
                MinWidth = 100,
                FontSize = 15,
                Padding = new Thickness(10, 0, 0, 0),
                IsReadOnly = true,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = trigger.value
            };
            Grid.SetColumn(programTextBox, 1);
            innerInnerGrid.Children.Add(button);
            innerInnerGrid.Children.Add(programTextBox);
            Grid.SetRow(innerInnerGrid, 1);
            innerGrid.Children.Add(textBox);
            innerGrid.Children.Add(innerInnerGrid);
            Grid.SetColumn(innerGrid, 1);
            Button deleteButton = new Button { Content = "Удалить" };
            Grid.SetColumn(deleteButton, 2);
            mainGrid.Children.Add(comboBox);
            mainGrid.Children.Add(innerGrid);
            mainGrid.Children.Add(deleteButton);
            return mainGrid;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                if (comboBox.SelectedIndex >= 0)
                {

                }
            }
        }

        private void LoadTrigger()
        {
            foreach (var trigger in Context.Command.Triggers)
            {
                StackPanel.Children.Add(AddTrigger(trigger));
            }
        }
    }
}
