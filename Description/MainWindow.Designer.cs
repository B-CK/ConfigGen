﻿namespace Description
{
    partial class MainWindow
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
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin4 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin4 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient10 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient22 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin4 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient23 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient11 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient24 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient25 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient26 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient12 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient27 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient28 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._createItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._createModuleItem = new System.Windows.Forms.ToolStripMenuItem();
            this._openModuleItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveTypeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveModuleItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveAnotherModuleItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._svnCommitItem = new System.Windows.Forms.ToolStripMenuItem();
            this._svnUpdateItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this._checkMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._openFindNamespaceItem = new System.Windows.Forms.ToolStripMenuItem();
            this._openConsoleItem = new System.Windows.Forms.ToolStripMenuItem();
            this._openTypeEditorItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._docItem = new System.Windows.Forms.ToolStripMenuItem();
            this._dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.BackColor = System.Drawing.Color.DarkGray;
            this.MainMenu.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.EditToolStripMenuItem,
            this.ViewToolStripMenuItem,
            this.HelpToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1108, 28);
            this.MainMenu.TabIndex = 7;
            this.MainMenu.Text = "MainMenu";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._createItem,
            this.SaveTypeMenuItem,
            this.toolStripSeparator1,
            this._createModuleItem,
            this._openModuleItem,
            this._saveModuleItem,
            this._saveAnotherModuleItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.FileToolStripMenuItem.Text = "文件";
            // 
            // _createItem
            // 
            this._createItem.Name = "_createItem";
            this._createItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this._createItem.Size = new System.Drawing.Size(231, 26);
            this._createItem.Text = "新建";
            this._createItem.Click += new System.EventHandler(this.CreateItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(228, 6);
            // 
            // _createModuleItem
            // 
            this._createModuleItem.Name = "_createModuleItem";
            this._createModuleItem.Size = new System.Drawing.Size(231, 26);
            this._createModuleItem.Text = "创建模块";
            this._createModuleItem.Click += new System.EventHandler(this.CreateModuleItem_Click);
            // 
            // _openModuleItem
            // 
            this._openModuleItem.Name = "_openModuleItem";
            this._openModuleItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this._openModuleItem.Size = new System.Drawing.Size(231, 26);
            this._openModuleItem.Text = "打开模块";
            this._openModuleItem.Click += new System.EventHandler(this.OpenModuleItem_Click);
            // 
            // SaveTypeMenuItem
            // 
            this.SaveTypeMenuItem.Name = "SaveTypeMenuItem";
            this.SaveTypeMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveTypeMenuItem.Size = new System.Drawing.Size(231, 26);
            this.SaveTypeMenuItem.Text = "保存类型";
            this.SaveTypeMenuItem.Click += new System.EventHandler(this.SaveTypeMenuItem_Click);
            // 
            // _saveModuleItem
            // 
            this._saveModuleItem.Name = "_saveModuleItem";
            this._saveModuleItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this._saveModuleItem.Size = new System.Drawing.Size(231, 26);
            this._saveModuleItem.Text = "保存模块";
            this._saveModuleItem.Click += new System.EventHandler(this.SaveModuleItem_Click);
            // 
            // _saveAnotherModuleItem
            // 
            this._saveAnotherModuleItem.Name = "_saveAnotherModuleItem";
            this._saveAnotherModuleItem.Size = new System.Drawing.Size(231, 26);
            this._saveAnotherModuleItem.Text = "另存模块";
            this._saveAnotherModuleItem.Click += new System.EventHandler(this.SaveAnotherModuleItem_Click);
            // 
            // EditToolStripMenuItem
            // 
            this.EditToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshItem,
            this.toolStripSeparator2,
            this._svnCommitItem,
            this._svnUpdateItem,
            this.toolStripSeparator6,
            this._checkMenuItem});
            this.EditToolStripMenuItem.Name = "EditToolStripMenuItem";
            this.EditToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.EditToolStripMenuItem.Text = "编辑";
            // 
            // RefreshItem
            // 
            this.RefreshItem.Name = "RefreshItem";
            this.RefreshItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.RefreshItem.Size = new System.Drawing.Size(216, 26);
            this.RefreshItem.Text = "刷新";
            this.RefreshItem.Click += new System.EventHandler(this.RefreshItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(142, 6);
            // 
            // _svnCommitItem
            // 
            this._svnCommitItem.Name = "_svnCommitItem";
            this._svnCommitItem.Size = new System.Drawing.Size(145, 26);
            this._svnCommitItem.Text = "提交SVN";
            // 
            // _svnUpdateItem
            // 
            this._svnUpdateItem.Name = "_svnUpdateItem";
            this._svnUpdateItem.Size = new System.Drawing.Size(145, 26);
            this._svnUpdateItem.Text = "更新SVN";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(142, 6);
            // 
            // _checkMenuItem
            // 
            this._checkMenuItem.Name = "_checkMenuItem";
            this._checkMenuItem.Size = new System.Drawing.Size(145, 26);
            this._checkMenuItem.Text = "检查错误";
            this._checkMenuItem.Click += new System.EventHandler(this.CheckError_Click);
            // 
            // ViewToolStripMenuItem
            // 
            this.ViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openFindNamespaceItem,
            this._openConsoleItem,
            this._openTypeEditorItem});
            this.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            this.ViewToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.ViewToolStripMenuItem.Text = "视图";
            // 
            // _openFindNamespaceItem
            // 
            this._openFindNamespaceItem.Name = "_openFindNamespaceItem";
            this._openFindNamespaceItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this._openFindNamespaceItem.Size = new System.Drawing.Size(230, 26);
            this._openFindNamespaceItem.Text = "显示查询空间窗口";
            this._openFindNamespaceItem.Click += new System.EventHandler(this.OpenFindNamespaceItem_Click);
            // 
            // _openConsoleItem
            // 
            this._openConsoleItem.Name = "_openConsoleItem";
            this._openConsoleItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this._openConsoleItem.Size = new System.Drawing.Size(230, 26);
            this._openConsoleItem.Text = "显示日志窗口";
            this._openConsoleItem.Click += new System.EventHandler(this.OpenConsoleItem_Click);
            // 
            // _openTypeEditorItem
            // 
            this._openTypeEditorItem.Name = "_openTypeEditorItem";
            this._openTypeEditorItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this._openTypeEditorItem.Size = new System.Drawing.Size(230, 26);
            this._openTypeEditorItem.Text = "显示类型编辑窗口";
            this._openTypeEditorItem.Click += new System.EventHandler(this.OpenTypeEditorItem_Click);
            // 
            // HelpToolStripMenuItem
            // 
            this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._docItem});
            this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            this.HelpToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.HelpToolStripMenuItem.Text = "其他";
            // 
            // _docItem
            // 
            this._docItem.Name = "_docItem";
            this._docItem.Size = new System.Drawing.Size(114, 26);
            this._docItem.Text = "说明";
            // 
            // _dockPanel
            // 
            this._dockPanel.ActiveAutoHideContent = null;
            this._dockPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dockPanel.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._dockPanel.Location = new System.Drawing.Point(0, 31);
            this._dockPanel.Name = "_dockPanel";
            this._dockPanel.Size = new System.Drawing.Size(1108, 597);
            dockPanelGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin4.DockStripGradient = dockPanelGradient10;
            tabGradient22.EndColor = System.Drawing.SystemColors.Control;
            tabGradient22.StartColor = System.Drawing.SystemColors.Control;
            tabGradient22.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin4.TabGradient = tabGradient22;
            autoHideStripSkin4.TextFont = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            dockPanelSkin4.AutoHideStripSkin = autoHideStripSkin4;
            tabGradient23.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient23.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient23.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient4.ActiveTabGradient = tabGradient23;
            dockPanelGradient11.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient11.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient4.DockStripGradient = dockPanelGradient11;
            tabGradient24.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient24.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient24.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient4.InactiveTabGradient = tabGradient24;
            dockPaneStripSkin4.DocumentGradient = dockPaneStripGradient4;
            dockPaneStripSkin4.TextFont = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            tabGradient25.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient25.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient25.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient25.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient4.ActiveCaptionGradient = tabGradient25;
            tabGradient26.EndColor = System.Drawing.SystemColors.Control;
            tabGradient26.StartColor = System.Drawing.SystemColors.Control;
            tabGradient26.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient4.ActiveTabGradient = tabGradient26;
            dockPanelGradient12.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient12.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient4.DockStripGradient = dockPanelGradient12;
            tabGradient27.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient27.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient27.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient27.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient4.InactiveCaptionGradient = tabGradient27;
            tabGradient28.EndColor = System.Drawing.Color.Transparent;
            tabGradient28.StartColor = System.Drawing.Color.Transparent;
            tabGradient28.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient4.InactiveTabGradient = tabGradient28;
            dockPaneStripSkin4.ToolWindowGradient = dockPaneStripToolWindowGradient4;
            dockPanelSkin4.DockPaneStripSkin = dockPaneStripSkin4;
            this._dockPanel.Skin = dockPanelSkin4;
            this._dockPanel.TabIndex = 8;
            // 
            // _openFileDialog
            // 
            this._openFileDialog.DefaultExt = "xml";
            this._openFileDialog.Filter = "打开模块|*.xml";
            this._openFileDialog.FilterIndex = 0;
            this._openFileDialog.Title = "文件";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "xml";
            this._saveFileDialog.Filter = "保存模块|*.xml";
            this._saveFileDialog.FilterIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(1108, 623);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this._dockPanel);
            this.DoubleBuffered = true;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MainMenu;
            this.MinimumSize = new System.Drawing.Size(1126, 670);
            this.Name = "MainWindow";
            this.Text = "结构描述";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _docItem;
        private System.Windows.Forms.ToolStripMenuItem ViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _createModuleItem;
        private System.Windows.Forms.ToolStripMenuItem _openModuleItem;
        private System.Windows.Forms.ToolStripMenuItem _saveModuleItem;
        private System.Windows.Forms.ToolStripMenuItem _saveAnotherModuleItem;
        private System.Windows.Forms.ToolStripMenuItem _openFindNamespaceItem;
        private System.Windows.Forms.ToolStripMenuItem _openConsoleItem;
        private System.Windows.Forms.ToolStripMenuItem _openTypeEditorItem;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem _createItem;
        private System.Windows.Forms.ToolStripMenuItem SaveTypeMenuItem;
        public WeifenLuo.WinFormsUI.Docking.DockPanel _dockPanel;
        private System.Windows.Forms.ToolStripMenuItem EditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _svnCommitItem;
        private System.Windows.Forms.ToolStripMenuItem _svnUpdateItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem _checkMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RefreshItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

