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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.MemberListLabel = new System.Windows.Forms.Label();
            this._memberList = new System.Windows.Forms.DataGridView();
            this._fieldNameLib = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._fieldAliasLib = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._fieldValueLib = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._fieldDescLib = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._fieldGroupLib = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._typeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._memberList)).BeginInit();
            this.DataMenuStrip.SuspendLayout();
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
            // MemberListLabel
            // 
            this.MemberListLabel.AutoSize = true;
            this.MemberListLabel.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.MemberListLabel.ForeColor = System.Drawing.Color.LightGray;
            this.MemberListLabel.Location = new System.Drawing.Point(4, 211);
            this.MemberListLabel.Name = "MemberListLabel";
            this.MemberListLabel.Size = new System.Drawing.Size(90, 17);
            this.MemberListLabel.TabIndex = 11;
            this.MemberListLabel.Text = ">成员列表";
            // 
            // _memberList
            // 
            this._memberList.AllowDrop = true;
            this._memberList.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.LightGray;
            this._memberList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this._memberList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._memberList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._memberList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._memberList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._memberList.CausesValidation = false;
            this._memberList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._memberList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this._memberList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._memberList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._fieldNameLib,
            this._fieldAliasLib,
            this._fieldValueLib,
            this._fieldDescLib,
            this._fieldGroupLib});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._memberList.DefaultCellStyle = dataGridViewCellStyle3;
            this._memberList.GridColor = System.Drawing.Color.Silver;
            this._memberList.Location = new System.Drawing.Point(4, 232);
            this._memberList.Name = "_memberList";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._memberList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this._memberList.RowHeadersWidth = 28;
            this._memberList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.LightGray;
            this._memberList.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this._memberList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._memberList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
            this._memberList.RowTemplate.Height = 27;
            this._memberList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._memberList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._memberList.Size = new System.Drawing.Size(826, 360);
            this._memberList.TabIndex = 10;
            this._memberList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.MemberList_CellEndEdit);
            this._memberList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MemberList_CellMouseDoubleClick);
            this._memberList.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MemberList_CellMouseMove);
            this._memberList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.MemberList_RowsAdded);
            this._memberList.DragDrop += new System.Windows.Forms.DragEventHandler(this.MemberList_DragDrop);
            this._memberList.DragEnter += new System.Windows.Forms.DragEventHandler(this.MemberList_DragEnter);
            // 
            // _fieldNameLib
            // 
            this._fieldNameLib.FillWeight = 89.54317F;
            this._fieldNameLib.HeaderText = "名称";
            this._fieldNameLib.Name = "_fieldNameLib";
            this._fieldNameLib.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._fieldNameLib.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _fieldAliasLib
            // 
            this._fieldAliasLib.HeaderText = "别名";
            this._fieldAliasLib.Name = "_fieldAliasLib";
            this._fieldAliasLib.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._fieldAliasLib.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _fieldValueLib
            // 
            this._fieldValueLib.FillWeight = 89.54317F;
            this._fieldValueLib.HeaderText = "值";
            this._fieldValueLib.Name = "_fieldValueLib";
            this._fieldValueLib.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._fieldValueLib.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _fieldDescLib
            // 
            this._fieldDescLib.FillWeight = 150F;
            this._fieldDescLib.HeaderText = "描述";
            this._fieldDescLib.Name = "_fieldDescLib";
            this._fieldDescLib.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _fieldGroupLib
            // 
            this._fieldGroupLib.FillWeight = 89.54317F;
            this._fieldGroupLib.HeaderText = "组";
            this._fieldGroupLib.Name = "_fieldGroupLib";
            this._fieldGroupLib.ReadOnly = true;
            this._fieldGroupLib.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._fieldGroupLib.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DataMenuStrip
            // 
            this.DataMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.DataMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteToolStripMenuItem});
            this.DataMenuStrip.Name = "DataMenuStrip";
            this.DataMenuStrip.Size = new System.Drawing.Size(154, 28);
            // 
            // DeleteToolStripMenuItem
            // 
            this.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            this.DeleteToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.DeleteToolStripMenuItem.Text = "删除选择行";
            this.DeleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // EnumEditorDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(835, 594);
            this.Controls.Add(this.MemberListLabel);
            this.Controls.Add(this._memberList);
            this.Controls.Add(this._typeGroupBox);
            this.Name = "EnumEditorDock";
            this.Text = "Enum编辑";
            this._typeGroupBox.ResumeLayout(false);
            this._typeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._memberList)).EndInit();
            this.DataMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.TextBox _groupTextBox;
        private System.Windows.Forms.Button _groupButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label MemberListLabel;
        private System.Windows.Forms.DataGridView _memberList;
        private System.Windows.Forms.ContextMenuStrip DataMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn _fieldNameLib;
        private System.Windows.Forms.DataGridViewTextBoxColumn _fieldAliasLib;
        private System.Windows.Forms.DataGridViewTextBoxColumn _fieldValueLib;
        private System.Windows.Forms.DataGridViewTextBoxColumn _fieldDescLib;
        private System.Windows.Forms.DataGridViewTextBoxColumn _fieldGroupLib;
    }
}
