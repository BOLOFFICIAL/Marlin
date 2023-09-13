using Marlin.Commands;
using Marlin.Models.Main;
using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Marlin.ViewModels.Main
{
    public class ActionsPageViewModel : ViewModel
    {
        private string _title;
        private StackPanel panel = new StackPanel();
        private GridLength _lengthAbout = new GridLength(0, GridUnitType.Pixel);
        public ICommand ToMainCommand { get; }
        public ICommand EditActionCommand { get; }
        public ICommand RunActionCommand { get; }
        public ICommand AddActionCommand { get; }
        public ICommand DeleteActionCommand { get; }

        public ActionsPageViewModel()
        {
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
            EditActionCommand = new LambdaCommand(OnEditActionCommandExecuted);
            RunActionCommand = new LambdaCommand(OnRunActionCommandExecuted);
            AddActionCommand = new LambdaCommand(OnAddActionCommandExecuted);
            DeleteActionCommand = new LambdaCommand(OnDeleteActionCommandExecuted);
            LoadActions();
        }

        public string Title
        {
            get
            {
                switch (Context.Action)
                {
                    case ActionType.Settings:
                        return "Настройки";
                    case ActionType.Command:
                        return "Команды";
                    case ActionType.Script:
                        return "Скрипты";
                    default:
                        return "ErrorAction";
                }
            }
        }

        public string TitleAbout
        {
            get => _title;
            set => Set(ref _title, value);
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

        public StackPanel StackPanel
        {
            get
            {
                return panel;
            }
        }

        void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                if (border.Child is Grid grid)
                {
                    UIElement foundElement = grid.Children.OfType<UIElement>().FirstOrDefault(e => e.GetType() == typeof(TextBlock));
                    if (foundElement is not null && foundElement is TextBlock textBox)
                    {
                        var command = Command.GetCommand(textBox.Text);
                        if (command.Checkpuss)
                        {
                            if (!Program.Authentication("Для открытия содержимого введите пароль"))
                            {
                                return;
                            }
                        }
                        OpenAction(command);
                        LengthAbout = new GridLength(2, GridUnitType.Star);
                    }
                }
            }
        }

        public GridLength LengthAbout
        {
            get => _lengthAbout;
            set => Set(ref _lengthAbout, value);
        }

        private void OnToMainCommandExecuted(object p)
        {
            Program.SetPage(new MainPage());
        }

        private void OnEditActionCommandExecuted(object p)
        {
            Context.SelectedId = (int)p;
            if (Context.Action == ActionType.Command)
            {
                if (Command.GetCommand(Context.SelectedId).Checkpuss)
                {
                    if (!Program.Authentication("Для открытия содержимого введите пароль"))
                    {
                        return;
                    }
                }
                Program.SetPage(new CommandPage());
            }
            if (Context.Action == ActionType.Script)
            {
                Program.SetPage(new ScriptPage());
            }
        }

        private void OnDeleteActionCommandExecuted(object p)
        {
            Context.SelectedId = (int)p;
            if (Context.Action == ActionType.Command)
            {
                var command = Command.GetCommand(Context.SelectedId);
                var check = command.Checkpuss;
                if (check)
                {
                    if (!Program.Authentication("Для удаления команды введите пароль", check: check))
                    {
                        return;
                    }
                }
                else 
                {
                    Models.MessageBox.MakeMessage($"Вы действительно хотите удалить команду {command.Title}",MessageType.YesNoQuestion);
                    if (Context.MessageBox.Answer == "No") 
                    {
                        return;
                    }
                }
                ProgramData.Commands.Remove(Command.GetCommand(Context.SelectedId));
                LoadActions();
            }
            if (Context.Action == ActionType.Script)
            {
                Program.SetPage(new ScriptPage());
            }
        }

        private void OnRunActionCommandExecuted(object p)
        {
            Context.SelectedId = (int)p;
            if (Context.Action == ActionType.Command)
            {
                var command = ProgramData.Commands[Context.SelectedId];
                if (command.Checkpuss)
                {
                    if (!Program.Authentication("Для запуска комманды необходимо подтвердить пароль"))
                    {
                        return;
                    }
                }
                WinSystem.RunCmd(command.ResultCommand);
            }

            if (Context.Action == ActionType.Script)
            {

            }
        }

        private void OpenAction(Command command)
        {
            TitleAbout = command.Title;
        }

        private void OnAddActionCommandExecuted(object p)
        {
            Context.SelectedId = -1;
            if (Context.Action == ActionType.Command)
            {
                Program.SetPage(new CommandPage());
            }
            if (Context.Action == ActionType.Script)
            {
                Program.SetPage(new ScriptPage());
            }
        }

        private void LoadActions()
        {
            StackPanel.Children.Clear();
            if (Context.Action == ActionType.Command)
            {
                foreach (var command in ProgramData.Commands)
                {
                    Border border = new Border
                    {
                        Margin = new Thickness(6, 3, 6, 3),
                        Padding = new Thickness(0),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor)),
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.ExternalBackgroundColor)),
                        BorderThickness = new Thickness(2),
                        CornerRadius = new CornerRadius(20)
                    };

                    border.MouseLeftButtonDown += Border_MouseLeftButtonDown;

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
                        Text = command.Title
                    };
                    Button buttonDelete = new Button
                    {
                        Margin = new Thickness(0, 5, 5, 5),
                        Padding = new Thickness(5, 0, 5, 4),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.FontColor)),
                        BorderBrush = new SolidColorBrush(Colors.Transparent),
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor)),
                        Height = 30,
                        Command = DeleteActionCommand,
                        CommandParameter = command.id,
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
                        Command = RunActionCommand,
                        CommandParameter = command.id,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Content = "Запустить"
                    };
                    Button buttonEdit = new Button
                    {
                        Margin = new Thickness(0, 5, 20, 5),
                        Padding = new Thickness(0, 0, 0, 4),
                        Width = 33,
                        Command = EditActionCommand,
                        CommandParameter = command.id,
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
                    border.Child = grid;
                    panel.Children.Add(border);
                }
            }
            if (Context.Action == ActionType.Script)
            {
                panel = Script.panel;
            }
        }
    }
}
