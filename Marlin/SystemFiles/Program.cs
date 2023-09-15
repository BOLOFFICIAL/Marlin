using Marlin.SystemFiles.Types;
using Microsoft.Win32;
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

        public static string[] Objects = { "Фаил", "Папка", "Url" };

        public static string[] ObjectActions = { "Открыть", "Закрыть", "Удалить" };

        public static string[] ObjectActionsSimple = { "Открыть", "Удалить" };

        public static string[] Actions = { "Сделать свое действие", "Встроенные методы" };

        public static string[] EmbeddedActions = { "Озвучивание текста", "Нажатие клавиш", "Перемещение курсора" };

        public static int[] Speeds = Enumerable.Range(-5, 16).ToArray();

        public static async Task SetPage(Page page)
        {
            //for (double i = 1; i > 0; i -= 0.2)
            //{
            //    Context.MainWindow.Opacity = i;
            //    await Task.Delay(1);
            //}
            Context.MainWindow.Content = page;
            //for (double i = 0.1; i <= 1; i += 0.2)
            //{
            //    Context.MainWindow.Opacity = i;
            //    await Task.Delay(1);
            //}
        }

        public static void AddToStartup()
        {
            string appPath = Process.GetCurrentProcess().MainModule.FileName;
            string appName = "Marlin";
            try
            {
                RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (startupKey != null)
                {
                    startupKey.SetValue(appName, appPath);
                    startupKey.Close();
                    Console.WriteLine("Программа успешно добавлена в автозапуск.");
                }
                else
                {
                    Console.WriteLine("Не удалось открыть ключ реестра.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }
        }

        public static void RemoveFromStartup()
        {
            string appName = "Marlin";
            try
            {
                RegistryKey startupKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (startupKey != null)
                {
                    if (startupKey.GetValue(appName) != null)
                    {
                        startupKey.DeleteValue(appName, false);
                        Console.WriteLine("Программа успешно удалена из автозапуска.");
                    }
                    else
                    {
                        Console.WriteLine("Программа не найдена в автозапуске.");
                    }

                    startupKey.Close();
                }
                else
                {
                    Console.WriteLine("Не удалось открыть ключ реестра.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }
        }

        public static bool Authentication(string message, string error = "Введен неправильный пароль", bool check = false)
        {
            if (DateTime.Now - Context.LastCheckPassword > TimeSpan.FromSeconds(Context.Settings.TimeCheckPassword) || check)
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
    }
}
