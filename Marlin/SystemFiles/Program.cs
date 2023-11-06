using Marlin.SystemFiles.Types;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Marlin.SystemFiles
{
    public class Program
    {
        public readonly static string[] Genders = { "Мужской", "Женский" };

        public readonly static string[] Triggers = { "Фраза", "Время", "Запуск Marlin", "Запуск программы" };

        public readonly static string[] Objects = { "Фаил", "Папка", "Url" };

        public readonly static string[] ObjectActions = { "Открыть", "Завершить работу", "Уничтожить процесс", "Удалить фаил" };

        public readonly static string[] ObjectActionsSimple = { "Открыть", "Удалить" };

        public readonly static string[] Actions = { "Сделать свое действие", "Встроенные методы" };

        public readonly static string[] EmbeddedActions = { "Озвучивание текста", "Нажатие клавиш", "Перемещение курсора" };

        public readonly static string[] DaysOfWeek = { "Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };

        public readonly static int[] Speeds = Enumerable.Range(-5, 16).ToArray();

        public readonly static int[] Hourss = Enumerable.Range(0, 24).ToArray();

        public readonly static int[] Minutes = Enumerable.Range(0, 60).ToArray();

        public readonly static int[] Days = Enumerable.Range(1, 32).ToArray();

        public readonly static int[] Months = Enumerable.Range(1, 13).ToArray();

        public readonly static int[] Years = Enumerable.Range(2000, 2100).ToArray();

        public static async Task SetPage(Page page)
        {
            for (double i = 1; i > 0; i -= 0.2)
            {
                Context.MainWindow.Opacity = i;
                await Task.Delay(1);
            }
            Context.MainWindow.Content = page;
            for (double i = 0.2; i <= 1; i += 0.2)
            {
                Context.MainWindow.Opacity = i;
                await Task.Delay(1);
            }
        }

        public static bool Equals<T>(T first, T second)
        {
            if (second is null)
            {
                return false;
            }

            string firstelement = JsonConvert.SerializeObject(first);
            string secondelement = JsonConvert.SerializeObject(second);

            if (firstelement.Length != secondelement.Length)
            {
                return false;
            }
            for (int i = 0; i < firstelement.Length; i++)
            {
                if (firstelement[i] != secondelement[i])
                {
                    return false;
                }
            }
            return true;
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
                }
                else
                {
                    Models.MessageBox.MakeMessage("Не удалось открыть ключ реестра.", MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                Models.MessageBox.MakeMessage("Произошла ошибка добавления Marlin в автозагрузку", MessageType.Error);
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
                    }
                    startupKey.Close();
                }
                else
                {
                    Models.MessageBox.MakeMessage("Не удалось открыть ключ реестра.", MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                Models.MessageBox.MakeMessage("Произошла ошибка добавления Marlin в автозагрузку", MessageType.Error);
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
                    if (Context.MessageBox.Answer.Length>0) 
                    {
                        Models.MessageBox.MakeMessage(error, MessageType.Error); 
                    }
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static void ChangePassword()
        {
            Context.Settings.Password = Context.Settings.NewPassword;
            ProgramData.SaveData();
        }

        public static string EncryptText(string text, string key = "")
        {
            if (key.Length == 0)
            {
                key = Math.PI.ToString("F14");
            }

            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte[] encryptedBytes = new byte[textBytes.Length];

            for (int i = 0; i < textBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(textBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptText(string encryptedText, string key = "")
        {
            if (key.Length == 0)
            {
                key = Math.PI.ToString("F14");
            }

            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte[] decryptedBytes = new byte[encryptedBytes.Length];

            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                decryptedBytes[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public static void StartActions(TriggersType triggerstype)
        {
            var checkpass = false;
            var failauthentication = false;
            foreach (var command in Context.ProgramData.Commands)
            {
                foreach (var trigger in command.Triggers)
                {
                    if (trigger.triggertype == triggerstype)
                    {
                        if (command.Checkpuss && failauthentication)
                        {
                            continue;
                        }
                        if (command.Checkpuss && !checkpass)
                        {
                            if (Program.Authentication("Для запуска действий подтвердите пароль"))
                            {
                                checkpass = true;
                            }
                            else
                            {
                                failauthentication = true;
                                continue;
                            }
                        }
                        command.ExecuteCommandAsync();
                    }
                }
            }
            foreach (var script in Context.ProgramData.Scripts)
            {
                foreach (var trigger in script.Triggers)
                {
                    if (trigger.triggertype == triggerstype)
                    {
                        if (script.Checkpuss && failauthentication)
                        {
                            continue;
                        }
                        if (script.Checkpuss && !checkpass)
                        {
                            if (Program.Authentication("Для запуска действий подтвердите пароль"))
                            {
                                checkpass = true;
                            }
                            else
                            {
                                return;
                            }
                        }
                        script.ExecuteScriptAsync();
                    }
                }
            }
        }
    }
}
