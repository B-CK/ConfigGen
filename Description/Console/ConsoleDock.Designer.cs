namespace Description
{
    partial class ConsoleDock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleDock));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this._logCheckBox = new System.Windows.Forms.CheckBox();
            this._logWarnCheckBox = new System.Windows.Forms.CheckBox();
            this._logErrorCheckBox = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._logListView = new System.Windows.Forms.ListView();
            this.flowLayoutPanel1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Controls.Add(this._logCheckBox);
            this.flowLayoutPanel1.Controls.Add(this._logWarnCheckBox);
            this.flowLayoutPanel1.Controls.Add(this._logErrorCheckBox);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // _logCheckBox
            // 
            resources.ApplyResources(this._logCheckBox, "_logCheckBox");
            this._logCheckBox.Checked = true;
            this._logCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._logCheckBox.ForeColor = System.Drawing.Color.LightGray;
            this._logCheckBox.Image = global::Description.Properties.Resources.ConsoleInfoIcon;
            this._logCheckBox.Name = "_logCheckBox";
            this._logCheckBox.UseVisualStyleBackColor = true;
            this._logCheckBox.CheckStateChanged += new System.EventHandler(this.LogCheckStateChanged);
            // 
            // _logWarnCheckBox
            // 
            resources.ApplyResources(this._logWarnCheckBox, "_logWarnCheckBox");
            this._logWarnCheckBox.Checked = true;
            this._logWarnCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._logWarnCheckBox.ForeColor = System.Drawing.Color.LightGray;
            this._logWarnCheckBox.Image = global::Description.Properties.Resources.ConsoleWarnicon;
            this._logWarnCheckBox.Name = "_logWarnCheckBox";
            this._logWarnCheckBox.UseVisualStyleBackColor = true;
            this._logWarnCheckBox.CheckStateChanged += new System.EventHandler(this.LogCheckStateChanged);
            // 
            // _logErrorCheckBox
            // 
            resources.ApplyResources(this._logErrorCheckBox, "_logErrorCheckBox");
            this._logErrorCheckBox.Checked = true;
            this._logErrorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._logErrorCheckBox.ForeColor = System.Drawing.Color.LightGray;
            this._logErrorCheckBox.Image = global::Description.Properties.Resources.ConsoleErroricon;
            this._logErrorCheckBox.Name = "_logErrorCheckBox";
            this._logErrorCheckBox.UseVisualStyleBackColor = true;
            this._logErrorCheckBox.CheckStateChanged += new System.EventHandler(this.LogCheckStateChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._clearAllToolStripMenuItem,
            this.toolStripSeparator1,
            this._copyToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            // 
            // _clearAllToolStripMenuItem
            // 
            this._clearAllToolStripMenuItem.Name = "_clearAllToolStripMenuItem";
            resources.ApplyResources(this._clearAllToolStripMenuItem, "_clearAllToolStripMenuItem");
            this._clearAllToolStripMenuItem.Click += new System.EventHandler(this.ClearLogs);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // _copyToolStripMenuItem
            // 
            this._copyToolStripMenuItem.Name = "_copyToolStripMenuItem";
            resources.ApplyResources(this._copyToolStripMenuItem, "_copyToolStripMenuItem");
            this._copyToolStripMenuItem.Click += new System.EventHandler(this.CopyLogs);
            // 
            // _logListView
            // 
            resources.ApplyResources(this._logListView, "_logListView");
            this._logListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._logListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._logListView.ContextMenuStrip = this.contextMenuStrip;
            this._logListView.ForeColor = System.Drawing.Color.LightGray;
            this._logListView.FullRowSelect = true;
            this._logListView.HideSelection = false;
            this._logListView.Name = "_logListView";
            this._logListView.TileSize = new System.Drawing.Size(980, 20);
            this._logListView.UseCompatibleStateImageBehavior = false;
            this._logListView.View = System.Windows.Forms.View.Tile;
            this._logListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LogListView_KeyDown);
            this._logListView.MouseEnter += new System.EventHandler(this.LogListView_Enter);
            this._logListView.MouseLeave += new System.EventHandler(this.LogListView_Leave);
            // 
            // ConsoleDock
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ControlBox = false;
            this.Controls.Add(this._logListView);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConsoleDock";
            this.ShowIcon = false;
            this.TabText = "日志";
            this.SizeChanged += new System.EventHandler(this.ConsoleDock_SizeChanged);
            this.Click += new System.EventHandler(this.ConsoleDock_DoubleClick);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox _logCheckBox;
        private System.Windows.Forms.CheckBox _logWarnCheckBox;
        private System.Windows.Forms.CheckBox _logErrorCheckBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _clearAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _copyToolStripMenuItem;
        internal System.Windows.Forms.ListView _logListView;
    }
}