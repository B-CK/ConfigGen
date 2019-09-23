namespace Description
{
    partial class FindNamespaceDock
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
            this._nodeTreeView = new System.Windows.Forms.TreeView();
            this._classFilterBox = new System.Windows.Forms.TextBox();
            this._classPictureBox = new System.Windows.Forms.PictureBox();
            this._nodeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteNodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._includeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._excludeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交SVNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._showAllBox = new System.Windows.Forms.CheckBox();
            this._errorBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._classPictureBox)).BeginInit();
            this._nodeMenu.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _nodeTreeView
            // 
            this._nodeTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nodeTreeView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._nodeTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._nodeTreeView.ForeColor = System.Drawing.Color.LightGray;
            this._nodeTreeView.Location = new System.Drawing.Point(3, 65);
            this._nodeTreeView.Name = "_nodeTreeView";
            this._nodeTreeView.PathSeparator = ".";
            this._nodeTreeView.Size = new System.Drawing.Size(232, 554);
            this._nodeTreeView.TabIndex = 2;
            this._nodeTreeView.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.NodeTreeView_NodeMouseHover);
            this._nodeTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeTreeView_NodeMouseClick);
            this._nodeTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeTreeView_NodeMouseDoubleClick);
            // 
            // _classFilterBox
            // 
            this._classFilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._classFilterBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._classFilterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._classFilterBox.ForeColor = System.Drawing.Color.LightGray;
            this._classFilterBox.Location = new System.Drawing.Point(39, 35);
            this._classFilterBox.Name = "_classFilterBox";
            this._classFilterBox.Size = new System.Drawing.Size(196, 27);
            this._classFilterBox.TabIndex = 1;
            // 
            // _classPictureBox
            // 
            this._classPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._classPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._classPictureBox.Image = global::Description.Properties.Resources.MagnifyingGlass;
            this._classPictureBox.Location = new System.Drawing.Point(6, 35);
            this._classPictureBox.Name = "_classPictureBox";
            this._classPictureBox.Size = new System.Drawing.Size(26, 26);
            this._classPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._classPictureBox.TabIndex = 9;
            this._classPictureBox.TabStop = false;
            // 
            // _nodeMenu
            // 
            this._nodeMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._nodeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteNodeMenuItem,
            this._includeMenuItem,
            this._excludeMenuItem,
            this.提交SVNToolStripMenuItem,
            this.提交ToolStripMenuItem});
            this._nodeMenu.Name = "_nodeMenu";
            this._nodeMenu.Size = new System.Drawing.Size(169, 124);
            // 
            // DeleteNodeMenuItem
            // 
            this.DeleteNodeMenuItem.Name = "DeleteNodeMenuItem";
            this.DeleteNodeMenuItem.Size = new System.Drawing.Size(168, 24);
            this.DeleteNodeMenuItem.Text = "删除节点";
            this.DeleteNodeMenuItem.Click += new System.EventHandler(this.NodeTreeView_DeleteNode);
            // 
            // _includeMenuItem
            // 
            this._includeMenuItem.Name = "_includeMenuItem";
            this._includeMenuItem.Size = new System.Drawing.Size(168, 24);
            this._includeMenuItem.Text = "包含到模块中";
            this._includeMenuItem.Click += new System.EventHandler(this.NodeTreeView_Include);
            // 
            // _excludeMenuItem
            // 
            this._excludeMenuItem.Name = "_excludeMenuItem";
            this._excludeMenuItem.Size = new System.Drawing.Size(168, 24);
            this._excludeMenuItem.Text = "排除到模块外";
            this._excludeMenuItem.Click += new System.EventHandler(this.NodeTreeView_Exclude);
            // 
            // 提交SVNToolStripMenuItem
            // 
            this.提交SVNToolStripMenuItem.Name = "提交SVNToolStripMenuItem";
            this.提交SVNToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.提交SVNToolStripMenuItem.Text = "更新";
            this.提交SVNToolStripMenuItem.Click += new System.EventHandler(this.UpdateToLib);
            // 
            // 提交ToolStripMenuItem
            // 
            this.提交ToolStripMenuItem.Name = "提交ToolStripMenuItem";
            this.提交ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.提交ToolStripMenuItem.Text = "提交";
            this.提交ToolStripMenuItem.Click += new System.EventHandler(this.CommitToLib);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel.Controls.Add(this._showAllBox);
            this.flowLayoutPanel.Controls.Add(this._errorBox);
            this.flowLayoutPanel.ForeColor = System.Drawing.Color.LightGray;
            this.flowLayoutPanel.Location = new System.Drawing.Point(3, 1);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(232, 30);
            this.flowLayoutPanel.TabIndex = 10;
            // 
            // _showAllBox
            // 
            this._showAllBox.Appearance = System.Windows.Forms.Appearance.Button;
            this._showAllBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._showAllBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._showAllBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._showAllBox.Location = new System.Drawing.Point(3, 0);
            this._showAllBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this._showAllBox.Name = "_showAllBox";
            this._showAllBox.Size = new System.Drawing.Size(23, 27);
            this._showAllBox.TabIndex = 0;
            this._showAllBox.Text = "A";
            this._showAllBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._showAllBox.UseVisualStyleBackColor = false;
            this._showAllBox.CheckedChanged += new System.EventHandler(this.ShowAllBox_CheckedChanged);
            // 
            // _errorBox
            // 
            this._errorBox.Appearance = System.Windows.Forms.Appearance.Button;
            this._errorBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._errorBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._errorBox.Location = new System.Drawing.Point(32, 0);
            this._errorBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this._errorBox.Name = "_errorBox";
            this._errorBox.Size = new System.Drawing.Size(23, 27);
            this._errorBox.TabIndex = 0;
            this._errorBox.Text = "E";
            this._errorBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._errorBox.UseVisualStyleBackColor = true;
            this._errorBox.CheckedChanged += new System.EventHandler(this.ErrorBox_CheckedChanged);
            // 
            // FindNamespaceDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(237, 623);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this._classPictureBox);
            this.Controls.Add(this._nodeTreeView);
            this.Controls.Add(this._classFilterBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindNamespaceDock";
            this.ShowIcon = false;
            this.Text = "空间查询";
            ((System.ComponentModel.ISupportInitialize)(this._classPictureBox)).EndInit();
            this._nodeMenu.ResumeLayout(false);
            this.flowLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _classPictureBox;
        private System.Windows.Forms.TreeView _nodeTreeView;
        private System.Windows.Forms.TextBox _classFilterBox;
        private System.Windows.Forms.ContextMenuStrip _nodeMenu;
        private System.Windows.Forms.ToolStripMenuItem DeleteNodeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交SVNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _includeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _excludeMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.CheckBox _showAllBox;
        private System.Windows.Forms.CheckBox _errorBox;
    }
}