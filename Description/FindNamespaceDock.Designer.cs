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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点2", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this._typeTreeView = new System.Windows.Forms.TreeView();
            this._classFilterBox = new System.Windows.Forms.TextBox();
            this._classPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._classPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _typeTreeView
            // 
            this._typeTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._typeTreeView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._typeTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._typeTreeView.ForeColor = System.Drawing.Color.LightGray;
            this._typeTreeView.Location = new System.Drawing.Point(3, 35);
            this._typeTreeView.Name = "_typeTreeView";
            treeNode1.Name = "节点3";
            treeNode1.Text = "节点3";
            treeNode2.ForeColor = System.Drawing.Color.Black;
            treeNode2.Name = "节点2";
            treeNode2.Text = "节点2";
            this._typeTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this._typeTreeView.PathSeparator = ".";
            this._typeTreeView.Size = new System.Drawing.Size(232, 584);
            this._typeTreeView.TabIndex = 2;
            this._typeTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TypeTreeView_NodeMouseDoubleClick);
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
            // FindNamespaceDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(237, 623);
            this.Controls.Add(this._classPictureBox);
            this.Controls.Add(this._typeTreeView);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _classPictureBox;
        private System.Windows.Forms.TreeView _typeTreeView;
        private System.Windows.Forms.TextBox _classFilterBox;
    }
}