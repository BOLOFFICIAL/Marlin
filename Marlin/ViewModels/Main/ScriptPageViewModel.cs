using Marlin.Commands;
using Marlin.Models.Main;
using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Marlin.ViewModels.Main
{
    public class ScriptPageViewModel : ViewModel
    {
        public ICommand ToMainCommand { get; }
        public ICommand ButtonActionCommand { get; }
        public ICommand AddActionCommand { get; }
        public ICommand RemoveActionCommand { get; }
        public ICommand AppTriggerCommand { get; }
        public ICommand AddTriggerCommand { get; }
        public ICommand RemoveTriggerCommand { get; }

        private StackPanel panel = new StackPanel();
        private StackPanel triggerpanel = new StackPanel();
        private string selectedtrigger;
        private Command _selectedCommand = new();
        private string _pagetitle = "Новый скрипт";
        private string triggervalue;
        private string apptrigger;

        private GridLength texttriggerlength = new GridLength(0, GridUnitType.Pixel);
        private GridLength marlintriggerlength = GridLength.Auto;
        private GridLength apptriggerlength = new GridLength(0, GridUnitType.Pixel);

        public ScriptPageViewModel()
        {
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
            ButtonActionCommand = new LambdaCommand(OnButtonActionCommandExecuted, CanButtonActionCommandExecute);
            AddActionCommand = new LambdaCommand(OnAddActionCommandExecuted, CanAddActionCommandExecute);
            RemoveActionCommand = new LambdaCommand(OnRemoveActionCommandExecuted);
            AppTriggerCommand = new LambdaCommand(OnAppTriggerCommandExecuted);
            RemoveTriggerCommand = new LambdaCommand(OnRemoveTriggerCommandExecuted);
            AddTriggerCommand = new LambdaCommand(OnAddTriggerCommandExecuted, CanAddTriggerCommandExecute);
            SelectedTrigger = Program.Triggers[0];

            Context.Script = new Models.Main.Script();
            Context.CopyScript = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Context.Script));

            if (Context.SelectedId > -1)
            {

                Context.Script = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Script.GetScript(Context.SelectedId)));
                LoadCommands();
                PageTitle = Context.Script.Title;
                Context.CopyScript = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Script.GetScript(Context.SelectedId)));
                LoadTrigger();
            }

            else
            {
                if (Context.ProgramData.Scripts.Count > 0)
                {
                    Title = "Скрипт" + (Context.ProgramData.Scripts[Context.ProgramData.Scripts.Count - 1].id + 1).ToString();
                }
                else
                {
                    Title = "Скрипт" + Context.Script.id.ToString();
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

        public StackPanel StackPanel
        {
            get => panel;
            set => Set(ref panel, value);
        }

        public StackPanel TriggerPanel
        {
            get => triggerpanel;
            set => Set(ref triggerpanel, value);
        }

        public string Title
        {
            get => Context.Script.Title;
            set => Set(ref Context.Script.Title, value);
        }

        public bool Checkpuss
        {
            get => Context.Script.Checkpuss;
            set => Set(ref Context.Script.Checkpuss, value);
        }

        public int TimeDelay
        {
            get => Context.Script.TimeDelay;
            set => Set(ref Context.Script.TimeDelay, value);
        }

        public string Comment
        {
            get => Context.Script.Comment;
            set => Set(ref Context.Script.Comment, value);
        }

        public List<Command> Commands
        {
            get => Context.ProgramData.Commands;
        }

        public Command SelectedCommand
        {
            get => _selectedCommand;
            set => Set(ref _selectedCommand, value);
        }

        public string PageTitle
        {
            get => _pagetitle;
            set => Set(ref _pagetitle, value);
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

        public string[] Triggers
        {
            get => Program.Triggers;
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

        private void OnButtonActionCommandExecuted(object p)
        {
            if (ValidationCommand())
            {
                if (Context.SelectedId > -1)
                {
                    Script.SetScript(Context.SelectedId, Context.Script);
                }
                else
                {
                    Script.AddScript(Context.Script);
                }

                ProgramData.SaveData();
                Program.SetPage(new ActionsPage());
            }
        }

        private bool CanButtonActionCommandExecute(object p)
        {
            return !Context.Script.Equals(Context.CopyScript) && Context.Script.Commands.Count > 1;
        }

        private void OnToMainCommandExecuted(object p)
        {
            Program.SetPage(new ActionsPage());
        }

        private bool CanAddActionCommandExecute(object p)
        {
            return Context.ProgramData.Commands.Count > 0;
        }

        private void OnAddActionCommandExecuted(object p)
        {
            if (Context.Script.Commands.Contains(Command.GetCommand(SelectedCommand.ToString()).id))
            {
                Models.MessageBox.MakeMessage("В этом скрипте уже присутствует такая команда.\nДобавить команду повторно?", SystemFiles.Types.MessageType.YesNoQuestion);
                if (Context.MessageBox.Answer == "No")
                {
                    return;
                }
            }
            var command = CreateCommand(RemoveActionCommand, SelectedCommand.ToString(), Context.Script.Commands.Count);

            StackPanel.Children.Add(command);

            Context.Script.Commands.Add(SelectedCommand.id);
        }

        private void OnRemoveActionCommandExecuted(object p)
        {
            Context.Script.Commands.RemoveAt((int)p);
            LoadCommands();
        }

        private void OnRemoveTriggerCommandExecuted(object p)
        {
            Context.Script.Triggers.RemoveAt((int)p);
            LoadTrigger();
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
                trigger.textvalue = TextTrigger.ToUpper();
                trigger.triggertype = TriggerType.Phrase;
                trigger.appvalue = "";
                value += "Фраза: " + trigger.textvalue;
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
                TriggerPanel.Children.Add(CreateCommand(RemoveTriggerCommand, value, Context.Script.Triggers.Count));
                Context.Script.Triggers.Add(trigger);

                TextTrigger = "";
                AppTrigger = "";
            }

        }

        private bool ValidationTrigger(Models.Main.Trigger trigger)
        {
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
            foreach (var trg in Context.Script.Triggers)
            {
                if (trg.Equals(trigger))
                {
                    Models.MessageBox.MakeMessage("У элемента уже присутствует такой триггер", MessageType.Error);
                    return false;
                }
            }
            foreach (var command in Context.ProgramData.Commands)
            {
                foreach (var trg in command.Triggers)
                {
                    if (trg.Equals(trigger))
                    {
                        Models.MessageBox.MakeMessage("У одной из команд есть такой триггер.", MessageType.Error);
                        return false;
                    }
                }
            }
            foreach (var script in Context.ProgramData.Scripts)
            {
                foreach (var trg in script.Triggers)
                {
                    if (trg.Equals(trigger))
                    {
                        Models.MessageBox.MakeMessage("У одного из скриптов есть такой триггер.", MessageType.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ValidationCommand()
        {
            foreach (var command in Context.ProgramData.Commands)
            {
                if (command.Title == Context.Script.Title && Context.Script.Title != Context.CopyScript.Title)
                {
                    Models.MessageBox.MakeMessage("Команда с таким именем уже существует", MessageType.Error);
                    return false;
                }
            }
            foreach (var scrpt in Context.ProgramData.Scripts)
            {
                if (scrpt.Title == Context.Script.Title && Context.Script.Title != Context.CopyScript.Title)
                {
                    Models.MessageBox.MakeMessage("Скрипт с таким именем уже существует", MessageType.Error);
                    return false;
                }
            }
            return true;//проверка уникальности имени скрипта среди команд и скриптов.
        }

        private void LoadTrigger()
        {
            TriggerPanel.Children.Clear();
            for (int i = 0; i < Context.Script.Triggers.Count; i++)
            {
                string value = "";
                if (Context.Script.Triggers[i].triggertype == TriggerType.Phrase)
                {
                    value += "Фраза: " + Context.Script.Triggers[i].textvalue;
                }
                if (Context.Script.Triggers[i].triggertype == TriggerType.Time)
                {
                    value += "Время: " + Context.Script.Triggers[i].textvalue;
                }
                if (Context.Script.Triggers[i].triggertype == TriggerType.StartMarlin)
                {
                    value += "Запуск Marlin";
                }
                if (Context.Script.Triggers[i].triggertype == TriggerType.StartApp)
                {
                    value += "Программа: " + Context.Script.Triggers[i].appvalue;
                }
                TriggerPanel.Children.Add(CreateCommand(RemoveTriggerCommand, value, i));
            }
        }

        private void LoadCommands()
        {
            panel.Children.Clear();
            for (int i = 0; i < Context.Script.Commands.Count; i++)
            {
                var command = Command.GetCommand(Context.Script.Commands[i]);
                if (command != null)
                {
                    panel.Children.Add(CreateCommand(RemoveActionCommand, command.Title, i));
                }
            }
        }

        private Border CreateCommand(ICommand command, string commandname, int number)
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
                Text = commandname
            };

            Grid.SetRow(textBlock, 0);
            Grid.SetColumn(textBlock, 0);

            var button = new Button
            {
                Content = "✖",
                Padding = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold,
                Command = command,
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
