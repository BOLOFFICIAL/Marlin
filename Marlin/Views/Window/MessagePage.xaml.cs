using Marlin.SystemFiles;
using System.Windows.Controls;

namespace Marlin.Views.Message
{
    /// <summary>
    /// Логика взаимодействия для MessagePage.xaml
    /// </summary>
    public partial class MessagePage : Page
    {
        public MessagePage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Content.ToString() != "➤")
                {
                    Context.MessageBox.Answer = btn.Content.ToString();
                }
            }
            Context.Window.Close();
            Voix.SpeakAsync("");
        }
    }
}
