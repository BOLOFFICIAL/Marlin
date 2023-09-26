using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Marlin.SystemFiles
{
    public class BuiltinMethod
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, UIntPtr extraInfo);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);

        private const int KEYEVENTF_KEYDOWN = 0x0000;
        private const int KEYEVENTF_KEYUP = 0x0002;
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void MovingCursor(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void PressingKeys(string text)
        {
            Thread.Sleep(100);
            SendKeys.SendWait(text);
        }

        public static void PressingKeys(params int[] keyCodes)
        {
            foreach (byte keyCode in keyCodes)
            {
                if (keyCode == 1)
                {
                    PressingMouse(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP);
                }
                else if (keyCode == 2)
                {
                    PressingMouse(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP);
                }
                else
                {
                    keybd_event(keyCode, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
                }
            }

            Thread.Sleep(10);

            foreach (byte keyCode in keyCodes)
            {
                if (keyCode != 1 && keyCode != 2)
                {
                    keybd_event(keyCode, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                }
            }
        }

        private static void PressingMouse(uint mouseEventFlags)
        {
            mouse_event(mouseEventFlags, 0, 0, 0, 0);
        }
    }
}
