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
            }
        }

        private void MakeMessage(String Content, AuthorType type)
        {
            Grid mainGrid = new Grid();
            ColumnDefinition column1 = new ColumnDefinition();
            ColumnDefinition column2 = new ColumnDefinition();
            column1.Width = new GridLength(1, GridUnitType.Star);
            column2.Width = new GridLength(1, GridUnitType.Star);
            mainGrid.ColumnDefinitions.Add(column1);
            mainGrid.ColumnDefinitions.Add(column2);
            Border border = new Border();
            border.CornerRadius = new CornerRadius(15);
            border.SetBinding(Border.BorderBrushProperty, new Binding("PageColor") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            border.BorderThickness = new Thickness(2);
            if (type == AuthorType.User)
            {
                border.Margin = new Thickness(1, 4, 50, 5);
                border.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                border.Margin = new Thickness(50, 4, 1, 5);
                border.HorizontalAlignment = HorizontalAlignment.Right;
            }
            border.Padding = new Thickness(10, 5, 10, 5);
            
            Grid innerGrid = new Grid();
            RowDefinition row1 = new RowDefinition();
            RowDefinition row2 = new RowDefinition();
            row1.Height = new GridLength(1, GridUnitType.Star);
            row2.Height = new GridLength(1, GridUnitType.Star);
            innerGrid.RowDefinitions.Add(row1);
            innerGrid.RowDefinitions.Add(row2);

            Grid authorGrid = new Grid();
            ColumnDefinition authorColumn1 = new ColumnDefinition();
            ColumnDefinition authorColumn2 = new ColumnDefinition();
            authorColumn1.Width = new GridLength(1, GridUnitType.Star);
            authorColumn2.Width = new GridLength(1, GridUnitType.Star);
            authorGrid.ColumnDefinitions.Add(authorColumn1);
            authorGrid.ColumnDefinitions.Add(authorColumn2);

            Label authorTextBlock = new Label();
            authorTextBlock.FontSize = 13;
            authorTextBlock.SetBinding(Label.ForegroundProperty, new Binding("FontColor") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            authorTextBlock.HorizontalContentAlignment = HorizontalAlignment.Right;
            authorTextBlock.FontWeight = FontWeights.Bold;
            authorTextBlock.Margin = new Thickness(0, 0, 5, 0);
            if (type == AuthorType.User)
            {
                authorTextBlock.SetBinding(Label.ContentProperty, new Binding("Author") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            }
            else
            {
                authorTextBlock.Content = "Marlin";
            }
            Grid.SetColumn(authorTextBlock, 0);

            Label timeTextBlock = new Label();
            timeTextBlock.SetBinding(Label.ForegroundProperty, new Binding("FontColor") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            timeTextBlock.Content = $"{DateTime.UtcNow.ToLocalTime().Hour:D2}:{DateTime.UtcNow.ToLocalTime().Minute:D2}";
            Grid.SetColumn(timeTextBlock, 1);
            timeTextBlock.FontSize = 13;
            timeTextBlock.HorizontalContentAlignment = HorizontalAlignment.Left;
            timeTextBlock.FontWeight = FontWeights.Bold;

            authorGrid.Children.Add(authorTextBlock);
            authorGrid.Children.Add(timeTextBlock);

            Grid contentGrid = new Grid();
            TextBlock contentTextBlock = new TextBlock();
            contentTextBlock.Text = Content;
            contentTextBlock.TextWrapping = TextWrapping.Wrap;
            contentTextBlock.FontSize = 18;
            contentTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            contentGrid.Children.Add(contentTextBlock);
            Grid.SetRow(authorGrid, 0);
            Grid.SetRow(contentGrid, 1);
            innerGrid.Children.Add(authorGrid);
            innerGrid.Children.Add(contentGrid);
            border.Child = innerGrid;
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
