namespace Description
{
    partial class NamespaceDock
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
            this._nodeFilterBox = new System.Windows.Forms.TextBox();
            this._classPictureBox = new System.Windows.Forms.PictureBox();
            this._nodeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._modifyRootMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveRootMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._excludeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._includeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._rootSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteNodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._nodeSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.提交SVNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._showAllBox = new System.Windows.Forms.CheckBox();
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
            this._nodeTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeTreeView_NodeMouseClick);
            this._nodeTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeTreeView_NodeMouseDoubleClick);
            // 
            // _nodeFilterBox
            // 
            this._nodeFilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nodeFilterBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._nodeFilterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._nodeFilterBox.ForeColor = System.Drawing.Color.LightGray;
            this._nodeFilterBox.Location = new System.Drawing.Point(39, 35);
            this._nodeFilterBox.Name = "_nodeFilterBox";
            this._nodeFilterBox.Size = new System.Drawing.Size(196, 27);
            this._nodeFilterBox.TabIndex = 1;
            this._nodeFilterBox.TextChanged += new System.EventHandler(this.NodeFilterBox_TextChanged);
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
            this._modifyRootMenuItem,
            this._saveRootMenuItem,
            this._includeMenuItem,
            this._excludeMenuItem,
            this._rootSeparator,
            this.DeleteNodeMenuItem,
            this._nodeSeparator,
            this.提交SVNToolStripMenuItem,
            this.提交ToolStripMenuItem});
            this._nodeMenu.Name = "_nodeMenu";
            this._nodeMenu.Size = new System.Drawing.Size(211, 212);
            // 
            // _modifyRootMenuItem
            // 
            this._modifyRootMenuItem.Name = "_modifyRootMenuItem";
            this._modifyRootMenuItem.Size = new System.Drawing.Size(210, 24);
            this._modifyRootMenuItem.Text = "修改根属性";
            this._modifyRootMenuItem.Click += new System.EventHandler(this.NodeTreeView_Modify);
            // 
            // _saveRootMenuItem
            // 
            this._saveRootMenuItem.Name = "_saveRootMenuItem";
            this._saveRootMenuItem.Size = new System.Drawing.Size(210, 24);
            this._saveRootMenuItem.Text = "保存根节点";
            this._saveRootMenuItem.Click += new System.EventHandler(this.NodeTreeView_Save);
            // 
            // _excludeMenuItem
            // 
            this._excludeMenuItem.Name = "_excludeMenuItem";
            this._excludeMenuItem.Size = new System.Drawing.Size(210, 24);
            this._excludeMenuItem.Text = "排除";
            this._excludeMenuItem.Click += new System.EventHandler(this.NodeTreeView_Exclude);
            // 
            // _includeMenuItem
            // 
            this._includeMenuItem.Name = "_includeMenuItem";
            this._includeMenuItem.Size = new System.Drawing.Size(210, 24);
            this._includeMenuItem.Text = "包含";
            this._includeMenuItem.Click += new System.EventHandler(this.NodeTreeView_Include);
            // 
            // _rootSeparator
            // 
            this._rootSeparator.Name = "_rootSeparator";
            this._rootSeparator.Size = new System.Drawing.Size(207, 6);
            // 
            // DeleteNodeMenuItem
            // 
            this.DeleteNodeMenuItem.Name = "DeleteNodeMenuItem";
            this.DeleteNodeMenuItem.Size = new System.Drawing.Size(210, 24);
            this.DeleteNodeMenuItem.Text = "删除节点";
            this.DeleteNodeMenuItem.Click += new System.EventHandler(this.NodeTreeView_DeleteNode);
            // 
            // _nodeSeparator
            // 
            this._nodeSeparator.Name = "_nodeSeparator";
            this._nodeSeparator.Size = new System.Drawing.Size(207, 6);
            // 
            // 提交SVNToolStripMenuItem
            // 
            this.提交SVNToolStripMenuItem.Name = "提交SVNToolStripMenuItem";
            this.提交SVNToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.提交SVNToolStripMenuItem.Text = "更新";
            this.提交SVNToolStripMenuItem.Click += new System.EventHandler(this.UpdateToLib);
            // 
            // 提交ToolStripMenuItem
            // 
            this.提交ToolStripMenuItem.Name = "提交ToolStripMenuItem";
            this.提交ToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.提交ToolStripMenuItem.Text = "提交";
            this.提交ToolStripMenuItem.Click += new System.EventHandler(this.CommitToLib);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel.Controls.Add(this._showAllBox);
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
            // NamespaceDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(237, 623);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this._classPictureBox);
            this.Controls.Add(this._nodeTreeView);
            this.Controls.Add(this._nodeFilterBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NamespaceDock";
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
        private System.Windows.Forms.TextBox _nodeFilterBox;
        private System.Windows.Forms.ContextMenuStrip _nodeMenu;
        private System.Windows.Forms.ToolStripMenuItem DeleteNodeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交SVNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _includeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _excludeMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.CheckBox _showAllBox;
        private System.Windows.Forms.ToolStripMenuItem _saveRootMenuItem;
        private System.Windows.Forms.ToolStripSeparator _nodeSeparator;
        private System.Windows.Forms.ToolStripMenuItem _modifyRootMenuItem;
        private System.Windows.Forms.ToolStripSeparator _rootSeparator;
    }
}