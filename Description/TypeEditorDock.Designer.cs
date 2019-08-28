namespace Description
{
    partial class TypeEditorDock
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
            this._saveTypeButton = new System.Windows.Forms.Button();
            this._deleteTypeButton = new System.Windows.Forms.Button();
            this.MemberGroupBox = new System.Windows.Forms.GroupBox();
            this.MemberSplitContainer = new System.Windows.Forms.SplitContainer();
            this._memberListBox = new System.Windows.Forms.ListBox();
            this._downPictureBox = new System.Windows.Forms.PictureBox();
            this._upPictureBox = new System.Windows.Forms.PictureBox();
            this._minusPictureBox = new System.Windows.Forms.PictureBox();
            this._plusPictureBox = new System.Windows.Forms.PictureBox();
            this._memPictureBox = new System.Windows.Forms.PictureBox();
            this._memFilterBox = new System.Windows.Forms.TextBox();
            this.TypeGroupBox = new System.Windows.Forms.GroupBox();
            this._baseComboBox = new System.Windows.Forms.ComboBox();
            this._dataPathButton = new System.Windows.Forms.Button();
            this._keyComboBox = new System.Windows.Forms.ComboBox();
            this._typeComboBox = new System.Windows.Forms.ComboBox();
            this.BaseLabel = new System.Windows.Forms.Label();
            this.ShowDataPathLabel = new System.Windows.Forms.Label();
            this.DataPathLabel = new System.Windows.Forms.Label();
            this._displayTextBox = new System.Windows.Forms.TextBox();
            this.DisplayLabel = new System.Windows.Forms.Label();
            this.NamespaceDesLabel = new System.Windows.Forms.Label();
            this.KeyLabel = new System.Windows.Forms.Label();
            this._typeNameTextBox = new System.Windows.Forms.TextBox();
            this.TypeNamelabel = new System.Windows.Forms.Label();
            this._namespaceDesTextBox = new System.Windows.Forms.TextBox();
            this._namespaceTextBox = new System.Windows.Forms.TextBox();
            this.NamespaceLabel = new System.Windows.Forms.Label();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.MemberGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemberSplitContainer)).BeginInit();
            this.MemberSplitContainer.Panel1.SuspendLayout();
            this.MemberSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._downPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._upPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._minusPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._plusPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._memPictureBox)).BeginInit();
            this.TypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _saveTypeButton
            // 
            this._saveTypeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._saveTypeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._saveTypeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._saveTypeButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._saveTypeButton.ForeColor = System.Drawing.Color.LightGray;
            this._saveTypeButton.Location = new System.Drawing.Point(652, 14);
            this._saveTypeButton.Name = "_saveTypeButton";
            this._saveTypeButton.Size = new System.Drawing.Size(166, 38);
            this._saveTypeButton.TabIndex = 8;
            this._saveTypeButton.Text = "保存";
            this._saveTypeButton.UseVisualStyleBackColor = false;
            // 
            // _deleteTypeButton
            // 
            this._deleteTypeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._deleteTypeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._deleteTypeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._deleteTypeButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._deleteTypeButton.ForeColor = System.Drawing.Color.LightGray;
            this._deleteTypeButton.Location = new System.Drawing.Point(652, 56);
            this._deleteTypeButton.Name = "_deleteTypeButton";
            this._deleteTypeButton.Size = new System.Drawing.Size(166, 38);
            this._deleteTypeButton.TabIndex = 10;
            this._deleteTypeButton.Text = "删除";
            this._deleteTypeButton.UseVisualStyleBackColor = false;
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
            this.MemberGroupBox.Size = new System.Drawing.Size(815, 378);
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
            this.MemberSplitContainer.Panel1.Controls.Add(this._downPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._upPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._minusPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._plusPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._memPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._memFilterBox);
            this.MemberSplitContainer.Panel1MinSize = 322;
            this.MemberSplitContainer.Size = new System.Drawing.Size(809, 357);
            this.MemberSplitContainer.SplitterDistance = 322;
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
            this._memberListBox.ForeColor = System.Drawing.Color.LightGray;
            this._memberListBox.FormattingEnabled = true;
            this._memberListBox.ItemHeight = 17;
            this._memberListBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "1",
            "1",
            "q",
            "w",
            "e",
            "r",
            "t",
            "y",
            "u",
            "i",
            "o",
            "a",
            "s",
            "d",
            "g",
            "h"});
            this._memberListBox.Location = new System.Drawing.Point(5, 46);
            this._memberListBox.Name = "_memberListBox";
            this._memberListBox.ScrollAlwaysVisible = true;
            this._memberListBox.Size = new System.Drawing.Size(309, 308);
            this._memberListBox.TabIndex = 8;
            // 
            // _downPictureBox
            // 
            this._downPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._downPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._downPictureBox.Image = global::Description.Properties.Resources.TriangleDown;
            this._downPictureBox.Location = new System.Drawing.Point(288, 10);
            this._downPictureBox.Name = "_downPictureBox";
            this._downPictureBox.Size = new System.Drawing.Size(26, 26);
            this._downPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._downPictureBox.TabIndex = 6;
            this._downPictureBox.TabStop = false;
            // 
            // _upPictureBox
            // 
            this._upPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._upPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._upPictureBox.Image = global::Description.Properties.Resources.TriangleUp;
            this._upPictureBox.Location = new System.Drawing.Point(256, 10);
            this._upPictureBox.Name = "_upPictureBox";
            this._upPictureBox.Size = new System.Drawing.Size(26, 26);
            this._upPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._upPictureBox.TabIndex = 6;
            this._upPictureBox.TabStop = false;
            // 
            // _minusPictureBox
            // 
            this._minusPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._minusPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._minusPictureBox.Image = global::Description.Properties.Resources.Minus;
            this._minusPictureBox.Location = new System.Drawing.Point(224, 10);
            this._minusPictureBox.Name = "_minusPictureBox";
            this._minusPictureBox.Size = new System.Drawing.Size(26, 26);
            this._minusPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._minusPictureBox.TabIndex = 6;
            this._minusPictureBox.TabStop = false;
            // 
            // _plusPictureBox
            // 
            this._plusPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._plusPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._plusPictureBox.Image = global::Description.Properties.Resources.Plus;
            this._plusPictureBox.Location = new System.Drawing.Point(192, 10);
            this._plusPictureBox.Name = "_plusPictureBox";
            this._plusPictureBox.Size = new System.Drawing.Size(26, 26);
            this._plusPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._plusPictureBox.TabIndex = 6;
            this._plusPictureBox.TabStop = false;
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
            // _memFilterBox
            // 
            this._memFilterBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._memFilterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._memFilterBox.ForeColor = System.Drawing.Color.LightGray;
            this._memFilterBox.Location = new System.Drawing.Point(38, 10);
            this._memFilterBox.Name = "_memFilterBox";
            this._memFilterBox.Size = new System.Drawing.Size(144, 27);
            this._memFilterBox.TabIndex = 2;
            // 
            // TypeGroupBox
            // 
            this.TypeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TypeGroupBox.Controls.Add(this._baseComboBox);
            this.TypeGroupBox.Controls.Add(this._dataPathButton);
            this.TypeGroupBox.Controls.Add(this._keyComboBox);
            this.TypeGroupBox.Controls.Add(this._typeComboBox);
            this.TypeGroupBox.Controls.Add(this.BaseLabel);
            this.TypeGroupBox.Controls.Add(this.ShowDataPathLabel);
            this.TypeGroupBox.Controls.Add(this.DataPathLabel);
            this.TypeGroupBox.Controls.Add(this._displayTextBox);
            this.TypeGroupBox.Controls.Add(this.DisplayLabel);
            this.TypeGroupBox.Controls.Add(this.NamespaceDesLabel);
            this.TypeGroupBox.Controls.Add(this.KeyLabel);
            this.TypeGroupBox.Controls.Add(this._typeNameTextBox);
            this.TypeGroupBox.Controls.Add(this.TypeNamelabel);
            this.TypeGroupBox.Controls.Add(this._namespaceDesTextBox);
            this.TypeGroupBox.Controls.Add(this._namespaceTextBox);
            this.TypeGroupBox.Controls.Add(this.NamespaceLabel);
            this.TypeGroupBox.Controls.Add(this.TypeLabel);
            this.TypeGroupBox.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TypeGroupBox.ForeColor = System.Drawing.Color.LightGray;
            this.TypeGroupBox.Location = new System.Drawing.Point(3, 2);
            this.TypeGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.TypeGroupBox.Name = "TypeGroupBox";
            this.TypeGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.TypeGroupBox.Size = new System.Drawing.Size(645, 202);
            this.TypeGroupBox.TabIndex = 7;
            this.TypeGroupBox.TabStop = false;
            this.TypeGroupBox.Text = "类型";
            // 
            // _baseComboBox
            // 
            this._baseComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._baseComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._baseComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._baseComboBox.FormattingEnabled = true;
            this._baseComboBox.Location = new System.Drawing.Point(95, 107);
            this._baseComboBox.Name = "_baseComboBox";
            this._baseComboBox.Size = new System.Drawing.Size(541, 25);
            this._baseComboBox.TabIndex = 3;
            // 
            // _dataPathButton
            // 
            this._dataPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._dataPathButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._dataPathButton.Enabled = false;
            this._dataPathButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._dataPathButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._dataPathButton.ForeColor = System.Drawing.Color.LightGray;
            this._dataPathButton.Location = new System.Drawing.Point(572, 135);
            this._dataPathButton.Name = "_dataPathButton";
            this._dataPathButton.Size = new System.Drawing.Size(63, 27);
            this._dataPathButton.TabIndex = 5;
            this._dataPathButton.Text = "...";
            this._dataPathButton.UseVisualStyleBackColor = false;
            // 
            // _keyComboBox
            // 
            this._keyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._keyComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._keyComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._keyComboBox.FormattingEnabled = true;
            this._keyComboBox.Location = new System.Drawing.Point(435, 48);
            this._keyComboBox.Name = "_keyComboBox";
            this._keyComboBox.Size = new System.Drawing.Size(201, 25);
            this._keyComboBox.TabIndex = 3;
            // 
            // _typeComboBox
            // 
            this._typeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._typeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._typeComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._typeComboBox.FormattingEnabled = true;
            this._typeComboBox.Location = new System.Drawing.Point(95, 19);
            this._typeComboBox.Name = "_typeComboBox";
            this._typeComboBox.Size = new System.Drawing.Size(541, 25);
            this._typeComboBox.TabIndex = 3;
            // 
            // BaseLabel
            // 
            this.BaseLabel.AutoSize = true;
            this.BaseLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BaseLabel.ForeColor = System.Drawing.Color.LightGray;
            this.BaseLabel.Location = new System.Drawing.Point(6, 112);
            this.BaseLabel.Name = "BaseLabel";
            this.BaseLabel.Size = new System.Drawing.Size(45, 15);
            this.BaseLabel.TabIndex = 1;
            this.BaseLabel.Text = "父类:";
            // 
            // ShowDataPathLabel
            // 
            this.ShowDataPathLabel.AutoSize = true;
            this.ShowDataPathLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ShowDataPathLabel.ForeColor = System.Drawing.Color.LightGray;
            this.ShowDataPathLabel.Location = new System.Drawing.Point(95, 140);
            this.ShowDataPathLabel.Name = "ShowDataPathLabel";
            this.ShowDataPathLabel.Size = new System.Drawing.Size(0, 15);
            this.ShowDataPathLabel.TabIndex = 1;
            // 
            // DataPathLabel
            // 
            this.DataPathLabel.AutoSize = true;
            this.DataPathLabel.Enabled = false;
            this.DataPathLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataPathLabel.ForeColor = System.Drawing.Color.LightGray;
            this.DataPathLabel.Location = new System.Drawing.Point(6, 141);
            this.DataPathLabel.Name = "DataPathLabel";
            this.DataPathLabel.Size = new System.Drawing.Size(75, 15);
            this.DataPathLabel.TabIndex = 1;
            this.DataPathLabel.Text = "数据路径:";
            // 
            // _displayTextBox
            // 
            this._displayTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._displayTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._displayTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._displayTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._displayTextBox.Location = new System.Drawing.Point(95, 165);
            this._displayTextBox.Name = "_displayTextBox";
            this._displayTextBox.Size = new System.Drawing.Size(541, 27);
            this._displayTextBox.TabIndex = 2;
            // 
            // DisplayLabel
            // 
            this.DisplayLabel.AutoSize = true;
            this.DisplayLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DisplayLabel.ForeColor = System.Drawing.Color.LightGray;
            this.DisplayLabel.Location = new System.Drawing.Point(6, 171);
            this.DisplayLabel.Name = "DisplayLabel";
            this.DisplayLabel.Size = new System.Drawing.Size(45, 15);
            this.DisplayLabel.TabIndex = 1;
            this.DisplayLabel.Text = "描述:";
            // 
            // NamespaceDesLabel
            // 
            this.NamespaceDesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NamespaceDesLabel.AutoSize = true;
            this.NamespaceDesLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NamespaceDesLabel.ForeColor = System.Drawing.Color.LightGray;
            this.NamespaceDesLabel.Location = new System.Drawing.Point(401, 81);
            this.NamespaceDesLabel.Name = "NamespaceDesLabel";
            this.NamespaceDesLabel.Size = new System.Drawing.Size(15, 15);
            this.NamespaceDesLabel.TabIndex = 1;
            this.NamespaceDesLabel.Text = "-";
            // 
            // KeyLabel
            // 
            this.KeyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.KeyLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.KeyLabel.ForeColor = System.Drawing.Color.LightGray;
            this.KeyLabel.Location = new System.Drawing.Point(392, 53);
            this.KeyLabel.Name = "KeyLabel";
            this.KeyLabel.Size = new System.Drawing.Size(62, 15);
            this.KeyLabel.TabIndex = 1;
            this.KeyLabel.Text = "Key:";
            // 
            // _typeNameTextBox
            // 
            this._typeNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._typeNameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._typeNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._typeNameTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._typeNameTextBox.Location = new System.Drawing.Point(95, 47);
            this._typeNameTextBox.Name = "_typeNameTextBox";
            this._typeNameTextBox.Size = new System.Drawing.Size(288, 27);
            this._typeNameTextBox.TabIndex = 2;
            // 
            // TypeNamelabel
            // 
            this.TypeNamelabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TypeNamelabel.AutoSize = true;
            this.TypeNamelabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TypeNamelabel.ForeColor = System.Drawing.Color.LightGray;
            this.TypeNamelabel.Location = new System.Drawing.Point(4, 53);
            this.TypeNamelabel.Name = "TypeNamelabel";
            this.TypeNamelabel.Size = new System.Drawing.Size(45, 15);
            this.TypeNamelabel.TabIndex = 1;
            this.TypeNamelabel.Text = "名称:";
            // 
            // _namespaceDesTextBox
            // 
            this._namespaceDesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._namespaceDesTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._namespaceDesTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._namespaceDesTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._namespaceDesTextBox.Location = new System.Drawing.Point(435, 77);
            this._namespaceDesTextBox.Name = "_namespaceDesTextBox";
            this._namespaceDesTextBox.Size = new System.Drawing.Size(200, 27);
            this._namespaceDesTextBox.TabIndex = 2;
            // 
            // _namespaceTextBox
            // 
            this._namespaceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._namespaceTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._namespaceTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._namespaceTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._namespaceTextBox.Location = new System.Drawing.Point(95, 77);
            this._namespaceTextBox.Name = "_namespaceTextBox";
            this._namespaceTextBox.Size = new System.Drawing.Size(288, 27);
            this._namespaceTextBox.TabIndex = 2;
            // 
            // NamespaceLabel
            // 
            this.NamespaceLabel.AutoSize = true;
            this.NamespaceLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NamespaceLabel.ForeColor = System.Drawing.Color.LightGray;
            this.NamespaceLabel.Location = new System.Drawing.Point(4, 83);
            this.NamespaceLabel.Name = "NamespaceLabel";
            this.NamespaceLabel.Size = new System.Drawing.Size(75, 15);
            this.NamespaceLabel.TabIndex = 1;
            this.NamespaceLabel.Text = "命名空间:";
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TypeLabel.ForeColor = System.Drawing.Color.LightGray;
            this.TypeLabel.Location = new System.Drawing.Point(4, 24);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(45, 15);
            this.TypeLabel.TabIndex = 1;
            this.TypeLabel.Text = "类型:";
            // 
            // TypeEditorDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(822, 594);
            this.Controls.Add(this._saveTypeButton);
            this.Controls.Add(this._deleteTypeButton);
            this.Controls.Add(this.MemberGroupBox);
            this.Controls.Add(this.TypeGroupBox);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(838, 641);
            this.Name = "TypeEditorDock";
            this.ShowIcon = false;
            this.Text = "类型编辑";
            this.MemberGroupBox.ResumeLayout(false);
            this.MemberSplitContainer.Panel1.ResumeLayout(false);
            this.MemberSplitContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemberSplitContainer)).EndInit();
            this.MemberSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._downPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._upPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._minusPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._plusPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._memPictureBox)).EndInit();
            this.TypeGroupBox.ResumeLayout(false);
            this.TypeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _saveTypeButton;
        private System.Windows.Forms.Button _deleteTypeButton;
        private System.Windows.Forms.GroupBox MemberGroupBox;
        private System.Windows.Forms.SplitContainer MemberSplitContainer;
        private System.Windows.Forms.ListBox _memberListBox;
        private System.Windows.Forms.PictureBox _downPictureBox;
        private System.Windows.Forms.PictureBox _upPictureBox;
        private System.Windows.Forms.PictureBox _minusPictureBox;
        private System.Windows.Forms.PictureBox _plusPictureBox;
        private System.Windows.Forms.PictureBox _memPictureBox;
        private System.Windows.Forms.TextBox _memFilterBox;
        private System.Windows.Forms.GroupBox TypeGroupBox;
        private System.Windows.Forms.ComboBox _baseComboBox;
        private System.Windows.Forms.Button _dataPathButton;
        private System.Windows.Forms.ComboBox _keyComboBox;
        private System.Windows.Forms.ComboBox _typeComboBox;
        private System.Windows.Forms.Label BaseLabel;
        private System.Windows.Forms.Label ShowDataPathLabel;
        private System.Windows.Forms.Label DataPathLabel;
        private System.Windows.Forms.TextBox _displayTextBox;
        private System.Windows.Forms.Label DisplayLabel;
        private System.Windows.Forms.Label NamespaceDesLabel;
        private System.Windows.Forms.Label KeyLabel;
        private System.Windows.Forms.TextBox _typeNameTextBox;
        private System.Windows.Forms.Label TypeNamelabel;
        private System.Windows.Forms.TextBox _namespaceDesTextBox;
        private System.Windows.Forms.TextBox _namespaceTextBox;
        private System.Windows.Forms.Label NamespaceLabel;
        private System.Windows.Forms.Label TypeLabel;
    }
}