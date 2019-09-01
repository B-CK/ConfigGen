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
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除EnumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除命名空间ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.提交SVNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this._classPictureBox)).BeginInit();
            this._nodeMenu.SuspendLayout();
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
            this._nodeTreeView.Location = new System.Drawing.Point(3, 35);
            this._nodeTreeView.Name = "_nodeTreeView";
            this._nodeTreeView.PathSeparator = ".";
            this._nodeTreeView.Size = new System.Drawing.Size(232, 584);
            this._nodeTreeView.TabIndex = 2;
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
            this._classFilterBox.Location = new System.Drawing.Point(39, 3);
            this._classFilterBox.Name = "_classFilterBox";
            this._classFilterBox.Size = new System.Drawing.Size(196, 27);
            this._classFilterBox.TabIndex = 1;
            // 
            // _classPictureBox
            // 
            this._classPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._classPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._classPictureBox.Image = global::Description.Properties.Resources.MagnifyingGlass;
            this._classPictureBox.Location = new System.Drawing.Point(6, 3);
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
            this.删除ToolStripMenuItem,
            this.删除EnumToolStripMenuItem,
            this.删除命名空间ToolStripMenuItem,
            this.toolStripSeparator1,
            this.提交SVNToolStripMenuItem,
            this.提交ToolStripMenuItem});
            this._nodeMenu.Name = "_nodeMenu";
            this._nodeMenu.Size = new System.Drawing.Size(211, 158);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.删除ToolStripMenuItem.Text = "删除Class";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.DeleteClass);
            // 
            // 删除EnumToolStripMenuItem
            // 
            this.删除EnumToolStripMenuItem.Name = "删除EnumToolStripMenuItem";
            this.删除EnumToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.删除EnumToolStripMenuItem.Text = "删除Enum";
            this.删除EnumToolStripMenuItem.Click += new System.EventHandler(this.DeleteEnum);
            // 
            // 删除命名空间ToolStripMenuItem
            // 
            this.删除命名空间ToolStripMenuItem.Name = "删除命名空间ToolStripMenuItem";
            this.删除命名空间ToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.删除命名空间ToolStripMenuItem.Text = "删除命名空间";
            this.删除命名空间ToolStripMenuItem.Click += new System.EventHandler(this.DeleteNamespace);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
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
            // FindNamespaceDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(237, 623);
            this.Controls.Add(this._classPictureBox);
            this.Controls.Add(this._nodeTreeView);
            this.Controls.Add(this._classFilterBox);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _classPictureBox;
        private System.Windows.Forms.TreeView _nodeTreeView;
        private System.Windows.Forms.TextBox _classFilterBox;
        private System.Windows.Forms.ContextMenuStrip _nodeMenu;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除EnumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除命名空间ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 提交SVNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交ToolStripMenuItem;
    }
}