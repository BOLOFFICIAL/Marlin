using Marlin.Commands;
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
        private Models.Main.Command _selectedCommand = new();
        private Script _selectedScript = new();
        private int selectedhour = 0;
        private int selectedminute = 0;
        private string datetrigger = "";
        private int selectredday = 0;
        private bool _isCommand = true;
        private bool _isScript = false;

        private string _pagetitle = "Новый скрипт";

        private string triggervalue = "";
        private string apptrigger = "";

        private bool periodically = false;

        private GridLength texttriggerlength = new GridLength(0, GridUnitType.Pixel);
        private GridLength timetriggerlength = new GridLength(0, GridUnitType.Pixel);
        private GridLength marlintriggerlength = GridLength.Auto;
        private GridLength apptriggerlength = new GridLength(0, GridUnitType.Pixel);

        private GridLength commandlength = GridLength.Auto;
        private GridLength scriptlength = new GridLength(0, GridUnitType.Pixel);

        private GridLength datetriggerlength = GridLength.Auto;
        private GridLength weektriggerlength = new GridLength(0, GridUnitType.Pixel);

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

            var now = DateTime.Now;
            selectredday = (int)now.DayOfWeek;
            SelectedHour = now.Hour;
            SelectedMinute = now.Minute;
            DateTrigger = now.Date.ToString("dd.MM");

            if (Context.SelectedId > -1)
            {
                Context.Script = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Script.GetScript(Context.SelectedId)));
                LoadActions();
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

        public bool IsCommand
        {
            get => _isCommand;
            set
            {
                if (Set(ref _isCommand, value))
                {
                    IsScript = !value;
                    if (IsScript)
                    {
                        Scriptlength = GridLength.Auto;
                        Commandlength = new GridLength(0, GridUnitType.Pixel);
                    }
                }
            }
        }

        public bool IsScript
        {
            get => _isScript;
            set
            {
                if (Set(ref _isScript, value))
                {
                    IsCommand = !value;
                    if (IsCommand)
                    {
                        Commandlength = GridLength.Auto;
                        Scriptlength = new GridLength(0, GridUnitType.Pixel);
                    }
                }
            }
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
            set
            {
                if (!value)
                {
                    if (!Program.Authentication("Для снятия защиты подтвердите пароль", check: true))
                    {
                        return;
                    }
                }
                Set(ref Context.Script.Checkpuss, value);
            }
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

        public List<Models.Main.Command> Commands
        {
            get => Context.ProgramData.Commands;
        }

        public List<Script> Scripts
        {
            get
            {
                if (Context.SelectedId > -1)
                {
                    return ScriptsWithoutLoops(Context.SelectedId);
                }
                else
                {
                    return Context.ProgramData.Scripts;
                }
            }
        }

        public Models.Main.Command SelectedCommand
        {
            get => _selectedCommand;
            set => Set(ref _selectedCommand, value);
        }

        public Script SelectedScript
        {
            get => _selectedScript;
            set => Set(ref _selectedScript, value);
        }

        public string PageTitle
        {
            get => _pagetitle;
            set => Set(ref _pagetitle, value);
        }

        public bool Periodically
        {
            get => periodically;
            set
            {
                Set(ref periodically, value);
                if (value)
                {
                    Datetriggerlength = new GridLength(0, GridUnitType.Pixel);
                    Weektriggerlength = GridLength.Auto;
                }
                else
                {
                    Datetriggerlength = GridLength.Auto;
                    Weektriggerlength = new GridLength(0, GridUnitType.Pixel);
                }
            }
        }

        public GridLength TextTriggerLength
        {
            get => texttriggerlength;
            set => Set(ref texttriggerlength, value);
        }

        public GridLength TimeTriggerLength
        {
            get => timetriggerlength;
            set => Set(ref timetriggerlength, value);
        }

        public GridLength AppTriggerLength
        {
            get => apptriggerlength;
            set => Set(ref apptriggerlength, value);
        }

        public GridLength MarlinTriggerLength
        {
            get => marlintriggerlength;
            set => Set(ref marlintriggerlength, value);
        }

        public GridLength Datetriggerlength
        {
            get => datetriggerlength;
            set => Set(ref datetriggerlength, value);
        }

        public GridLength Weektriggerlength
        {
            get => weektriggerlength;
            set => Set(ref weektriggerlength, value);
        }

        public GridLength Commandlength
        {
            get => commandlength;
            set => Set(ref commandlength, value);
        }

        public GridLength Scriptlength
        {
            get => scriptlength;
            set => Set(ref scriptlength, value);
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
                    if (SelectedTrigger == Program.Triggers[(int)TriggersType.Phrase])
                    {
                        TextTriggerLength = GridLength.Auto;
                        TimeTriggerLength = new GridLength(0, GridUnitType.Pixel);
                        MarlinTriggerLength = new GridLength(0, GridUnitType.Pixel);
                        AppTriggerLength = new GridLength(0, GridUnitType.Pixel);
                    }
                    if (SelectedTrigger == Program.Triggers[(int)TriggersType.Time])
                    {
                        TextTriggerLength = new GridLength(0, GridUnitType.Pixel);
                        TimeTriggerLength = GridLength.Auto;
                        MarlinTriggerLength = new GridLength(0, GridUnitType.Pixel);
                        AppTriggerLength = new GridLength(0, GridUnitType.Pixel);
                    }
                    if (SelectedTrigger == Program.Triggers[(int)TriggersType.StartMarlin])
                    {
                        TextTriggerLength = new GridLength(0, GridUnitType.Pixel);
                        TimeTriggerLength = new GridLength(0, GridUnitType.Pixel);
                        MarlinTriggerLength = GridLength.Auto;
                        AppTriggerLength = new GridLength(0, GridUnitType.Pixel);
                    }
                    if (SelectedTrigger == Program.Triggers[(int)TriggersType.StartApp])
                    {
                        TextTriggerLength = new GridLength(0, GridUnitType.Pixel);
                        TimeTriggerLength = new GridLength(0, GridUnitType.Pixel);
                        MarlinTriggerLength = new GridLength(0, GridUnitType.Pixel);
                        AppTriggerLength = GridLength.Auto;
                    }
                }
            }
        }

        public string[] Triggers
        {
            get => Program.Triggers;
        }

        public string[] DaysOfWeek
        {
            get => Program.DaysOfWeek;
        }

        public int[] Hours
        {
            get => Program.Hourss;
        }

        public int[] Minutes
        {
            get => Program.Minutes;
        }

        public int SelectedHour
        {
            get => selectedhour;
            set => Set(ref selectedhour, value);
        }

        public int SelectedMinute
        {
            get => selectedminute;
            set => Set(ref selectedminute, value);
        }

        public int SelectedDay
        {
            get => selectredday;
            set => Set(ref selectredday, value);
        }

        public string DateTrigger
        {
            get => datetrigger;
            set => Set(ref datetrigger, value);
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
            return !Program.Equals(Context.Script, Context.CopyScript) && Context.Script.Actions.Count > 1;
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
            if (IsCommand)
            {
                if (Context.Script.Commands.Count(cmd => cmd == SelectedCommand.id) == 1)
                {
                    Models.MessageBox.MakeMessage("В этом скрипте уже присутствует такая команда.\nДобавить команду повторно?", SystemFiles.Types.MessageType.YesNoQuestion);
                    if (Context.MessageBox.Answer == "No")
                    {
                        return;
                    }
                }
                var command = CreateElement(RemoveActionCommand, "Команда: " + SelectedCommand.ToString(), Context.Script.Commands.Count, 0, Context.Script.Actions.Count);

                StackPanel.Children.Add(command);

                Context.Script.Commands.Add(SelectedCommand.id);
                Context.Script.Actions.Add(0);
            }
            if (IsScript)
            {
                if (Context.Script.Scripts.Count(scrpt => scrpt == SelectedScript.id) == 1)
                {
                    Models.MessageBox.MakeMessage("В этом скрипте уже присутствует такой скрипт.\nДобавить скрипт повторно?", SystemFiles.Types.MessageType.YesNoQuestion);
                    if (Context.MessageBox.Answer == "No")
                    {
                        return;
                    }
                }
                var script = CreateElement(RemoveActionCommand, "Скрипт: " + SelectedScript.ToString(), Context.Script.Scripts.Count, 1, Context.Script.Actions.Count);

                StackPanel.Children.Add(script);

                Context.Script.Scripts.Add(SelectedScript.id);
                Context.Script.Actions.Add(1);
            }
        }

        private void OnRemoveActionCommandExecuted(object p)
        {
            var actioncode = ((string)p).Split(',');
            if (int.Parse(actioncode[1]) == 0)
            {
                Context.Script.Commands.RemoveAt(int.Parse(actioncode[0]));
            }
            if (int.Parse(actioncode[1]) == 1)
            {
                Context.Script.Scripts.RemoveAt(int.Parse(actioncode[0]));
            }
            Context.Script.Actions.RemoveAt(int.Parse(actioncode[2]));
            LoadActions();
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
            if (SelectedTrigger == Program.Triggers[(int)TriggersType.StartMarlin])
            {
                return true;
            }
            if (SelectedTrigger == Program.Triggers[(int)TriggersType.Time])
            {
                if (!periodically)
                {
                    if (datetrigger.Length > 0)
                    {
                        if (datetrigger.Contains(':'))
                        {
                            return false;
                        }
                        else
                        {
                            return DateTime.TryParse(DateTrigger, out DateTime date);
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return TextTrigger.Length > 0;
        }

        private void OnAddTriggerCommandExecuted(object p)
        {
            var trigger = new Models.Main.Trigger();
            string value = "";
            if (SelectedTrigger == Program.Triggers[(int)TriggersType.Phrase])
            {
                trigger.textvalue = TextTrigger.ToUpper();
                trigger.triggertype = TriggersType.Phrase;
                trigger.appvalue = "";
                value += "Фраза: " + trigger.textvalue;
            }
            if (SelectedTrigger == Program.Triggers[(int)TriggersType.Time])
            {
                if (periodically)
                {
                    trigger.textvalue = $"{Program.DaysOfWeek[SelectedDay]} в ";
                }
                else
                {
                    if (DateTrigger.Length > 0)
                    {
                        trigger.textvalue = $"{DateTime.Parse(DateTrigger).ToString("dd.MM.yyyy")} в ";
                    }
                    else
                    {
                        trigger.textvalue = "В ";
                    }
                }
                var hour = SelectedHour < 10 ? "0" + SelectedHour.ToString() : SelectedHour.ToString();
                var minute = SelectedMinute < 10 ? "0" + SelectedMinute.ToString() : SelectedMinute.ToString();
                trigger.textvalue += $"{hour}:{minute}";
                trigger.triggertype = TriggersType.Time;
                trigger.appvalue = "";
                value += "Время: " + trigger.textvalue;
            }
            if (SelectedTrigger == Program.Triggers[(int)TriggersType.StartMarlin])
            {
                trigger.textvalue = "";
                trigger.triggertype = TriggersType.StartMarlin;
                trigger.appvalue = "";
                value += "Запуск Marlin";
            }
            if (SelectedTrigger == Program.Triggers[(int)TriggersType.StartApp])
            {
                trigger.textvalue = TextTrigger;
                trigger.triggertype = TriggersType.StartApp;
                trigger.appvalue = AppTrigger;
                value += "Программа: " + AppTrigger;
            }
            if (ValidationTrigger(trigger))
            {
                TriggerPanel.Children.Add(CreateElement(RemoveTriggerCommand, value, Context.Script.Triggers.Count, 0, 0));
                Context.Script.Triggers.Add(trigger);

                TextTrigger = "";
                AppTrigger = "";
            }

        }

        private bool ValidationTrigger(Models.Main.Trigger trigger)
        {
            foreach (var trg in Context.Script.Triggers)
            {
                if (Program.Equals(trg, trigger))
                {
                    Models.MessageBox.MakeMessage("У элемента уже присутствует такой триггер", MessageType.Error);
                    return false;
                }
            }
            foreach (var command in Context.ProgramData.Commands)
            {
                foreach (var trg in command.Triggers)
                {
                    if (trg.triggertype == TriggersType.Phrase)
                    {
                        if (Program.Equals(trg, trigger))
                        {
                            Models.MessageBox.MakeMessage("У одной из команд есть такой триггер.", MessageType.Error);
                            return false;
                        }
                    }
                }
            }
            foreach (var script in Context.ProgramData.Scripts)
            {
                foreach (var trg in script.Triggers)
                {
                    if (trg.triggertype == TriggersType.Phrase)
                    {
                        if (Program.Equals(trg, trigger))
                        {
                            Models.MessageBox.MakeMessage("У одного из скриптов есть такой триггер.", MessageType.Error);
                            return false;
                        }
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
            return true;
        }

        private void LoadTrigger()
        {
            TriggerPanel.Children.Clear();
            for (int i = 0; i < Context.Script.Triggers.Count; i++)
            {
                string value = "";
                if (Context.Script.Triggers[i].triggertype == TriggersType.Phrase)
                {
                    value += "Фраза: " + Context.Script.Triggers[i].textvalue;
                }
                if (Context.Script.Triggers[i].triggertype == TriggersType.Time)
                {
                    value += "Время: " + Context.Script.Triggers[i].textvalue;
                }
                if (Context.Script.Triggers[i].triggertype == TriggersType.StartMarlin)
                {
                    value += "Запуск Marlin";
                }
                if (Context.Script.Triggers[i].triggertype == TriggersType.StartApp)
                {
                    value += "Программа: " + Context.Script.Triggers[i].appvalue;
                }
                TriggerPanel.Children.Add(CreateElement(RemoveTriggerCommand, value, i, 0, 0));
            }
        }

        private void LoadActions()
        {
            panel.Children.Clear();
            int comandindex = 0;
            int scriptindex = 0;

            for (int i = 0; i < Context.Script.Actions.Count; i++)
            {
                if (Context.Script.Actions[i] == 0)
                {
                    var command = Models.Main.Command.GetCommand(Context.Script.Commands[comandindex]);
                    if (command != null)
                    {
                        panel.Children.Add(CreateElement(RemoveActionCommand, "Команда: " + command.Title, comandindex, Context.Script.Actions[i], i));
                    }
                    comandindex++;
                }
                if (Context.Script.Actions[i] == 1)
                {
                    var script = Script.GetScript(Context.Script.Scripts[scriptindex]);
                    if (script != null)
                    {
                        panel.Children.Add(CreateElement(RemoveActionCommand, "Скрипт: " + script.Title, scriptindex, Context.Script.Actions[i], i));
                    }
                    scriptindex++;
                }
            }
        }

        private Border CreateElement(ICommand command, string commandname, int number, int action, int action_number)
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
                CommandParameter = number + "," + action + "," + action_number,
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

        private bool CheckLoopInScripts(int script_id, Script script)
        {
            foreach (var sc in script.Scripts)
            {
                var sctpt = Script.GetScript(sc);
                if (sctpt.id == script_id)
                {
                    return false;
                }
                else
                {
                    if (!CheckLoopInScripts(script_id, sctpt))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private List<Script> ScriptsWithoutLoops(int scriptid)
        {
            var scriptsWithoutLoops = new List<Script>();
            foreach (var script in Context.ProgramData.Scripts)
            {
                if (script.id != scriptid && CheckLoopInScripts(scriptid, script))
                {
                    scriptsWithoutLoops.Add(script);
                }
            }
            return scriptsWithoutLoops;
        }
    }
}
