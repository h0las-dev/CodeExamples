namespace MouseTrackerTest
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public static class WinAPI
    {
        private const int WHKEYBOARDLL = 13;

        private const int WMKEYDOWN = 0x0100;

        private const int WHMOUSELL = 14;

        private static double resultWay = 0;

        private static POINT oldPoint;

        private static POINT newPoint;

        private static bool mouseHookIsOn = false;

        private static LowLevelMouseProc procMouse = HookCallback;

        private static IntPtr hookMouseID = IntPtr.Zero;

        private static LowLevelKeyboardProc procKeyboard = HookKeyboardCallback;

        private static IntPtr hookKeyboardID = IntPtr.Zero;

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1, 
            ShowMinimized = 2,  
            Maximize = 3,     
            ShowMaximized = 3,
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref MSG lpmsg);

        [DllImport("user32.dll")]
        public static extern sbyte GetMessage(out MSG lpmsg, IntPtr hwnd, uint wmsgFilterMin, uint wmsgFilterMax);

        public static void Main(string[] args)
        {
            MainWinApi(System.Diagnostics.Process.GetCurrentProcess().Handle, IntPtr.Zero, string.Empty, (int)ShowWindowCommands.Normal);
        }

        public static bool MainWinApi(IntPtr hinstance, IntPtr hcrevInstance, string lpcmdLine, int ncmdShow)
        {
            Console.WriteLine("Press ENTER for set mouse hook...");
            Console.WriteLine("Press ESC for Exit...");
            hookKeyboardID = SetHook(procKeyboard);

            MSG msg;

            sbyte hasMessage;

            while ((hasMessage = WinAPI.GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0 && hasMessage != -1)
            {
                WinAPI.TranslateMessage(ref msg);
                WinAPI.DispatchMessage(ref msg);
            }

            return msg.Wparam == UIntPtr.Zero;
        }

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WHMOUSELL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WHKEYBOARDLL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookKeyboardCallback(int ncode, IntPtr wparam, IntPtr lparam)
        {
            if (ncode >= 0 && wparam == (IntPtr)WMKEYDOWN)
            {
                int vkcode = Marshal.ReadInt32(lparam);
                var pressedKey = (Keys)vkcode;
                if (pressedKey == Keys.Enter)
                {
                    if (mouseHookIsOn)
                    {
                        Console.WriteLine("Press ENTER for set mouse hook...");
                        mouseHookIsOn = false;
                        resultWay = 0;
                        UnhookWindowsHookEx(hookMouseID);
                    }
                    else
                    {
                        Console.WriteLine("Press ENTER for unset mouse hook...");
                        mouseHookIsOn = true;
                        resultWay = 0;
                        hookMouseID = SetHook(procMouse);
                    }
                }
                else if (pressedKey == Keys.Escape)
                {
                    UnhookWindowsHookEx(hookMouseID);
                    UnhookWindowsHookEx(hookKeyboardID);

                    Console.WriteLine("Bye!");
                    Environment.Exit(0);
                }
            }

            return CallNextHookEx(hookKeyboardID, ncode, wparam, lparam);
        }

        private static IntPtr HookCallback(int ncode, IntPtr wparam, IntPtr lparam)
        {
            if (ncode >= 0 && MouseMessages.WM_MOUSEMOVE == (MouseMessages)wparam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lparam, typeof(MSLLHOOKSTRUCT));

                if (resultWay == 0)
                {
                    oldPoint.X = 0;
                    oldPoint.Y = 0;

                    newPoint.X = hookStruct.Pt.X;
                    newPoint.Y = hookStruct.Pt.Y;

                    resultWay = 1;
                }
                else
                {
                    oldPoint.X = newPoint.X;
                    oldPoint.Y = newPoint.Y;

                    newPoint.X = hookStruct.Pt.X;
                    newPoint.Y = hookStruct.Pt.Y;

                    resultWay += Math.Sqrt(((newPoint.X - oldPoint.X) * (newPoint.X - oldPoint.X)) + ((newPoint.Y - oldPoint.Y) * (newPoint.Y - oldPoint.Y)));
                }

                Console.WriteLine("resultWay = {0}", resultWay);
            }

            return CallNextHookEx(hookMouseID, ncode, wparam, lparam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hmodule, uint dwordThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hmod, uint dwordThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int ncode, IntPtr wparam, IntPtr lparam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpmoduleName);

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr Hwnd;
            public uint Message;
            public UIntPtr Wparam;
            public UIntPtr Lparam;
            public uint Time;
            public POINT Pt;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT Pt;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr DwordExtraInfo;
        }
    }  
}