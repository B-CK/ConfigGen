namespace Desc.Editor
{
    partial class ClassEditorDock
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this._typeGroupBox = new System.Windows.Forms.GroupBox();
            this._inheritComboBox = new System.Windows.Forms.ComboBox();
            this._namespaceComboBox = new System.Windows.Forms.ComboBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this._groupTextBox = new System.Windows.Forms.TextBox();
            this._descTextBox = new System.Windows.Forms.TextBox();
            this._groupButton = new System.Windows.Forms.Button();
            this.DataPathButton = new System.Windows.Forms.Button();
            this._dataPathTextBox = new System.Windows.Forms.TextBox();
            this.GroupLabel = new System.Windows.Forms.Label();
            this.NamespaceLabel = new System.Windows.Forms.Label();
            this.DescLabel = new System.Windows.Forms.Label();
            this._indexComboBox = new System.Windows.Forms.ComboBox();
            this.DataPathLabel = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this.IndexLabel = new System.Windows.Forms.Label();
            this.InhertLabel = new System.Windows.Forms.Label();
            this._memberList = new System.Windows.Forms.DataGridView();
            this.MemberListLabel = new System.Windows.Forms.Label();
            this.DataMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._fieldNameLib = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._fieldTypeLib = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this._elememtLib = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._descLib = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._fieldGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._checkerLib = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._typeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._memberList)).BeginInit();
            this.DataMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _typeGroupBox
            // 
            this._typeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._typeGroupBox.Controls.Add(this._inheritComboBox);
            this._typeGroupBox.Controls.Add(this._namespaceComboBox);
            this._typeGroupBox.Controls.Add(this.NameLabel);
            this._typeGroupBox.Controls.Add(this._groupTextBox);
            this._typeGroupBox.Controls.Add(this._descTextBox);
            this._typeGroupBox.Controls.Add(this._groupButton);
            this._typeGroupBox.Controls.Add(this.DataPathButton);
            this._typeGroupBox.Controls.Add(this._dataPathTextBox);
            this._typeGroupBox.Controls.Add(this.GroupLabel);
            this._typeGroupBox.Controls.Add(this.NamespaceLabel);
            this._typeGroupBox.Controls.Add(this.DescLabel);
            this._typeGroupBox.Controls.Add(this._indexComboBox);
            this._typeGroupBox.Controls.Add(this.DataPathLabel);
            this._typeGroupBox.Controls.Add(this._nameTextBox);
            this._typeGroupBox.Controls.Add(this.IndexLabel);
            this._typeGroupBox.Controls.Add(this.InhertLabel);
            this._typeGroupBox.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._typeGroupBox.ForeColor = System.Drawing.Color.LightGray;
            this._typeGroupBox.Location = new System.Drawing.Point(3, 2);
            this._typeGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this._typeGroupBox.Name = "_typeGroupBox";
            this._typeGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this._typeGroupBox.Size = new System.Drawing.Size(828, 250);
            this._typeGroupBox.TabIndex = 7;
            this._typeGroupBox.TabStop = false;
            this._typeGroupBox.Text = "类型";
            // 
            // _inheritComboBox
            // 
            this._inheritComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._inheritComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._inheritComboBox.DisplayMember = "DisplayFullName";
            this._inheritComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._inheritComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._inheritComboBox.FormattingEnabled = true;
            this._inheritComboBox.Location = new System.Drawing.Point(99, 118);
            this._inheritComboBox.Name = "_inheritComboBox";
            this._inheritComboBox.Size = new System.Drawing.Size(713, 25);
            this._inheritComboBox.Sorted = true;
            this._inheritComboBox.TabIndex = 3;
            this._inheritComboBox.SelectedIndexChanged += new System.EventHandler(this.OnInheritChange);
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
            this._namespaceComboBox.Location = new System.Drawing.Point(99, 85);
            this._namespaceComboBox.Name = "_namespaceComboBox";
            this._namespaceComboBox.Size = new System.Drawing.Size(713, 25);
            this._namespaceComboBox.Sorted = true;
            this._namespaceComboBox.TabIndex = 3;
            this._namespaceComboBox.SelectedIndexChanged += new System.EventHandler(this.OnValueChange);
            this._namespaceComboBox.TextChanged += new System.EventHandler(this.OnValueChange);
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
            this._groupTextBox.Location = new System.Drawing.Point(99, 188);
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
            this._descTextBox.Location = new System.Drawing.Point(99, 151);
            this._descTextBox.Name = "_descTextBox";
            this._descTextBox.Size = new System.Drawing.Size(713, 27);
            this._descTextBox.TabIndex = 2;
            this._descTextBox.TextChanged += new System.EventHandler(this.OnValueChange);
            // 
            // _groupButton
            // 
            this._groupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._groupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._groupButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._groupButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._groupButton.ForeColor = System.Drawing.Color.LightGray;
            this._groupButton.Location = new System.Drawing.Point(749, 185);
            this._groupButton.Name = "_groupButton";
            this._groupButton.Size = new System.Drawing.Size(63, 27);
            this._groupButton.TabIndex = 5;
            this._groupButton.Text = "...";
            this._groupButton.UseVisualStyleBackColor = false;
            this._groupButton.Click += new System.EventHandler(this.GroupButton_Click);
            // 
            // DataPathButton
            // 
            this.DataPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DataPathButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.DataPathButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DataPathButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataPathButton.ForeColor = System.Drawing.Color.LightGray;
            this.DataPathButton.Location = new System.Drawing.Point(749, 217);
            this.DataPathButton.Name = "DataPathButton";
            this.DataPathButton.Size = new System.Drawing.Size(63, 27);
            this.DataPathButton.TabIndex = 5;
            this.DataPathButton.Text = "...";
            this.DataPathButton.UseVisualStyleBackColor = false;
            this.DataPathButton.Click += new System.EventHandler(this.DataPathButton_Click);
            // 
            // _dataPathTextBox
            // 
            this._dataPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dataPathTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._dataPathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._dataPathTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._dataPathTextBox.Location = new System.Drawing.Point(99, 219);
            this._dataPathTextBox.Name = "_dataPathTextBox";
            this._dataPathTextBox.ReadOnly = true;
            this._dataPathTextBox.Size = new System.Drawing.Size(644, 20);
            this._dataPathTextBox.TabIndex = 2;
            this._dataPathTextBox.TextChanged += new System.EventHandler(this.OnDataPathChange);
            // 
            // GroupLabel
            // 
            this.GroupLabel.AutoSize = true;
            this.GroupLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GroupLabel.ForeColor = System.Drawing.Color.LightGray;
            this.GroupLabel.Location = new System.Drawing.Point(14, 189);
            this.GroupLabel.Name = "GroupLabel";
            this.GroupLabel.Size = new System.Drawing.Size(45, 15);
            this.GroupLabel.TabIndex = 1;
            this.GroupLabel.Text = "分组:";
            // 
            // NamespaceLabel
            // 
            this.NamespaceLabel.AutoSize = true;
            this.NamespaceLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NamespaceLabel.ForeColor = System.Drawing.Color.LightGray;
            this.NamespaceLabel.Location = new System.Drawing.Point(14, 90);
            this.NamespaceLabel.Name = "NamespaceLabel";
            this.NamespaceLabel.Size = new System.Drawing.Size(75, 15);
            this.NamespaceLabel.TabIndex = 1;
            this.NamespaceLabel.Text = "命名空间:";
            // 
            // DescLabel
            // 
            this.DescLabel.AutoSize = true;
            this.DescLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DescLabel.ForeColor = System.Drawing.Color.LightGray;
            this.DescLabel.Location = new System.Drawing.Point(14, 157);
            this.DescLabel.Name = "DescLabel";
            this.DescLabel.Size = new System.Drawing.Size(45, 15);
            this.DescLabel.TabIndex = 1;
            this.DescLabel.Text = "描述:";
            // 
            // _indexComboBox
            // 
            this._indexComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._indexComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._indexComboBox.DisplayMember = "DisplayName";
            this._indexComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._indexComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._indexComboBox.FormattingEnabled = true;
            this._indexComboBox.Location = new System.Drawing.Point(99, 52);
            this._indexComboBox.Name = "_indexComboBox";
            this._indexComboBox.Size = new System.Drawing.Size(713, 25);
            this._indexComboBox.Sorted = true;
            this._indexComboBox.TabIndex = 3;
            this._indexComboBox.SelectedIndexChanged += new System.EventHandler(this.OnValueChange);
            this._indexComboBox.TextChanged += new System.EventHandler(this.OnValueChange);
            // 
            // DataPathLabel
            // 
            this.DataPathLabel.AutoSize = true;
            this.DataPathLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataPathLabel.ForeColor = System.Drawing.Color.LightGray;
            this.DataPathLabel.Location = new System.Drawing.Point(14, 222);
            this.DataPathLabel.Name = "DataPathLabel";
            this.DataPathLabel.Size = new System.Drawing.Size(75, 15);
            this.DataPathLabel.TabIndex = 1;
            this.DataPathLabel.Text = "数据路径:";
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
            // IndexLabel
            // 
            this.IndexLabel.AutoSize = true;
            this.IndexLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IndexLabel.ForeColor = System.Drawing.Color.LightGray;
            this.IndexLabel.Location = new System.Drawing.Point(14, 57);
            this.IndexLabel.Name = "IndexLabel";
            this.IndexLabel.Size = new System.Drawing.Size(60, 15);
            this.IndexLabel.TabIndex = 1;
            this.IndexLabel.Text = "关键字:";
            // 
            // InhertLabel
            // 
            this.InhertLabel.AutoSize = true;
            this.InhertLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InhertLabel.ForeColor = System.Drawing.Color.LightGray;
            this.InhertLabel.Location = new System.Drawing.Point(14, 123);
            this.InhertLabel.Name = "InhertLabel";
            this.InhertLabel.Size = new System.Drawing.Size(45, 15);
            this.InhertLabel.TabIndex = 1;
            this.InhertLabel.Text = "父类:";
            // 
            // _memberList
            // 
            this._memberList.AllowDrop = true;
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
            this._fieldTypeLib,
            this._elememtLib,
            this._descLib,
            this._fieldGroup,
            this._checkerLib});
            this._memberList.ContextMenuStrip = this.DataMenuStrip;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._memberList.DefaultCellStyle = dataGridViewCellStyle3;
            this._memberList.GridColor = System.Drawing.Color.Silver;
            this._memberList.Location = new System.Drawing.Point(5, 280);
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
            this._memberList.Size = new System.Drawing.Size(826, 312);
            this._memberList.TabIndex = 8;
            this._memberList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.MemberList_CellEndEdit);
            this._memberList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MemberList_CellMouseDoubleClick);
            this._memberList.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MemberList_CellMouseMove);
            this._memberList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.MemberList_RowsAdded);
            this._memberList.DragDrop += new System.Windows.Forms.DragEventHandler(this.MemberList_DragDrop);
            this._memberList.DragEnter += new System.Windows.Forms.DragEventHandler(this.MemberList_DragEnter);
            // 
            // MemberListLabel
            // 
            this.MemberListLabel.AutoSize = true;
            this.MemberListLabel.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.MemberListLabel.ForeColor = System.Drawing.Color.LightGray;
            this.MemberListLabel.Location = new System.Drawing.Point(5, 259);
            this.MemberListLabel.Name = "MemberListLabel";
            this.MemberListLabel.Size = new System.Drawing.Size(90, 17);
            this.MemberListLabel.TabIndex = 9;
            this.MemberListLabel.Text = ">成员列表";
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
            // _fieldNameLib
            // 
            this._fieldNameLib.FillWeight = 89.54317F;
            this._fieldNameLib.HeaderText = "名称";
            this._fieldNameLib.Name = "_fieldNameLib";
            this._fieldNameLib.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._fieldNameLib.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _fieldTypeLib
            // 
            this._fieldTypeLib.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this._fieldTypeLib.FillWeight = 180F;
            this._fieldTypeLib.HeaderText = "类型";
            this._fieldTypeLib.Name = "_fieldTypeLib";
            // 
            // _elememtLib
            // 
            this._elememtLib.FillWeight = 89.54317F;
            this._elememtLib.HeaderText = "值/集合类";
            this._elememtLib.Name = "_elememtLib";
            this._elememtLib.ReadOnly = true;
            this._elememtLib.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._elememtLib.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _descLib
            // 
            this._descLib.FillWeight = 89.54317F;
            this._descLib.HeaderText = "描述";
            this._descLib.Name = "_descLib";
            this._descLib.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _fieldGroup
            // 
            this._fieldGroup.FillWeight = 89.54317F;
            this._fieldGroup.HeaderText = "组";
            this._fieldGroup.Name = "_fieldGroup";
            this._fieldGroup.ReadOnly = true;
            this._fieldGroup.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._fieldGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _checkerLib
            // 
            this._checkerLib.FillWeight = 89.54317F;
            this._checkerLib.HeaderText = "检查条件";
            this._checkerLib.Name = "_checkerLib";
            this._checkerLib.ReadOnly = true;
            this._checkerLib.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this._checkerLib.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClassEditorDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(835, 594);
            this.Controls.Add(this.MemberListLabel);
            this.Controls.Add(this._memberList);
            this.Controls.Add(this._typeGroupBox);
            this.Name = "ClassEditorDock";
            this.Text = "Class编辑";
            this._typeGroupBox.ResumeLayout(false);
            this._typeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._memberList)).EndInit();
            this.DataMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox _typeGroupBox;
        private System.Windows.Forms.ComboBox _inheritComboBox;
        private System.Windows.Forms.ComboBox _namespaceComboBox;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox _descTextBox;
        private System.Windows.Forms.Button DataPathButton;
        private System.Windows.Forms.TextBox _dataPathTextBox;
        private System.Windows.Forms.Label NamespaceLabel;
        private System.Windows.Forms.Label DescLabel;
        private System.Windows.Forms.ComboBox _indexComboBox;
        private System.Windows.Forms.Label DataPathLabel;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.Label IndexLabel;
        private System.Windows.Forms.Label InhertLabel;
        private System.Windows.Forms.Label GroupLabel;
        private System.Windows.Forms.TextBox _groupTextBox;
        private System.Windows.Forms.Button _groupButton;
        private System.Windows.Forms.Label MemberListLabel;
        private System.Windows.Forms.DataGridView _memberList;
        private System.Windows.Forms.ContextMenuStrip DataMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn _fieldNameLib;
        private System.Windows.Forms.DataGridViewComboBoxColumn _fieldTypeLib;
        private System.Windows.Forms.DataGridViewTextBoxColumn _elememtLib;
        private System.Windows.Forms.DataGridViewTextBoxColumn _descLib;
        private System.Windows.Forms.DataGridViewTextBoxColumn _fieldGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn _checkerLib;
    }
}