using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CH.Toolbox
{
    public partial class frmMain : Form
    {
        #region 依赖

        private string _commandDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Commands");

        private string _lnkPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" +
                                  Application.ProductName + ".lnk";

        private int _countMin = 25;
        private int _countDown;

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

        #endregion

        #region 消息

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
            var categorys = CommandHelper.GetAllCommands(_commandDir);
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
                    var file = (Array) e.Data.GetData(DataFormats.FileDrop);
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

                    var command = ((ListView) sender).SelectedItems[0].Tag as Command;
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
            if (myTab.TabPages.Count > 1)
            {
                myTab.SelectedIndex = 1;
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

        #region 其它

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
            if (File.Exists(_lnkPath))
            {
                File.Delete(_lnkPath);
            }
            var shell = new IWshRuntimeLibrary.WshShell();
            var shortCut = (IWshRuntimeLibrary.IWshShortcut) shell.CreateShortcut(_lnkPath);
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

        private void btnOpenDir_Click(object sender, EventArgs e)
        {
            Process.Start("Explorer.exe", _commandDir);
            Hide();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void btnOpenInExplorer_Click(object sender, EventArgs e)
        {
            var stab = myTab.SelectedTab;
            var lv = stab.Controls.Find(stab.Name + "lv", false).FirstOrDefault() as ListView;
            if (lv?.SelectedItems.Count > 0)
            {
                var lvi = lv.SelectedItems[0];
                var command = lvi.Tag as Command;
                if (command != null)
                {
                    Process.Start("Explorer.exe", Path.GetDirectoryName(command.Cmd));
                    Hide();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                MessageBox.Show("请输入关键字!");
                return;
            }

            Task.Factory.StartNew(() =>
            {
                var html = HttpLibSyncRequest.Get(
                    $"http://fanyi.youdao.com/openapi.do?keyfrom=Codelf&key=2023743559&type=data&doctype=json&version=1.1&q={txtKeyword.Text}");

                if (!string.IsNullOrEmpty(html))
                {
                    var tr = html.ToObject<TranslationResult>();
                    BeginInvoke(new MethodInvoker(() =>
                    {

                        lbSuggestions.Items.Clear();

                    }));

                    if (tr.Web == null)
                    {
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            lbSuggestions.Items.Insert(0, "无结果");
                        }));
                        return;
                    }


                    foreach (var t in tr.Web)
                    {
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            foreach (var v in t.Value)
                            {
                                lbSuggestions.Items.Insert(0, v);
                            }
                        }));
                    }
                    var result = tr.Web.FirstOrDefault()?.Value?.FirstOrDefault();
                    if (!string.IsNullOrEmpty(result))
                    {
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            SetClipboardTextData(result);
                        }));
                    }
                }
            });

        }

        private void lbSuggestions_DoubleClick(object sender, EventArgs e)
        {
            var sel = lbSuggestions.SelectedItem;
            SetClipboardTextData(sel.ToString());
            Hide();
        }

        private void lbSuggestions_Click(object sender, EventArgs e)
        {
            var sel = lbSuggestions.SelectedItem;
            SetClipboardTextData(sel.ToString());
        }

        private void SetClipboardTextData(string text)
        {
            try
            {
                Clipboard.SetData(DataFormats.Text, text);
            }
            catch
            {
            }
        }

        private string GetClipboardTextData()
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    return Clipboard.GetData(DataFormats.Text).ToString();
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        private void btnCountDownStart_Click(object sender, EventArgs e)
        {
            _countDown = _countMin*60;
            btnCountDownStart.Enabled = false;
            tCountDown.Start();
        }

        private void tCountDown_Tick(object sender, EventArgs e)
        {
            _countDown--;
            BeginInvoke(new MethodInvoker(delegate
            {
                var m = _countDown/60;
                var s = _countDown%60;
                lbCountDown.Text = (m < 10 ? "0" + m : m.ToString()) + @":" + (s < 10 ? "0" + s : s.ToString());
            }));

            if (_countDown == 0)
            {
                btnCountDownStart.Enabled = true;
                tCountDown.Stop();
                BeginInvoke(new MethodInvoker(delegate
                {
                    lsTimes.Items.Insert(0,
                        DateTime.Now.AddMinutes(-_countMin).ToString("mm:ss") + "-" + DateTime.Now.ToString("mm:ss"));
                }));
                MessageBox.Show(@"完成一个番茄时间");
            }
        }

        private void btnCountDownClear_Click(object sender, EventArgs e)
        {
            lsTimes.Items.Clear();
        }

        private void myNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        #endregion

        private void btnClearClipboard_Click(object sender, EventArgs e)
        {
            lbClipboards.Items.Clear();
        }

        private void lbClipboards_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var sel = lbClipboards.SelectedItem;
            SetClipboardTextData(sel.ToString());
            Hide();
        }

        private void lbClipboards_MouseClick(object sender, MouseEventArgs e)
        {
            var sel = lbClipboards.SelectedItem;
            SetClipboardTextData(sel.ToString());
        }

        private void btnCopyClipboard_Click(object sender, EventArgs e)
        {
            var text = GetClipboardTextData()?.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                lbClipboards.Items.Insert(0, text);
            }
        }

        private void lbClipboards_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var sel = lbClipboards.SelectedItem;
                if (sel != null)
                {
                    lbClipboards.Items.Remove(sel);
                }
            }
        }
    }
}

