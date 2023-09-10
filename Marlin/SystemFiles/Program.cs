using Marlin.SystemFiles.Types;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Marlin.SystemFiles
{
    public class Program
    {
        public static async Task SetPage(Page page)
        {
            for (double i = 1; i > 0; i -= 0.1)
            {
                Context.MainWindow.Opacity = i;
                await Task.Delay(1);
            }
            Context.MainWindow.Content = page;
            for (double i = 0.1; i <= 1; i += 0.1)
            {
                Context.MainWindow.Opacity = i;
                await Task.Delay(1);
            }
        }

        public static void AddAutorun()
        {
            string path = Process.GetCurrentProcess().MainModule.FileName;
            string command = "reg add \"HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\" /v Marlin /t REG_SZ /d " + $"{path}";

            WinSystem.RunCmd(command);
        }

        public static void RemoveAutorun()
        {
            string command = $"reg delete \"HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\" /v Marlin /f";
            WinSystem.RunCmd(command);
        }

        public static bool Authentication(string message, string error = "Введен неправильный пароль")
        {
            Models.MessageBox.MakeMessage(message, MessageType.TextQuestion);
            if (Context.MessageBox.Answer == Context.Settings.Password)
            {
                return true;
            }
            else
            {
                Models.MessageBox.MakeMessage(error, MessageType.Error);
                return false;
            }
        }

        public static void ChangePassword(string newpassword)
        {
            Context.Settings.Password = newpassword;
        }
    }
}
