using Marlin.Models.Main;
using Marlin.SystemFiles.Types;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Marlin.SystemFiles
{
    public class Program
    {
        public static string[] Genders = { "Мужской", "Женский" };

        public static string[] Triggers = { "Фраза", "Время", "Запуск Marlin", "Запуск программы" };

        public static string[] Objects = { "Фаил", "Папка", "Url", "Программа" };

        public static string[] ObjectActions = { "Запустить", "Закрыть", "Удалить" };

        public static string[] Actions = { "Сделать свое действие", "Встроенные методы" };

        public static string[] EmbeddedActions = { "Озвучивание текста", "Нажатие клавиш", "Перемещение курсора" };

        public static int[] Speeds = Enumerable.Range(-5, 16).ToArray();

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
            if (DateTime.Now - Context.LastCheckPassword > TimeSpan.FromSeconds(Context.CopySettings.TimeCheckPassword) || Context.Settings.NewPassword.Length > 0)
            {
                Models.MessageBox.MakeMessage(message, MessageType.TextQuestion);
                if (Context.MessageBox.Answer == Context.Settings.Password)
                {
                    Context.LastCheckPassword = DateTime.Now;
                    return true;
                }
                else
                {
                    Models.MessageBox.MakeMessage(error, MessageType.Error);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static void ChangePassword(string newpassword)
        {
            Context.Settings.Password = newpassword;
        }

        public static void AddCommand(Command command)
        {
            command.id = ProgramData.Commands.Count;
            ProgramData.Commands.Add(command);
        }
    }
}
