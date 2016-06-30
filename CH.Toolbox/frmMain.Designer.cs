namespace CH.Toolbox
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.myContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnShow = new System.Windows.Forms.ToolStripMenuItem();
            this.btnHide = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenDir = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAutoRun = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUnAutoRun = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.myNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.myTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbSuggestions = new System.Windows.Forms.ListBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.myImageList = new System.Windows.Forms.ImageList(this.components);
            this.myRightContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.myContextMenuStrip.SuspendLayout();
            this.myTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.myRightContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // myContextMenuStrip
            // 
            this.myContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnShow,
            this.btnHide,
            this.btnRestart,
            this.btnOpenDir,
            this.btnAutoRun,
            this.btnUnAutoRun,
            this.btnExit});
            this.myContextMenuStrip.Name = "myContextMenuStrip";
            this.myContextMenuStrip.Size = new System.Drawing.Size(149, 158);
            // 
            // btnShow
            // 
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(148, 22);
            this.btnShow.Text = "显示";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnHide
            // 
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(148, 22);
            this.btnHide.Text = "隐藏";
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(148, 22);
            this.btnRestart.Text = "重启";
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnOpenDir
            // 
            this.btnOpenDir.Name = "btnOpenDir";
            this.btnOpenDir.Size = new System.Drawing.Size(148, 22);
            this.btnOpenDir.Text = "打开目录";
            this.btnOpenDir.Click += new System.EventHandler(this.btnOpenDir_Click);
            // 
            // btnAutoRun
            // 
            this.btnAutoRun.Name = "btnAutoRun";
            this.btnAutoRun.Size = new System.Drawing.Size(148, 22);
            this.btnAutoRun.Text = "开机启动";
            this.btnAutoRun.Click += new System.EventHandler(this.btnAutoRun_Click);
            // 
            // btnUnAutoRun
            // 
            this.btnUnAutoRun.Name = "btnUnAutoRun";
            this.btnUnAutoRun.Size = new System.Drawing.Size(148, 22);
            this.btnUnAutoRun.Text = "关闭开机启动";
            this.btnUnAutoRun.Click += new System.EventHandler(this.btnUnAutoRun_Click);
            // 
            // btnExit
            // 
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(148, 22);
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // myNotifyIcon
            // 
            this.myNotifyIcon.ContextMenuStrip = this.myContextMenuStrip;
            this.myNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("myNotifyIcon.Icon")));
            this.myNotifyIcon.Text = "CH.Toolbox";
            this.myNotifyIcon.Visible = true;
            // 
            // myTab
            // 
            this.myTab.Controls.Add(this.tabPage1);
            this.myTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myTab.ItemSize = new System.Drawing.Size(60, 35);
            this.myTab.Location = new System.Drawing.Point(0, 0);
            this.myTab.Name = "myTab";
            this.myTab.SelectedIndex = 0;
            this.myTab.Size = new System.Drawing.Size(774, 355);
            this.myTab.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbSuggestions);
            this.tabPage1.Controls.Add(this.btnSearch);
            this.tabPage1.Controls.Add(this.txtKeyword);
            this.tabPage1.Location = new System.Drawing.Point(4, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(766, 312);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "变量";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbSuggestions
            // 
            this.lbSuggestions.FormattingEnabled = true;
            this.lbSuggestions.ItemHeight = 12;
            this.lbSuggestions.Location = new System.Drawing.Point(4, 31);
            this.lbSuggestions.Name = "lbSuggestions";
            this.lbSuggestions.Size = new System.Drawing.Size(322, 268);
            this.lbSuggestions.TabIndex = 3;
            this.lbSuggestions.Click += new System.EventHandler(this.lbSuggestions_Click);
            this.lbSuggestions.DoubleClick += new System.EventHandler(this.lbSuggestions_DoubleClick);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(251, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(4, 6);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(241, 21);
            this.txtKeyword.TabIndex = 1;
            // 
            // myImageList
            // 
            this.myImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.myImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.myImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // myRightContextMenuStrip
            // 
            this.myRightContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRemove,
            this.btnOpenInExplorer});
            this.myRightContextMenuStrip.Name = "myRightContextMenuStrip";
            this.myRightContextMenuStrip.Size = new System.Drawing.Size(149, 48);
            // 
            // btnRemove
            // 
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(148, 22);
            this.btnRemove.Text = "删除";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnOpenInExplorer
            // 
            this.btnOpenInExplorer.Name = "btnOpenInExplorer";
            this.btnOpenInExplorer.Size = new System.Drawing.Size(148, 22);
            this.btnOpenInExplorer.Text = "打开文件目录";
            this.btnOpenInExplorer.Click += new System.EventHandler(this.btnOpenInExplorer_Click);
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 355);
            this.Controls.Add(this.myTab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CH.Toolbox";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.myContextMenuStrip.ResumeLayout(false);
            this.myTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.myRightContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip myContextMenuStrip;
        private System.Windows.Forms.NotifyIcon myNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem btnShow;
        private System.Windows.Forms.ToolStripMenuItem btnHide;
        private System.Windows.Forms.ToolStripMenuItem btnExit;
        private System.Windows.Forms.TabControl myTab;
        private System.Windows.Forms.ImageList myImageList;
        private System.Windows.Forms.ContextMenuStrip myRightContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem btnRemove;
        private System.Windows.Forms.ToolStripMenuItem btnAutoRun;
        private System.Windows.Forms.ToolStripMenuItem btnUnAutoRun;
        private System.Windows.Forms.ToolStripMenuItem btnRestart;
        private System.Windows.Forms.ToolStripMenuItem btnOpenDir;
        private System.Windows.Forms.ToolStripMenuItem btnOpenInExplorer;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.ListBox lbSuggestions;
    }
}

