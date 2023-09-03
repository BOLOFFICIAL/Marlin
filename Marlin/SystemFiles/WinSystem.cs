using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        public static void CheckRunProcess()
        {
            [DllImport("user32.dll")]
            static extern bool SetForegroundWindow(IntPtr hWnd);

            string appName = Process.GetCurrentProcess().ProcessName;
            int currentProcessId = Process.GetCurrentProcess().Id;

            List<int> processIds = GetProcessIdsByName(appName);
            processIds.Remove(currentProcessId);

            if (processIds.Count > 0)
            {
                int targetProcessId = processIds[0];

                try
                {
                    Process targetProcess = Process.GetProcessById(targetProcessId);
                    SetForegroundWindow(targetProcess.MainWindowHandle);
                    Process currentProcess = Process.GetCurrentProcess();
                    currentProcess.Kill();
                }
                catch (ArgumentException)
                {
                    Models.MessageBox.MakeMessage($"Процесс с ID {targetProcessId} не найден.");
                }
            }

            static List<int> GetProcessIdsByName(string processName)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                return processes.Select(process => process.Id).ToList();
            }
        }
    }
}
