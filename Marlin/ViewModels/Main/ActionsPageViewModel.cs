using Marlin.Commands;
using Marlin.SystemFiles;
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
        public ICommand ToMainCommand { get; }

        public ActionsPageViewModel()
        {
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
        }

        public string Title
        {
            get => Context.Action;
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
                var panel = new StackPanel();

                for (int i = 0; i < 10; i++)
                {
                    Border border = new Border
                    {
                        Margin = new Thickness(6, 3, 6, 3),
                        Padding = new Thickness(0),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor)), // Установите нужный цвет границы
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.ExternalBackgroundColor)),  // Установите нужный цвет фона
                        BorderThickness = new Thickness(2),
                        CornerRadius = new CornerRadius(20)
                    };

                    border.MouseLeftButtonDown += Border_MouseLeftButtonDown;

                    Grid grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    TextBlock textBlock = new TextBlock
                    {
                        FontSize = 15,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.FontColor)), // Установите нужный цвет текста
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = $"Bolofficial {i}"
                    };
                    Button buttonRun = new Button
                    {
                        Margin = new Thickness(0, 5, 5, 5),
                        Padding = new Thickness(5, 0, 5, 4),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.FontColor)), // Установите нужный цвет текста кнопки
                        BorderBrush = new SolidColorBrush(Colors.Transparent), // Установите нужный цвет границы кнопки
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor)), // Установите нужный цвет фона кнопки
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Content = "Запустить"
                    };
                    Button buttonEdit = new Button
                    {
                        Margin = new Thickness(0, 5, 20, 5),
                        Padding = new Thickness(0, 0, 0, 4),
                        Width = 33,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.FontColor)), // Установите нужный цвет текста кнопки
                        BorderBrush = new SolidColorBrush(Colors.Transparent), // Установите нужный цвет границы кнопки
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Context.Settings.Theme.PageColor)), // Установите нужный цвет фона кнопки
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Content = "✏️"
                    };
                    Grid.SetColumn(textBlock, 0);
                    Grid.SetColumn(buttonRun, 1);
                    Grid.SetColumn(buttonEdit, 2);
                    grid.Children.Add(textBlock);
                    grid.Children.Add(buttonRun);
                    grid.Children.Add(buttonEdit);
                    border.Child = grid;
                    panel.Children.Add(border);
                }
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
                        if (Program.Authentication("Для открытия содержимого введите пароль"))
                        {
                            TitleAbout = textBox.Text;
                        }

                        //Models.MessageBox.MakeMessage(textBox.Text);
                    }
                }
            }
        }

        private void OnToMainCommandExecuted(object p)
        {
            Program.SetPage(new MainPage());
        }
    }
}
