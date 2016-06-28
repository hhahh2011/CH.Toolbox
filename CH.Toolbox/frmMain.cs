using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CH.Toolbox
{
    public partial class frmMain : Form
    {
        #region 依赖
        private string _basePath = AppDomain.CurrentDomain.BaseDirectory;
        private string _lnkPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + Application.ProductName + ".lnk";
        public frmMain()
        {
            InitializeComponent();
            LoadData();
            RefreshAutoRun();
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

        #region 加载

        public void LoadData()
        {
            var categorys = CommandHelper.GetAllCommands(_basePath);
            foreach (var category in categorys)
            {
                var tp = new TabPage
                {
                    Text = category.DisplayName,
                    Dock = DockStyle.Fill,
                    Name = "tab" + category.DisplayName.GetHashCode()
                };

                var lv = new ListView
                {
                    Dock = DockStyle.Fill,
                    Name = tp.Name + "lv",
                    LargeImageList = myImageList,
                    Tag = category,
                    AllowDrop = true,
                    ContextMenuStrip = myRightContextMenuStrip
                };

                lv.DragEnter += (sender, e) =>
                {
                    e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Link : DragDropEffects.None;
                };

                lv.DragDrop += (sender, e) =>
                {
                    var c = (sender as ListView)?.Tag as CommandCategory;
                    var file = (Array)e.Data.GetData(DataFormats.FileDrop);
                    var cmd = CommandHelper.CreateCommand(file.GetValue(0)?.ToString(), c);
                    if (cmd != null)
                    {
                        lv.Items.Add(CreateListViewItem(cmd));
                    }
                };



                lv.MouseClick += (sender, e) =>
                {
                    if (e.Button != MouseButtons.Left)
                    {
                        return;
                    }

                    var command = ((ListView)sender).SelectedItems[0].Tag as Command;
                    if (command != null)
                    {
                        try
                        {
                            Process.Start(command.Cmd);
                            Hide();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                };

                foreach (var command in category.Commands)
                {
                    lv.Items.Add(CreateListViewItem(command));
                }
                tp.Controls.Add(lv);
                myTab.TabPages.Add(tp);
            }

        }

        public void RefreshAutoRun()
        {
            myContextMenuStrip.Items["btnAutoRun"].Visible = false;
            myContextMenuStrip.Items["btnUnAutoRun"].Visible = false;

            if (!File.Exists(_lnkPath))
            {
                myContextMenuStrip.Items["btnAutoRun"].Visible = true;
            }
            else
            {
                myContextMenuStrip.Items["btnUnAutoRun"].Visible = true;
            }
        }

        public ListViewItem CreateListViewItem(Command command)
        {
            if (!myImageList.Images.ContainsKey(command.Cmd))
            {
                var icon = IconHelper.GetFileIcon(command.Cmd);
                myImageList.Images.Add(command.Cmd, icon);
            }

            return new ListViewItem
            {
                ImageKey = command.Cmd,
                Text = command.Name,
                Tag = command,
            };
        }

        #endregion

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var stab = myTab.SelectedTab;
            var lv = stab.Controls.Find(stab.Name + "lv", false).FirstOrDefault() as ListView;
            if (lv?.SelectedItems.Count > 0)
            {
                var lvi = lv.SelectedItems[0];
                var command = lvi.Tag as Command;
                if (command != null)
                {
                    File.Delete(command.FullName);
                    lv.Items.Remove(lvi);
                }
            }
        }

        private void btnAutoRun_Click(object sender, EventArgs e)
        {
            var shell = new IWshRuntimeLibrary.WshShell();
            var shortCut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(_lnkPath);
            shortCut.TargetPath = Application.ExecutablePath;
            shortCut.WindowStyle = 1;
            shortCut.Description = Application.ProductName + Application.ProductVersion;
            shortCut.IconLocation = Application.ExecutablePath;
            shortCut.WorkingDirectory = Application.StartupPath;
            shortCut.Save();
            RefreshAutoRun();
        }

        private void btnUnAutoRun_Click(object sender, EventArgs e)
        {
            if (File.Exists(_lnkPath))
            {
                File.Delete(_lnkPath);
                RefreshAutoRun();
            }
        }
    }
}
