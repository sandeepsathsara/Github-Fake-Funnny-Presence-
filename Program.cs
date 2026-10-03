using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using System.Threading;

[assembly: AssemblyTitle("Porn Hub")]
[assembly: AssemblyProduct("Porn Hub")]
[assembly: AssemblyDescription("Porn Hub")]
[assembly: AssemblyCompany("S4KA")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyVersion("1.0.0.0")]

namespace PornHub
{
    static class Program
    {
        [DllImport("user32.dll", EntryPoint = "RegisterClassExW", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern ushort RegisterClassEx([In] ref WNDCLASSEX lpwcx);

        [DllImport("user32.dll", EntryPoint = "CreateWindowExW", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr CreateWindowEx(
            uint dwExStyle,
            string lpClassName,
            string lpWindowName,
            uint dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        static extern IntPtr DispatchMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll", EntryPoint = "DefWindowProcW", CharSet = CharSet.Unicode)]
        static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr BeginPaint(IntPtr hWnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", EntryPoint = "DrawTextW", CharSet = CharSet.Unicode)]
        static extern int DrawText(IntPtr hDC, string lpchText, int nCount, ref RECT lpRect, uint uFormat);

        [DllImport("user32.dll")]
        static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("user32.dll", EntryPoint = "LoadIconW", CharSet = CharSet.Unicode)]
        static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport("gdi32.dll", EntryPoint = "CreateFontW", CharSet = CharSet.Unicode)]
        static extern IntPtr CreateFont(
            int nHeight, int nWidth, int nEscapement, int nOrientation,
            int fnWeight, uint fdwItalic, uint fdwUnderline, uint fdwStrikeOut,
            uint fdwCharSet, uint fdwOutputPrecision, uint fdwClipPrecision,
            uint fdwQuality, uint fdwPitchAndFamily, string lpszFace);

        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        static extern uint SetTextColor(IntPtr hdc, uint crColor);

        [DllImport("gdi32.dll")]
        static extern int SetBkMode(IntPtr hdc, int iBkMode);

        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandleW", CharSet = CharSet.Unicode)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);

        [DllImport("kernel32.dll")]
        static extern bool SetProcessWorkingSetSize(IntPtr proc, IntPtr min, IntPtr max);

        delegate IntPtr WndProcDelegate(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct WNDCLASSEX
        {
            public uint cbSize;
            public uint style;
            public WndProcDelegate lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] rgbReserved;
        }

        const int SW_SHNORMAL = 1;
        const uint WM_PAINT = 0x000F;
        const uint WM_DESTROY = 0x0002;
        const uint WM_SIZE = 0x0005;
        const int TRANSPARENT = 1;
        const uint DT_CENTER = 0x00000001;
        const uint DT_VCENTER = 0x00000004;
        const uint DT_SINGLELINE = 0x00000020;
        const int IDC_ARROW = 32512;
        const int SM_CXSCREEN = 0;
        const int SM_CYSCREEN = 1;

        static WndProcDelegate wndProc;
        static IntPtr bgBrush;
        static IntPtr fontTitle;
        static IntPtr fontSub;
        static IntPtr fontNote;

        public static void TrimMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                EmptyWorkingSet(Process.GetCurrentProcess().Handle);
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, new IntPtr(-1), new IntPtr(-1));
            }
            catch { }
        }

        static IntPtr CustomWndProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            switch (uMsg)
            {
                case WM_PAINT:
                    PAINTSTRUCT ps;
                    IntPtr hdc = BeginPaint(hWnd, out ps);
                    RECT rect;
                    GetClientRect(hWnd, out rect);

                    SetBkMode(hdc, TRANSPARENT);

                    // Header Title
                    IntPtr oldFont = SelectObject(hdc, fontTitle);
                    SetTextColor(hdc, 0x00EB823B);
                    RECT titleRect = rect;
                    titleRect.Top = rect.Top + 25;
                    titleRect.Bottom = titleRect.Top + 50;
                    DrawText(hdc, "Porn Hub", -1, ref titleRect, DT_CENTER | DT_VCENTER | DT_SINGLELINE);

                    // Subtitle / Status
                    SelectObject(hdc, fontSub);
                    SetTextColor(hdc, 0x00E4E4E7);
                    RECT statusRect = rect;
                    statusRect.Top = titleRect.Bottom + 5;
                    statusRect.Bottom = statusRect.Top + 30;
                    DrawText(hdc, "Status: Active • Discord Rich Presence Connected", -1, ref statusRect, DT_CENTER | DT_VCENTER | DT_SINGLELINE);

                    // Note on performance
                    SelectObject(hdc, fontNote);
                    SetTextColor(hdc, 0x00808080);
                    RECT noteRect = rect;
                    noteRect.Top = statusRect.Bottom + 5;
                    noteRect.Bottom = noteRect.Top + 25;
                    DrawText(hdc, "0.0% CPU  |  Ultra-low Memory  |  Dev by S4KA", -1, ref noteRect, DT_CENTER | DT_VCENTER | DT_SINGLELINE);

                    SelectObject(hdc, oldFont);
                    EndPaint(hWnd, ref ps);
                    return IntPtr.Zero;

                case WM_SIZE:
                    TrimMemory();
                    return DefWindowProc(hWnd, uMsg, wParam, lParam);

                case WM_DESTROY:
                    PostQuitMessage(0);
                    return IntPtr.Zero;
            }

            return DefWindowProc(hWnd, uMsg, wParam, lParam);
        }

        [STAThread]
        static void Main()
        {
            // Start Discord Rich Presence in background
            DiscordRpc.Start();

            string className = "PornHubWindowClass";
            IntPtr hInstance = GetModuleHandle(null);

            // Dark background (RGB: 24, 24, 27 -> BGR 0x001B1818)
            bgBrush = CreateSolidBrush(0x001B1818);

            // High quality Segoe UI fonts
            fontTitle = CreateFont(-22, 0, 0, 0, 700, 0, 0, 0, 1, 0, 0, 5, 0, "Segoe UI");
            fontSub = CreateFont(-14, 0, 0, 0, 600, 0, 0, 0, 1, 0, 0, 5, 0, "Segoe UI");
            fontNote = CreateFont(-12, 0, 0, 0, 400, 0, 0, 0, 1, 0, 0, 5, 0, "Segoe UI");

            wndProc = new WndProcDelegate(CustomWndProc);

            IntPtr hIcon = LoadIcon(hInstance, new IntPtr(1));

            WNDCLASSEX wc = new WNDCLASSEX();
            wc.cbSize = (uint)Marshal.SizeOf(typeof(WNDCLASSEX));
            wc.style = 3;
            wc.lpfnWndProc = wndProc;
            wc.cbClsExtra = 0;
            wc.cbWndExtra = 0;
            wc.hInstance = hInstance;
            wc.hIcon = hIcon;
            wc.hCursor = LoadCursor(IntPtr.Zero, IDC_ARROW);
            wc.hbrBackground = bgBrush;
            wc.lpszMenuName = null;
            wc.lpszClassName = className;
            wc.hIconSm = hIcon;

            RegisterClassEx(ref wc);

            int width = 460;
            int height = 180;
            int screenWidth = GetSystemMetrics(SM_CXSCREEN);
            int screenHeight = GetSystemMetrics(SM_CYSCREEN);
            int x = (screenWidth - width) / 2;
            int y = (screenHeight - height) / 2;

            uint style = 0x00CA0000;

            IntPtr hWnd = CreateWindowEx(
                0,
                className,
                "Porn Hub",
                style,
                x,
                y,
                width,
                height,
                IntPtr.Zero,
                IntPtr.Zero,
                hInstance,
                IntPtr.Zero);

            ShowWindow(hWnd, SW_SHNORMAL);
            UpdateWindow(hWnd);

            TrimMemory();

            MSG msg;
            while (GetMessage(out msg, IntPtr.Zero, 0, 0) > 0)
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }

            DiscordRpc.Stop();

            if (bgBrush != IntPtr.Zero) DeleteObject(bgBrush);
            if (fontTitle != IntPtr.Zero) DeleteObject(fontTitle);
            if (fontSub != IntPtr.Zero) DeleteObject(fontSub);
            if (fontNote != IntPtr.Zero) DeleteObject(fontNote);
        }
    }

    static class DiscordRpc
    {
        private static NamedPipeClientStream pipe;
        private static Thread thread;
        private static volatile bool running = false;
        private const string CLIENT_ID = "1555522311011049534";

        public static void Start()
        {
            if (running) return;
            running = true;
            thread = new Thread(Worker)
            {
                IsBackground = true,
                Name = "DiscordRpcThread"
            };
            thread.Start();
        }

        public static void Stop()
        {
            running = false;
            try
            {
                if (pipe != null)
                {
                    pipe.Close();
                    pipe.Dispose();
                    pipe = null;
                }
            }
            catch { }
        }

        private static void Worker()
        {
            while (running)
            {
                try
                {
                    pipe = null;
                    for (int i = 0; i < 10 && running; i++)
                    {
                        try
                        {
                            var candidate = new NamedPipeClientStream(".", "discord-ipc-" + i, PipeDirection.InOut);
                            candidate.Connect(500);
                            pipe = candidate;
                            break;
                        }
                        catch
                        {
                        }
                    }

                    if (pipe == null || !pipe.IsConnected)
                    {
                        Thread.Sleep(4000);
                        continue;
                    }

                    // 1. Handshake (Opcode 0)
                    string handshake = "{\"v\":1,\"client_id\":\"" + CLIENT_ID + "\"}";
                    SendPacket(0, handshake);
                    ReadPacket(); // Wait for READY event

                    // 2. Send SET_ACTIVITY (Opcode 1)
                    long startTime = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    int pid = Process.GetCurrentProcess().Id;

                    string activity = "{\"cmd\":\"SET_ACTIVITY\",\"args\":{\"pid\":" + pid + ",\"activity\":{" +
                        "\"details\":\"Dev by S4KA\"," +
                        "\"state\":\"Active\"," +
                        "\"timestamps\":{\"start\":" + startTime + "}," +
                        "\"assets\":{" +
                        "\"large_image\":\"logo\"," +
                        "\"large_text\":\"Porn Hub\"" +
                        "}" +
                        "}},\"nonce\":\"1\"}";

                    SendPacket(1, activity);
                    ReadPacket(); // Response

                    Program.TrimMemory();

                    byte[] buffer = new byte[1024];
                    while (running && pipe.IsConnected)
                    {
                        int read = pipe.Read(buffer, 0, buffer.Length);
                        if (read == 0) break;
                    }
                }
                catch
                {
                    Thread.Sleep(4000);
                }
                finally
                {
                    try
                    {
                        if (pipe != null)
                        {
                            pipe.Close();
                            pipe.Dispose();
                            pipe = null;
                        }
                    }
                    catch { }
                }
            }
        }

        private static void SendPacket(int opcode, string json)
        {
            byte[] payload = Encoding.UTF8.GetBytes(json);
            BinaryWriter writer = new BinaryWriter(pipe);
            writer.Write(opcode);
            writer.Write(payload.Length);
            writer.Write(payload);
            writer.Flush();
        }

        private static void ReadPacket()
        {
            BinaryReader reader = new BinaryReader(pipe);
            int opcode = reader.ReadInt32();
            int length = reader.ReadInt32();
            if (length > 0)
            {
                reader.ReadBytes(length);
            }
        }
    }
}
