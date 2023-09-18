using Marlin.Commands;
using Marlin.Models.Main;
using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Newtonsoft.Json;
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

        private StackPanel panel = new StackPanel();
        private Command _selectedCommand = new();
        private string _pagetitle = "Новый скрипт";

        public ScriptPageViewModel()
        {
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
            ButtonActionCommand = new LambdaCommand(OnButtonActionCommandExecuted, CanButtonActionCommandExecute);
            AddActionCommand = new LambdaCommand(OnAddActionCommandExecuted, CanAddActionCommandExecute);
            RemoveActionCommand = new LambdaCommand(OnRemoveActionCommandExecuted);

            Context.Script = new Models.Main.Script();
            Context.CopyScript = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Context.Script));

            if (Context.SelectedId > -1)
            {

                Context.Script = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Script.GetScript(Context.SelectedId)));
                LoadCommands();
                PageTitle = Context.Script.Title;
                Context.CopyScript = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Script.GetScript(Context.SelectedId)));
            }

            else
            {
                if (Context.ProgramData.Scripts.Count > 0)
                {
                    Title = "Скипт" + (Context.ProgramData.Scripts[Context.ProgramData.Scripts.Count - 1].id + 1).ToString();
                }
                else
                {
                    Title = "Скипт" + Context.Script.id.ToString();
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
            return !Context.Script.Equals(Context.CopyScript);
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
            var command = CreateCommand(SelectedCommand.ToString(), Context.Script.Commands.Count);

            StackPanel.Children.Add(command);

            Context.Script.Commands.Add(SelectedCommand.id);
        }

        private void OnRemoveActionCommandExecuted(object p)
        {
            Context.Script.Commands.RemoveAt((int)p);
            LoadCommands();
        }

        private bool ValidationCommand()
        {
            return true;
        }

        private void LoadCommands()
        {
            panel.Children.Clear();
            for (int i = 0; i < Context.Script.Commands.Count; i++)
            {
                panel.Children.Add(CreateCommand(Command.GetCommand(Context.Script.Commands[i]).Title, i));
            }
        }

        private Border CreateCommand(string command,int number)
        {
            var border = new Border
            {
                Margin = new Thickness(0, 10, 0, 0),
                CornerRadius = new CornerRadius(25),
                HorizontalAlignment = HorizontalAlignment.Left,
                BorderThickness = new Thickness(2),
            };

            BindingOperations.SetBinding(border, Border.BackgroundProperty, new Binding("InternalBackgroundColor")
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
                Command = RemoveActionCommand,
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
