using Marlin.Commands;
using Marlin.Commands.Base;
using Marlin.Models.Main;
using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static System.Windows.Forms.Design.AxImporter;

namespace Marlin.ViewModels.Main
{
    public class ActionsPageViewModel : ViewModel
    {
        private string _title;
        private StackPanel panel = new StackPanel();
        private StackPanel triggers = new StackPanel();
        private StackPanel scriptcommand = new StackPanel();
        private GridLength _lengthAbout = new GridLength(0, GridUnitType.Pixel);
        private GridLength _lengthScriptCommand = new GridLength(0, GridUnitType.Pixel);
        private GridLength _lengthDescription = new GridLength(0, GridUnitType.Pixel);
        private GridLength _lengthTrigger = new GridLength(0, GridUnitType.Pixel);
        private string _description = "";
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

                    case ActionType.Reminder:
                        return "Напоминания";

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
            get => panel;
            set => Set(ref panel, value);
        }

        public StackPanel ScriptCommandPanel
        {
            get => scriptcommand;
            set => Set(ref scriptcommand, value);
        }

        public StackPanel TriggerPanel
        {
            get => triggers;
            set => Set(ref triggers, value);
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
                        LengthAbout = new GridLength(0, GridUnitType.Star);
                        LengthScriptCommand = new GridLength(0, GridUnitType.Star);
                        LengthDescription = new GridLength(0, GridUnitType.Star);
                        LengthTrigger = new GridLength(0, GridUnitType.Star);
                        TitleAbout = textBox.Text;
                        if (Context.Action == ActionType.Command)
                        {
                            var command = Models.Main.Command.GetCommand(textBox.Text);
                            if (command.Comment.Length > 0 || command.Triggers.Count > 0)
                            {
                                Description = command.Comment;
                                LoadTrigger(command.Triggers);
                                LengthAbout = new GridLength(1, GridUnitType.Star);
                                if (command.Comment.Length > 0)
                                {
                                    LengthDescription = new GridLength(1, GridUnitType.Star);
                                }
                                if (command.Triggers.Count > 0)
                                {
                                    LengthTrigger = new GridLength(1, GridUnitType.Star);
                                }
                            }
                        }
                        if (Context.Action == ActionType.Script)
                        {
                            var script = Script.GetScript(textBox.Text);
                            if (script.Comment.Length > 0 || script.Triggers.Count > 0 || script.Actions.Count > 0)
                            {
                                Description = script.Comment;
                                LoadTrigger(script.Triggers);
                                LoadScriptCommand(script);
                                LengthAbout = new GridLength(1, GridUnitType.Star);
                                if (script.Comment.Length > 0)
                                {
                                    LengthDescription = new GridLength(1, GridUnitType.Star);
                                }
                                if (script.Triggers.Count > 0)
                                {
                                    LengthTrigger = new GridLength(1, GridUnitType.Star);
                                }
                                if (script.Actions.Count > 0)
                                {
                                    LengthScriptCommand = new GridLength(1, GridUnitType.Star);
                                }
                            }
                        }
                    }
                }
            }
        }

        public GridLength LengthAbout
        {
            get => _lengthAbout;
            set
            {
                Set(ref _lengthAbout, value);
            }
        }

        public GridLength LengthScriptCommand
        {
            get => _lengthScriptCommand;
            set => Set(ref _lengthScriptCommand, value);
        }

        public GridLength LengthDescription
        {
            get => _lengthDescription;
            set => Set(ref _lengthDescription, value);
        }

        public GridLength LengthTrigger
        {
            get => _lengthTrigger;
            set => Set(ref _lengthTrigger, value);
        }

        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        private void OnToMainCommandExecuted(object p)
        {
            Program.SetPage(new MainPage());
        }

        private void OnEditActionCommandExecuted(object p)
        {
            if (Program.Authentication("Для изменения элемента подтвердите пароль"))
            {
                Context.SelectedId = (int)p;
                if (Context.Action == ActionType.Command)
                {
                    Program.SetPage(new CommandPage());
                }
                if (Context.Action == ActionType.Script)
                {
                    Program.SetPage(new ScriptPage());
                }
            }
        }

        private void OnDeleteActionCommandExecuted(object p)
        {
            Context.SelectedId = (int)p;
            LengthAbout = new GridLength(0, GridUnitType.Star);
            LengthDescription = new GridLength(0, GridUnitType.Star);
            LengthTrigger = new GridLength(0, GridUnitType.Star);
            string delete = "";
            if (Context.Action == ActionType.Command)
            {
                var command = Models.Main.Command.GetCommand(Context.SelectedId);
                delete = "команду " + command.Title;
            }
            if (Context.Action == ActionType.Script)
            {
                var script = Script.GetScript(Context.SelectedId);
                delete = "скрипт " + script.Title;
            }
            if (DateTime.Now - Context.LastCheckPassword > TimeSpan.FromSeconds(Context.Settings.TimeCheckPassword))
            {
                if (Program.Authentication($"Чтоб удалить {delete} подтвердите пароль"))
                {
                    if (Context.Action == ActionType.Command)
                    {
                        Models.Main.Command.RemoveCommand(Context.SelectedId);
                    }
                    if (Context.Action == ActionType.Script)
                    {
                        Script.RemoveScript(Context.SelectedId);
                    }
                    LoadActions();
                }
            }
            else
            {
                Models.MessageBox.MakeMessage($"Удалить {delete}?", MessageType.YesNoQuestion);
                if (Context.MessageBox.Answer == "Yes")
                {
                    if (Context.Action == ActionType.Command)
                    {
                        Models.Main.Command.RemoveCommand(Context.SelectedId);
                    }
                    if (Context.Action == ActionType.Script)
                    {
                        Script.RemoveScript(Context.SelectedId);
                    }
                    LoadActions();
                }
            }
        }

        private void OnRunActionCommandExecuted(object p)
        {
            LengthAbout = new GridLength(0, GridUnitType.Pixel);

            Context.SelectedId = (int)p;

            if (Context.Action == ActionType.Command)
            {
                var command = Models.Main.Command.GetCommand(Context.SelectedId);
                if (command.OneTime) 
                {
                    command = JsonConvert.DeserializeObject<Models.Main.Command>(JsonConvert.SerializeObject(Models.Main.Command.GetCommand(Context.SelectedId)));
                    Models.Main.Command.RemoveCommand(Context.SelectedId);
                }
                if (command.Checkpuss)
                {
                    if (!Program.Authentication("Для запуска комманды подтвердите пароль"))
                    {
                        return;
                    }
                }
                Sound.PlaySoundAsync(MessageType.Info);
                command.ExecuteCommandAsync();
            }

            if (Context.Action == ActionType.Script)
            {
                var script = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Script.GetScript(Context.SelectedId)));

                if (script.Checkpuss)
                {
                    if (!Program.Authentication("Для запуска скрипта необходимо подтвердить пароль"))
                    {
                        return;
                    }
                }
                else
                {
                    var checkpuss = false;

                    checkpuss = script.Commands.Any(command => Models.Main.Command.GetCommand(command).Checkpuss == true);

                    if (checkpuss)
                    {
                        if (!SystemFiles.Program.Authentication("Одна или несколько команд защищены паролем.\nДля запуска скрипта подтвердите пароль"))
                        {
                            return;
                        }
                    }
                }

                if (script.OneTime)
                {
                    Script.RemoveScript(Context.SelectedId);
                }
                else
                {
                    var updatescript = Script.GetScript(Context.SelectedId);

                    var rescommand = new List<int>();
                    var resscript = new List<int>();
                    var resaction = new List<int>();

                    var comandindex = 0;
                    var scriptindex = 0;

                    foreach (var action in updatescript.Actions)
                    {
                        if (action == 0)
                        {
                            var cmmnd = Models.Main.Command.GetCommand(updatescript.Commands[comandindex]);
                            if (cmmnd != null && !cmmnd.OneTime)
                            {
                                rescommand.Add(updatescript.Commands[comandindex]);
                                resaction.Add(0);
                            }
                            comandindex++;
                        }
                        if (action == 1)
                        {
                            var scrpt = Script.GetScript(updatescript.Scripts[scriptindex]);
                            if (scrpt != null && !scrpt.OneTime)
                            {
                                resscript.Add(updatescript.Scripts[scriptindex]);
                                resaction.Add(1);
                            }
                            scriptindex++;
                        }
                    }

                    updatescript.Commands = rescommand;
                    updatescript.Scripts = resscript;
                    updatescript.Actions = resaction;

                    if (resaction.Count == 0)
                    {
                        Script.RemoveScript(Context.SelectedId);
                    }
                }

                Sound.PlaySoundAsync(MessageType.Info);
                script.ExecuteScriptAsync();
            }

            LoadActions();
        }

        private void OnAddActionCommandExecuted(object p)
        {
            if (Program.Authentication("Для добавления элемента подтвердите пароль"))
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
                if (Context.Action == ActionType.Reminder)
                {

                }
            }
        }

        private void LoadActions()
        {
            StackPanel.Children.Clear();
            Models.Main.Command.CheckCommands();
            Context.ProgramData.Scripts = Script.ClearFromZero();
            ProgramData.SaveData();

            if (Context.Action == ActionType.Command)
            {
                foreach (var command in Context.ProgramData.Commands)
                {
                    var border = CreateBorder();
                    var textBlock = CreateTextBlock(command.Title, command.Checkpuss);
                    var buttonDelete = CreateButton(DeleteActionCommand, command.id, "⨉", 20);
                    var buttonRun = CreateButton(RunActionCommand, command.id, "▷", 20);
                    var buttonEdit = CreateButton(EditActionCommand, command.id, "✎", 15);
                    var grid = CreateGrid(textBlock, buttonDelete, buttonRun, buttonEdit);
                    border.Child = grid;
                    panel.Children.Add(border);
                }
            }

            if (Context.Action == ActionType.Script)
            {
                foreach (var script in Context.ProgramData.Scripts)
                {
                    var border = CreateBorder();
                    var textBlock = CreateTextBlock(script.Title, script.Checkpuss);
                    var buttonDelete = CreateButton(DeleteActionCommand, script.id, "⨉", 20);
                    var buttonRun = CreateButton(RunActionCommand, script.id, "▷", 20);
                    var buttonEdit = CreateButton(EditActionCommand, script.id, "✎", 15);
                    var grid = CreateGrid(textBlock, buttonDelete, buttonRun, buttonEdit);
                    border.Child = grid;
                    panel.Children.Add(border);
                }
            }

            Border CreateBorder()
            {
                Border border = new Border
                {
                    Margin = new Thickness(6, 3, 6, 3),
                    Padding = new Thickness(0, 0, 10, 0),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor)),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.ExternalBackgroundColor)),
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(20)
                };

                border.MouseLeftButtonDown += Border_MouseLeftButtonDown;

                return border;
            }

            Grid CreateGrid(TextBlock textBlock, Button buttonDelete, Button buttonRun, Button buttonEdit)
            {
                Grid.SetColumn(textBlock, 0);
                Grid.SetColumn(buttonDelete, 1);
                Grid.SetColumn(buttonRun, 2);
                Grid.SetColumn(buttonEdit, 3);

                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.Children.Add(textBlock);
                grid.Children.Add(buttonDelete);
                grid.Children.Add(buttonRun);
                grid.Children.Add(buttonEdit);

                return grid;
            }

            TextBlock CreateTextBlock(string title, bool checkpass)
            {
                var foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.FontColor));
                if (checkpass)
                {
                    foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor));
                }
                TextBlock textBlock = new TextBlock
                {
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Foreground = foreground,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(10, 10, 10, 10),
                    Text = title
                };

                return textBlock;
            }

            Button CreateButton(ICommand command, int id, string content, int size)
            {
                Button button = new Button
                {
                    Margin = new Thickness(0, 5, 5, 5),
                    Padding = new Thickness(5, 0, 5, 4),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.ExternalBackgroundColor)),
                    BorderBrush = new SolidColorBrush(Colors.Transparent),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor)),
                    Height = 30,
                    Width = 30,
                    FontSize = size,
                    Command = command,
                    CommandParameter = id,
                    FontWeight = FontWeights.Black,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Content = content
                };

                return button;
            }
        }

        private void LoadTrigger(List<Models.Main.Trigger> triggers)
        {
            TriggerPanel.Children.Clear();
            foreach (var trigger in triggers)
            {
                string value = "";
                if (trigger.triggertype == TriggersType.Phrase)
                {
                    value += "Фраза: " + trigger.textvalue;
                }
                if (trigger.triggertype == TriggersType.Time)
                {
                    value += "Время: " + trigger.textvalue;
                }
                if (trigger.triggertype == TriggersType.StartMarlin)
                {
                    value += "Запуск Marlin";
                }
                if (trigger.triggertype == TriggersType.StartApp)
                {
                    value += "Программа: " + trigger.appvalue;
                }
                TriggerPanel.Children.Add(CreateElement(value));
            }
        }

        private void LoadScriptCommand(Models.Main.Script script)
        {
            ScriptCommandPanel.Children.Clear();
            var commandindex = 0;
            var scriptindex = 0;
            foreach (var action in script.Actions)
            {
                if (action == 0)
                {
                    ScriptCommandPanel.Children.Add(CreateElement("Команда: " + Models.Main.Command.GetCommand(script.Commands[commandindex]).ToString()));
                    commandindex++;
                }
                if (action == 1)
                {
                    ScriptCommandPanel.Children.Add(CreateElement("Скрипт: " + Models.Main.Script.GetScript(script.Scripts[scriptindex]).ToString()));
                    scriptindex++;
                }
            }
        }

        private Border CreateElement(string trigger)
        {
            var border = new Border
            {
                Margin = new Thickness(10, 10, 10, 0),
                CornerRadius = new CornerRadius(15),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                BorderThickness = new Thickness(2),
                ToolTip = trigger,
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

            var textBlock = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(15, 3, 15, 5),
                Text = trigger
            };

            Grid.SetRow(textBlock, 0);
            Grid.SetColumn(textBlock, 0);

            grid.Children.Add(textBlock);

            border.Child = grid;

            return border;
        }
    }
}
