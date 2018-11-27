using System;
using System.Windows;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace OpenOffice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class Odswiezacz : Window
    {
        private DispatcherTimer timer;
        public Odswiezacz()
        {
            InitializeComponent();
            timer = new DispatcherTimer(); //initialize new timer
            timer.Tick += new EventHandler(akcja); //activate event to display last time of refreshing
            timer.Interval = new TimeSpan(0, 0, 0, 30); //time intervals how often to refresh
        }
        [DllImport("user32.dll")]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize); //sending input to the system
        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public SendInputEventType type; //input type
            public MouseKeybdhardwareInputUnion mkhi; //input flag
        }
        [StructLayout(LayoutKind.Explicit)]
        struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            public MouseInputData mi; //input data to be sent
        }
        //variables for mouse actions
        struct MouseInputData
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        //potential mouse event flags to be chosen for simulating mouse movement
        [Flags]
        enum MouseEventFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }
        //types of simulated inputs
        enum SendInputEventType : int
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }
        // START BUTTON method
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            INPUT input = new INPUT();
            input.type = SendInputEventType.InputMouse;
            input.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref input, Marshal.SizeOf(new INPUT()));
            timer.Start();
            DateTime a = DateTime.Now;
            txtNastepny.Text = (a.ToString());
        }
        //STOP BUTTON method
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            timer.Stop(); //stops the timer
            Close(); //closes the app
        }
        //EVENTS TO BE TRIGERRED every cycle
        void akcja(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                DateTime czas = DateTime.Now;
                txtNastepny.Text = (czas.ToString());
            }));
        }
    }
}
