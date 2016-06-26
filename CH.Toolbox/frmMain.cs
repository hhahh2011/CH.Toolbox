using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CH.Toolbox
{
    public partial class frmMain : Form
    {
        #region 依赖
        private string _basePath = AppDomain.CurrentDomain.BaseDirectory;
        public frmMain()
        {
            InitializeComponent();
            var test = CommandHelper.GetAllCommands(_basePath);
        }
        #endregion

        #region 热键
        private const int WM_HOTKEY = 0x312; //窗口消息-热键
        private const int WM_CREATE = 0x1; //窗口消息-创建
        private const int WM_DESTROY = 0x2; //窗口消息-销毁
        private const int HOTKEY_ID = 0x9527; //热键ID
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_HOTKEY: //窗口消息-热键ID
                    switch (m.WParam.ToInt32())
                    {
                        case HOTKEY_ID: //热键ID
                            if (this.Visible)
                            {
                                this.Hide();
                            }
                            else
                            {
                                this.Show();
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case WM_CREATE:
                    HotKeyHelper.RegKey(Handle, HOTKEY_ID, HotKeyHelper.KeyModifiers.Alt, Keys.D1);
                    break;
                case WM_DESTROY: //窗口消息-销毁
                    HotKeyHelper.UnRegKey(Handle, HOTKEY_ID); //销毁热键
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 窗口
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            myNotifyIcon.Visible = false;
            this.Close();
            this.Dispose();
            Application.Exit();
        } 
        #endregion
    }
}
