using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GlobalMouseHook;
using System.Runtime.InteropServices;

namespace MouseCoo
{
    public partial class Form1 : Form
    {

        #region  宏定义

        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int VK_CONTROL = 0x11;
        public const int VK_F5 = 0x74;
        public const int KEYEVENTF_KEYUP = 0x2;
        public const int VK_MENU = 0x12;
        public const int WM_SETTEXT = 0xC;
        public const int WM_CLEAR = 0x303;
        public const int BN_CLICKED = 0;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_CLOSE = 0x10;
        public const int WM_COMMAND = 0x111;
        public const int WM_SYSKEYDOWN = 0x104;
        public const int WM_SETFOCUS = 0x7;
        public const int WM_ACTIVATE = 0x6;
        public const int SC_MINIMIZE = 0xF020;
        public const int WM_SYSCOMMAND = 0x0112;

        private const int MOD_ALT = 0x1; //ALT 
        private const int MOD_CONTROL = 0x2; //CTRL 
        private const int MOD_SHIFT = 0x4; //SHIFT 
        private const int MOD_WIN = 0x8;//windows
        private const int WM_HOTKEY = 0x0312;

        #endregion

        #region key
        public const int VK_ESCAPE = 0x0027;//esc
        public const int VK_DOWN = 0x0040;//↓
        public const int VK_LEFT = 0x0037;//← 
        public const int VK_RIGHT = 0x0039;//→
        public const int VK_1 = 0x0031;//1
        public const int VK_NUMPAD1 = 0x0061;//1
        #endregion

        #region Declare Windows API
        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄
        public static extern int WindowFromPoint(
            int xPoint,
            int yPoint
        );

        [DllImport("user32.dll", EntryPoint = "SetFocus")]
        public static extern int SetFocus(
            int hWnd
        );

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            int hWnd, // handle to destination window 
            int Msg, // message 
            int wParam, // first message parameter 
            int lParam // second message parameter 
        );

        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern int GetForegroundWindow();

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern int SetForegroundWindow(int hwnd);

        [DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
        public static extern int GetActiveWindow();

        [DllImport("user32.dll", EntryPoint = "SetActiveWindow")]
        public static extern int SetActiveWindow(int hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SwitchToThisWindow(int hWnd, bool fAltTab);

        /// <summary>
        /// 0    关闭窗口
        /// 1    正常大小显示窗口
        /// 2    最小化窗口
        /// 3    最大化窗口
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(int hwnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        public static extern int GetWindowThreadProcessId(int hWnd, ref int lpdwProcessId);

        [DllImport("user32.dll")]//取设备场景
        private static extern IntPtr GetDC(IntPtr hwnd);//返回设备场景句柄

        [DllImport("gdi32.dll")]//取指定点颜色
        private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        public static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);//要定义热键的窗口的句柄,热键ID不能与其它ID重复,组合键(没有时为0),热键值)

        [DllImport("user32.dll")]
        public static extern int UnregisterHotKey(IntPtr hwnd, int id);

        [DllImport("user32.dll")]
        public static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        #endregion

        MouseHook mouse = new MouseHook();

        public Form1()
        {
            InitializeComponent();
            RegKey(this.Handle, 111, MOD_ALT, VK_1);
            RegKey(this.Handle, 222, MOD_ALT, VK_NUMPAD1);
        }


        //捕获键盘事件
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HOTKEY:    //这个是window消息定义的   注册的热键消息
                    if (m.WParam.ToString().Equals("111"))  //这是我们注册的那個热键
                    {
                        toolStripMenuItem2_Click(null, null);
                    }
                    else if (m.WParam.ToString().Equals("222"))
                    {
                        toolStripMenuItem2_Click(null, null);
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        /// <summary> 
        /// 注册热键 
        /// </summary> 
        /// <param name="hwnd">窗口句柄</param> 
        /// <param name="hotKey_id">热键ID</param> 
        /// <param name="fsModifiers">组合键(没有为0)</param> 
        /// <param name="vk">热键</param> 
        public bool RegKey(IntPtr hwnd, int hotKey_id, int fsModifiers, int vk)
        {
            bool result;
            if (RegisterHotKey(hwnd, hotKey_id, fsModifiers, vk) == 0)//注册失败返回0
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        void mouse_OnMouseActivity(object sender, MouseEventArgs e)
        {
            //string str = "X: " + e.X + " Y: " + e.Y;
            //this.Text = str;
            //int pointWin = WindowFromPoint(e.X, e.Y);

            //int activeWin = GetActiveWindow();
            //int curForeGroundWin = GetForegroundWindow();
            //int pointThreadID = 0;
            //pointThreadID = GetWindowThreadProcessId(pointWin, ref pointThreadID);
            //int currentThreadID = 0;
            //currentThreadID = GetWindowThreadProcessId(curForeGroundWin, ref currentThreadID);

            //if (pointThreadID == currentThreadID)
            //{
            //    if (curForeGroundWin != pointWin)
            //    {
            //        //SwitchToThisWindow(pointWin, true).ToString();
            //        //SetForegroundWindow(pointWin).ToString();
            //        //SetActiveWindow(pointWin);
            //        //SetFocus(pointWin);
            //        //abel2.Text = ShowWindow(pointWindow, 1).ToString();
            //        //SendMessage(pointWin, WM_SYSCOMMAND, SC_MINIMIZE, 0);
            //    }

            //}

            IntPtr hdc = GetDC(IntPtr.Zero);
            uint c = GetPixel(hdc, e.X, e.Y);
            ReleaseDC(IntPtr.Zero, hdc);

            textBox1.Text = Convert.ToString(c, 16);

            uint r = (c & 0xFF);//转换R
            uint g = (c & 0xFF00) / 256;//转换G
            uint b = (c & 0xFF0000) / 65536;//转换B
            pictureBox1.BackColor = Color.FromArgb((int)r, (int)g, (int)b);

            textBox2.Text = r.ToString();
            textBox3.Text = g.ToString();
            textBox4.Text = b.ToString();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                toolStripMenuItem1.Text = "Show";
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                this.Visible = true;
                toolStripMenuItem1.Text = "Hide";
            }
            else
            {
                this.Visible = false;
                toolStripMenuItem1.Text = "Show";
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            UnregisterHotKey(IntPtr.Zero, 111);//释放热键
            Application.Exit();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (toolStripMenuItem2.Text == "Start")
            {
                this.Text = "Started...";
                notifyIcon1.BalloonTipText = "Started...";
                notifyIcon1.ShowBalloonTip(1000);
                mouse.OnMouseActivity += new MouseEventHandler(mouse_OnMouseActivity);
                mouse.Start();
                toolStripMenuItem2.Text = "Stop";
            }
            else
            {
                notifyIcon1.BalloonTipText = "Stopped...";
                notifyIcon1.ShowBalloonTip(1000);
                this.Text = "Stopped...";
                mouse.Stop();
                toolStripMenuItem2.Text = "Start";
            }

        }
    }
}