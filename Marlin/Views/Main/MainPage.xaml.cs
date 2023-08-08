using Marlin.SystemFiles;
using Marlin.Views.Window;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Marlin.Views.Main
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            if (Context.Settings.Password.Length == 0)
            {
                Voix.SpeakAsync("Прежде чем приступить к использованию необходимо зарегистрироваться");
                System.Windows.Window window = new System.Windows.Window
                {
                    SizeToContent = System.Windows.SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    WindowStyle = WindowStyle.None,
                    ResizeMode = ResizeMode.NoResize,
                    Content = new RegistrationPage()
                };
                Context.Window = window;
                window.ShowDialog();
            }
            if (Context.Settings.Password.Length == 0)
            {
                Environment.Exit(0);
            }
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menu)
            {
                switch (menu.Header.ToString())
                {
                    case "Настройки": NavigationService.Navigate(new SettingsPage()); break;
                    case "Скрипты": Models.MessageBox.MakeMessage("Страница не доступна", MessageType.Error); break;
                    case "Действия": Models.MessageBox.MakeMessage("Страница не доступна", MessageType.Error); break;
                }
            }

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Context.MainPage.Command.Length > 0)
            {
                MakeMessage(Context.MainPage.Command, AuthorType.User);
                MakeMessage("BolofficialBolofficialBolofficialBolofficialBolofficialBolofficialBolofficialBolofficialBolofficial", AuthorType.Marlin);
            }
        }

        private void ContentTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock text)  
            {
                Context.MainPage.Command = text.Text;
            }
        }

        private void MakeMessage(String Content, AuthorType type)
        {
            // Создание и настройка основной сетки
            Grid mainGrid = new Grid();
            ColumnDefinition column1 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            ColumnDefinition column2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            mainGrid.ColumnDefinitions.Add(column1);
            mainGrid.ColumnDefinitions.Add(column2);

            // Создание и настройка границы
            Border border = new Border
            {
                CornerRadius = new CornerRadius(15),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(10, 5, 10, 5)
            };
            border.SetBinding(Border.BackgroundProperty, new Binding("ExternalBackgroundColor") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            border.SetBinding(Border.BorderBrushProperty, new Binding("PageColor") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            if (type == AuthorType.User)
            {
                border.Margin = new Thickness(5, 10, -30, 10);
                border.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                border.Margin = new Thickness(-30, 10, 5, 10);
                border.HorizontalAlignment = HorizontalAlignment.Right;
            }

            // Создание и настройка сетки внутри границы
            Grid innerGrid = new Grid();
            RowDefinition row1 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            RowDefinition row2 = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            innerGrid.RowDefinitions.Add(row1);
            innerGrid.RowDefinitions.Add(row2);

            // Создание и настройка сетки для автора
            Grid authorGrid = new Grid();
            ColumnDefinition authorColumn1 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) };
            ColumnDefinition authorColumn2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) };
            authorGrid.HorizontalAlignment = HorizontalAlignment.Center;
            authorGrid.ColumnDefinitions.Add(authorColumn1);
            authorGrid.ColumnDefinitions.Add(authorColumn2);

            // Создание и настройка блока с именем автора
            Label authorTextBlock = new Label
            {
                FontSize = 13,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, type == AuthorType.User ? 5 : 0, 0)
            };
            authorTextBlock.SetBinding(Label.ForegroundProperty, new Binding("FontColor") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            if (type == AuthorType.User)
            {
                authorTextBlock.SetBinding(Label.ContentProperty, new Binding("Author") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            }
            else
            {
                authorTextBlock.Content = "Marlin";
            }
            Grid.SetColumn(authorTextBlock, 0);

            // Создание и настройка блока с временем
            Label timeTextBlock = new Label
            {
                Content = $"{DateTime.UtcNow.ToLocalTime().Hour:D2}:{DateTime.UtcNow.ToLocalTime().Minute:D2}",
                FontSize = 13,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                FontWeight = FontWeights.Bold
            };
            timeTextBlock.SetBinding(Label.ForegroundProperty, new Binding("FontColor") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            Grid.SetColumn(timeTextBlock, 1);

            authorGrid.Children.Add(authorTextBlock);
            authorGrid.Children.Add(timeTextBlock);

            // Создание и настройка сетки для контента
            Grid contentGrid = new Grid();
            TextBlock contentTextBlock = new TextBlock
            {
                Text = Content,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            if (type == AuthorType.User)
            {
                contentTextBlock.MouseLeftButtonDown += ContentTextBlock_MouseLeftButtonDown;
            }

            contentGrid.Children.Add(contentTextBlock);

            // Расположение элементов внутри сетки
            Grid.SetRow(authorGrid, 0);
            Grid.SetRow(contentGrid, 1);
            innerGrid.Children.Add(authorGrid);
            innerGrid.Children.Add(contentGrid);
            border.Child = innerGrid;

            // Расположение границы в основной сетке
            Grid.SetColumn(border, type == AuthorType.User ? 0 : 1);
            mainGrid.Children.Add(border);

            var mes = new List<Grid>();
            mes.Add(mainGrid);

            foreach (var message in Context.MainPage.Message)
            {
                mes.Add(message);
            }
            Context.MainPage.Message = mes;

            if (type == AuthorType.User)
            {
                Context.MainPage.Command = "";
            }
        }
    }
}
