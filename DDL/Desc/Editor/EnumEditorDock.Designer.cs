namespace Desc.Editor
{
    partial class EnumEditorDock
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
            this._typeGroupBox = new System.Windows.Forms.GroupBox();
            this._namespaceComboBox = new System.Windows.Forms.ComboBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this._groupTextBox = new System.Windows.Forms.TextBox();
            this._descTextBox = new System.Windows.Forms.TextBox();
            this.NamespaceLabel = new System.Windows.Forms.Label();
            this._groupButton = new System.Windows.Forms.Button();
            this.DescLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this.MemberGroupBox = new System.Windows.Forms.GroupBox();
            this.MemberSplitContainer = new System.Windows.Forms.SplitContainer();
            this._memberListBox = new System.Windows.Forms.ListBox();
            this._memberMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._memPictureBox = new System.Windows.Forms.PictureBox();
            this._memFilterBox = new System.Windows.Forms.TextBox();
            this._spliter = new System.Windows.Forms.PictureBox();
            this._typeGroupBox.SuspendLayout();
            this.MemberGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemberSplitContainer)).BeginInit();
            this.MemberSplitContainer.Panel1.SuspendLayout();
            this.MemberSplitContainer.Panel2.SuspendLayout();
            this.MemberSplitContainer.SuspendLayout();
            this._memberMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._memPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._spliter)).BeginInit();
            this.SuspendLayout();
            // 
            // _typeGroupBox
            // 
            this._typeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._typeGroupBox.Controls.Add(this._namespaceComboBox);
            this._typeGroupBox.Controls.Add(this.NameLabel);
            this._typeGroupBox.Controls.Add(this._groupTextBox);
            this._typeGroupBox.Controls.Add(this._descTextBox);
            this._typeGroupBox.Controls.Add(this.NamespaceLabel);
            this._typeGroupBox.Controls.Add(this._groupButton);
            this._typeGroupBox.Controls.Add(this.DescLabel);
            this._typeGroupBox.Controls.Add(this.label3);
            this._typeGroupBox.Controls.Add(this.label1);
            this._typeGroupBox.Controls.Add(this._nameTextBox);
            this._typeGroupBox.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._typeGroupBox.ForeColor = System.Drawing.Color.LightGray;
            this._typeGroupBox.Location = new System.Drawing.Point(3, 2);
            this._typeGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this._typeGroupBox.Name = "_typeGroupBox";
            this._typeGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this._typeGroupBox.Size = new System.Drawing.Size(828, 202);
            this._typeGroupBox.TabIndex = 7;
            this._typeGroupBox.TabStop = false;
            this._typeGroupBox.Text = "类型";
            // 
            // _namespaceComboBox
            // 
            this._namespaceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._namespaceComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._namespaceComboBox.DisplayMember = "DisplayName";
            this._namespaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._namespaceComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._namespaceComboBox.FormattingEnabled = true;
            this._namespaceComboBox.Location = new System.Drawing.Point(99, 51);
            this._namespaceComboBox.Name = "_namespaceComboBox";
            this._namespaceComboBox.Size = new System.Drawing.Size(713, 25);
            this._namespaceComboBox.Sorted = true;
            this._namespaceComboBox.TabIndex = 3;
            this._namespaceComboBox.SelectedIndexChanged += new System.EventHandler(this.OnValueChange);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NameLabel.ForeColor = System.Drawing.Color.LightGray;
            this.NameLabel.Location = new System.Drawing.Point(14, 23);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(45, 15);
            this.NameLabel.TabIndex = 1;
            this.NameLabel.Text = "名称:";
            // 
            // _groupTextBox
            // 
            this._groupTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._groupTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._groupTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._groupTextBox.Location = new System.Drawing.Point(99, 117);
            this._groupTextBox.Name = "_groupTextBox";
            this._groupTextBox.ReadOnly = true;
            this._groupTextBox.Size = new System.Drawing.Size(644, 20);
            this._groupTextBox.TabIndex = 2;
            this._groupTextBox.TextChanged += new System.EventHandler(this.OnValueChange);
            // 
            // _descTextBox
            // 
            this._descTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._descTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._descTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._descTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._descTextBox.Location = new System.Drawing.Point(99, 83);
            this._descTextBox.Name = "_descTextBox";
            this._descTextBox.Size = new System.Drawing.Size(713, 27);
            this._descTextBox.TabIndex = 2;
            this._descTextBox.TextChanged += new System.EventHandler(this.OnValueChange);
            // 
            // NamespaceLabel
            // 
            this.NamespaceLabel.AutoSize = true;
            this.NamespaceLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NamespaceLabel.ForeColor = System.Drawing.Color.LightGray;
            this.NamespaceLabel.Location = new System.Drawing.Point(14, 56);
            this.NamespaceLabel.Name = "NamespaceLabel";
            this.NamespaceLabel.Size = new System.Drawing.Size(75, 15);
            this.NamespaceLabel.TabIndex = 1;
            this.NamespaceLabel.Text = "命名空间:";
            // 
            // _groupButton
            // 
            this._groupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._groupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._groupButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._groupButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._groupButton.ForeColor = System.Drawing.Color.LightGray;
            this._groupButton.Location = new System.Drawing.Point(750, 113);
            this._groupButton.Name = "_groupButton";
            this._groupButton.Size = new System.Drawing.Size(63, 27);
            this._groupButton.TabIndex = 5;
            this._groupButton.Text = "...";
            this._groupButton.UseVisualStyleBackColor = false;
            this._groupButton.Click += new System.EventHandler(this.GroupButton_Click);
            // 
            // DescLabel
            // 
            this.DescLabel.AutoSize = true;
            this.DescLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DescLabel.ForeColor = System.Drawing.Color.LightGray;
            this.DescLabel.Location = new System.Drawing.Point(14, 89);
            this.DescLabel.Name = "DescLabel";
            this.DescLabel.Size = new System.Drawing.Size(45, 15);
            this.DescLabel.TabIndex = 1;
            this.DescLabel.Text = "描述:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(99, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 15);
            this.label3.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(14, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "分组:";
            // 
            // _nameTextBox
            // 
            this._nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._nameTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._nameTextBox.Location = new System.Drawing.Point(99, 17);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.Size = new System.Drawing.Size(713, 27);
            this._nameTextBox.TabIndex = 2;
            this._nameTextBox.TextChanged += new System.EventHandler(this.OnNameValueChange);
            // 
            // MemberGroupBox
            // 
            this.MemberGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MemberGroupBox.Controls.Add(this.MemberSplitContainer);
            this.MemberGroupBox.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MemberGroupBox.ForeColor = System.Drawing.Color.LightGray;
            this.MemberGroupBox.Location = new System.Drawing.Point(3, 215);
            this.MemberGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.MemberGroupBox.Name = "MemberGroupBox";
            this.MemberGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.MemberGroupBox.Size = new System.Drawing.Size(828, 378);
            this.MemberGroupBox.TabIndex = 6;
            this.MemberGroupBox.TabStop = false;
            this.MemberGroupBox.Text = "成员列表";
            // 
            // MemberSplitContainer
            // 
            this.MemberSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MemberSplitContainer.Location = new System.Drawing.Point(3, 18);
            this.MemberSplitContainer.Name = "MemberSplitContainer";
            // 
            // MemberSplitContainer.Panel1
            // 
            this.MemberSplitContainer.Panel1.Controls.Add(this._memberListBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._memPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._memFilterBox);
            this.MemberSplitContainer.Panel1MinSize = 322;
            // 
            // MemberSplitContainer.Panel2
            // 
            this.MemberSplitContainer.Panel2.Controls.Add(this._spliter);
            this.MemberSplitContainer.Size = new System.Drawing.Size(822, 357);
            this.MemberSplitContainer.SplitterDistance = 327;
            this.MemberSplitContainer.TabIndex = 0;
            // 
            // _memberListBox
            // 
            this._memberListBox.AllowDrop = true;
            this._memberListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._memberListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._memberListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._memberListBox.ContextMenuStrip = this._memberMenu;
            this._memberListBox.DisplayMember = "DisplayName";
            this._memberListBox.ForeColor = System.Drawing.Color.LightGray;
            this._memberListBox.FormattingEnabled = true;
            this._memberListBox.ItemHeight = 17;
            this._memberListBox.Location = new System.Drawing.Point(5, 46);
            this._memberListBox.Name = "_memberListBox";
            this._memberListBox.ScrollAlwaysVisible = true;
            this._memberListBox.Size = new System.Drawing.Size(314, 342);
            this._memberListBox.TabIndex = 8;
            this._memberListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MemberListBox_MouseDown);
            // 
            // _memberMenu
            // 
            this._memberMenu.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._memberMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._memberMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddMenuItem,
            this.RemoveMenuItem});
            this._memberMenu.Name = "_memberMenu";
            this._memberMenu.Size = new System.Drawing.Size(135, 48);
            // 
            // AddMenuItem
            // 
            this.AddMenuItem.Name = "AddMenuItem";
            this.AddMenuItem.Size = new System.Drawing.Size(134, 22);
            this.AddMenuItem.Text = "添加成员";
            this.AddMenuItem.Click += new System.EventHandler(this.AddMenuItem_Click);
            // 
            // RemoveMenuItem
            // 
            this.RemoveMenuItem.Name = "RemoveMenuItem";
            this.RemoveMenuItem.Size = new System.Drawing.Size(134, 22);
            this.RemoveMenuItem.Text = "移除成员";
            this.RemoveMenuItem.Click += new System.EventHandler(this.RemoveMenuItem_Click);
            // 
            // _memPictureBox
            // 
            this._memPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._memPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._memPictureBox.Image = global::Desc.Properties.Resources.MagnifyingGlass;
            this._memPictureBox.Location = new System.Drawing.Point(6, 10);
            this._memPictureBox.Name = "_memPictureBox";
            this._memPictureBox.Size = new System.Drawing.Size(26, 26);
            this._memPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._memPictureBox.TabIndex = 6;
            this._memPictureBox.TabStop = false;
            // 
            // _memFilterBox
            // 
            this._memFilterBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._memFilterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._memFilterBox.ForeColor = System.Drawing.Color.LightGray;
            this._memFilterBox.Location = new System.Drawing.Point(38, 10);
            this._memFilterBox.Name = "_memFilterBox";
            this._memFilterBox.Size = new System.Drawing.Size(276, 27);
            this._memFilterBox.TabIndex = 2;
            this._memFilterBox.TextChanged += new System.EventHandler(this.MemFilterBox_TextChanged);
            // 
            // _spliter
            // 
            this._spliter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._spliter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._spliter.Location = new System.Drawing.Point(3, 3);
            this._spliter.Name = "_spliter";
            this._spliter.Size = new System.Drawing.Size(1, 426);
            this._spliter.TabIndex = 9;
            this._spliter.TabStop = false;
            // 
            // EnumEditorDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(835, 594);
            this.Controls.Add(this.MemberGroupBox);
            this.Controls.Add(this._typeGroupBox);
            this.Name = "EnumEditorDock";
            this.Text = "Enum编辑";
            this._typeGroupBox.ResumeLayout(false);
            this._typeGroupBox.PerformLayout();
            this.MemberGroupBox.ResumeLayout(false);
            this.MemberSplitContainer.Panel1.ResumeLayout(false);
            this.MemberSplitContainer.Panel1.PerformLayout();
            this.MemberSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MemberSplitContainer)).EndInit();
            this.MemberSplitContainer.ResumeLayout(false);
            this._memberMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._memPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._spliter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _typeGroupBox;
        private System.Windows.Forms.ComboBox _namespaceComboBox;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox _descTextBox;
        private System.Windows.Forms.Label NamespaceLabel;
        private System.Windows.Forms.Label DescLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.GroupBox MemberGroupBox;
        private System.Windows.Forms.ListBox _memberListBox;
        private System.Windows.Forms.PictureBox _memPictureBox;
        private System.Windows.Forms.TextBox _memFilterBox;
        internal System.Windows.Forms.SplitContainer MemberSplitContainer;
        private System.Windows.Forms.PictureBox _spliter;
        private System.Windows.Forms.ContextMenuStrip _memberMenu;
        private System.Windows.Forms.ToolStripMenuItem AddMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RemoveMenuItem;
        private System.Windows.Forms.TextBox _groupTextBox;
        private System.Windows.Forms.Button _groupButton;
        private System.Windows.Forms.Label label1;
    }
}
