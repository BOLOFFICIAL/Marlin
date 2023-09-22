using Marlin.SystemFiles.Types;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                    Models.MessageBox.MakeMessage(error, MessageType.Error);
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
        }

        public static string EncryptText(string text)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Math.PI.ToString("F14"));
                aesAlg.IV = new byte[16]; // Инициализационный вектор

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string DecryptText(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Math.PI.ToString("F14"));
                aesAlg.IV = new byte[16]; // Инициализационный вектор

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
