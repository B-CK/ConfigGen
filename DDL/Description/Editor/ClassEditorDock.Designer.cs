﻿namespace Description.Editor
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
            this.MemberGroupBox = new System.Windows.Forms.GroupBox();
            this.MemberSplitContainer = new System.Windows.Forms.SplitContainer();
            this.Down = new System.Windows.Forms.PictureBox();
            this.Up = new System.Windows.Forms.PictureBox();
            this._memberListBox = new System.Windows.Forms.ListBox();
            this._memberMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._memPictureBox = new System.Windows.Forms.PictureBox();
            this._memFindBox = new System.Windows.Forms.TextBox();
            this._spliter = new System.Windows.Forms.PictureBox();
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
            this.MemberGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemberSplitContainer)).BeginInit();
            this.MemberSplitContainer.Panel1.SuspendLayout();
            this.MemberSplitContainer.Panel2.SuspendLayout();
            this.MemberSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Up)).BeginInit();
            this._memberMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._memPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._spliter)).BeginInit();
            this._typeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MemberGroupBox
            // 
            this.MemberGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MemberGroupBox.Controls.Add(this.MemberSplitContainer);
            this.MemberGroupBox.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MemberGroupBox.ForeColor = System.Drawing.Color.LightGray;
            this.MemberGroupBox.Location = new System.Drawing.Point(3, 263);
            this.MemberGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.MemberGroupBox.Name = "MemberGroupBox";
            this.MemberGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.MemberGroupBox.Size = new System.Drawing.Size(828, 330);
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
            this.MemberSplitContainer.Panel1.Controls.Add(this.Down);
            this.MemberSplitContainer.Panel1.Controls.Add(this.Up);
            this.MemberSplitContainer.Panel1.Controls.Add(this._memberListBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._memPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._memFindBox);
            this.MemberSplitContainer.Panel1MinSize = 322;
            // 
            // MemberSplitContainer.Panel2
            // 
            this.MemberSplitContainer.Panel2.Controls.Add(this._spliter);
            this.MemberSplitContainer.Size = new System.Drawing.Size(822, 309);
            this.MemberSplitContainer.SplitterDistance = 337;
            this.MemberSplitContainer.TabIndex = 0;
            // 
            // Down
            // 
            this.Down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Down.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Down.Cursor = System.Windows.Forms.Cursors.Default;
            this.Down.Image = global::Description.Properties.Resources.ArrowDown;
            this.Down.InitialImage = global::Description.Properties.Resources.ArrowDown;
            this.Down.Location = new System.Drawing.Point(297, 4);
            this.Down.Name = "Down";
            this.Down.Size = new System.Drawing.Size(35, 35);
            this.Down.TabIndex = 9;
            this.Down.TabStop = false;
            this.Down.Click += new System.EventHandler(this.Down_Click);
            this.Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Picture_MouseDown);
            this.Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Picture_MouseUp);
            // 
            // Up
            // 
            this.Up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Up.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Up.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Up.Cursor = System.Windows.Forms.Cursors.Default;
            this.Up.Image = global::Description.Properties.Resources.ArrowUp;
            this.Up.InitialImage = global::Description.Properties.Resources.ArrowUp;
            this.Up.Location = new System.Drawing.Point(257, 5);
            this.Up.Name = "Up";
            this.Up.Size = new System.Drawing.Size(35, 35);
            this.Up.TabIndex = 9;
            this.Up.TabStop = false;
            this.Up.Click += new System.EventHandler(this.Up_Click);
            this.Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Picture_MouseDown);
            this.Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Picture_MouseUp);
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
            this._memberListBox.Size = new System.Drawing.Size(329, 257);
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
            this._memPictureBox.Image = global::Description.Properties.Resources.MagnifyingGlass;
            this._memPictureBox.Location = new System.Drawing.Point(6, 10);
            this._memPictureBox.Name = "_memPictureBox";
            this._memPictureBox.Size = new System.Drawing.Size(26, 26);
            this._memPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._memPictureBox.TabIndex = 6;
            this._memPictureBox.TabStop = false;
            // 
            // _memFindBox
            // 
            this._memFindBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._memFindBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._memFindBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._memFindBox.ForeColor = System.Drawing.Color.LightGray;
            this._memFindBox.Location = new System.Drawing.Point(38, 10);
            this._memFindBox.Name = "_memFindBox";
            this._memFindBox.Size = new System.Drawing.Size(214, 27);
            this._memFindBox.TabIndex = 2;
            this._memFindBox.TextChanged += new System.EventHandler(this.MemFilterBox_TextChanged);
            // 
            // _spliter
            // 
            this._spliter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._spliter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._spliter.Location = new System.Drawing.Point(0, 0);
            this._spliter.Name = "_spliter";
            this._spliter.Size = new System.Drawing.Size(1, 322);
            this._spliter.TabIndex = 9;
            this._spliter.TabStop = false;
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
            this._inheritComboBox.SelectedIndexChanged += new System.EventHandler(this.OnValueChange);
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
            // ClassEditorDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(835, 594);
            this.Controls.Add(this.MemberGroupBox);
            this.Controls.Add(this._typeGroupBox);
            this.Name = "ClassEditorDock";
            this.Text = "Class编辑";
            this.MemberGroupBox.ResumeLayout(false);
            this.MemberSplitContainer.Panel1.ResumeLayout(false);
            this.MemberSplitContainer.Panel1.PerformLayout();
            this.MemberSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MemberSplitContainer)).EndInit();
            this.MemberSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Up)).EndInit();
            this._memberMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._memPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._spliter)).EndInit();
            this._typeGroupBox.ResumeLayout(false);
            this._typeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox _memberListBox;
        private System.Windows.Forms.PictureBox _memPictureBox;
        private System.Windows.Forms.TextBox _memFindBox;
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
        private System.Windows.Forms.ContextMenuStrip _memberMenu;
        private System.Windows.Forms.ToolStripMenuItem AddMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RemoveMenuItem;
        private System.Windows.Forms.PictureBox _spliter;
        private System.Windows.Forms.GroupBox MemberGroupBox;
        internal System.Windows.Forms.SplitContainer MemberSplitContainer;
        private System.Windows.Forms.Label GroupLabel;
        private System.Windows.Forms.TextBox _groupTextBox;
        private System.Windows.Forms.Button _groupButton;
        private System.Windows.Forms.PictureBox Up;
        private System.Windows.Forms.PictureBox Down;
    }
}