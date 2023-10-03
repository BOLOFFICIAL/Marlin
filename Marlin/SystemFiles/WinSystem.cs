using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Marlin.SystemFiles
{
    public class WinSystem
    {
        public static void RunCmd(string command)
        {
            Task.Run(() =>
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.FileName = "cmd.exe";
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.StandardOutputEncoding = Encoding.Default;

                process.StartInfo = startInfo;
                process.Start();

                process.StandardInput.WriteLine(command);
                process.StandardInput.Flush();
                process.StandardInput.Close();

                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
            });
        }

        public static void RunProcess(string app = null)
        {
            [DllImport("user32.dll")]
            static extern bool SetForegroundWindow(IntPtr hWnd);

            List<int> processIds = new List<int>();
            int currentProcessId = -1;

            if (app == null)
            {
                processIds = GetProcessIdsByName(Process.GetCurrentProcess().ProcessName);
                currentProcessId = Process.GetCurrentProcess().Id;
                processIds.Remove(currentProcessId);
            }
            else
            {
                Thread.Sleep(300);
                processIds = GetProcessIdsByName(System.IO.Path.GetFileNameWithoutExtension(app));
            }

            if (processIds.Count > 0)
            {
                try
                {
                    if (app == null)
                    {
                        SetForegroundWindow(Process.GetProcessById(processIds[0]).MainWindowHandle);
                        Process.GetProcessById(currentProcessId).Kill();
                    }
                    else
                    {
                        SetForegroundWindow(Process.GetProcessById(processIds[processIds.Count - 1]).MainWindowHandle);
                    }
                }
                catch (ArgumentException)
                {
                    Models.MessageBox.MakeMessage($"Возникла ошибка");
                }
            }
        }

        public static List<int> GetProcessIdsByName(string processName)
        {
            return Process.GetProcessesByName(processName).Select(process => process.Id).ToList();
        }
    }
}
