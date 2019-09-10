namespace Description.Editor
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
            this._typeGroupBox = new System.Windows.Forms.GroupBox();
            this._inhertComboBox = new System.Windows.Forms.ComboBox();
            this._namespaceComboBox = new System.Windows.Forms.ComboBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this._descTextBox = new System.Windows.Forms.TextBox();
            this.NamespaceLabel = new System.Windows.Forms.Label();
            this.DescLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this.InhertLabel = new System.Windows.Forms.Label();
            this.MemberGroupBox = new System.Windows.Forms.GroupBox();
            this.MemberSplitContainer = new System.Windows.Forms.SplitContainer();
            this._memberListBox = new System.Windows.Forms.ListBox();
            this._downPictureBox = new System.Windows.Forms.PictureBox();
            this._upPictureBox = new System.Windows.Forms.PictureBox();
            this._minusPictureBox = new System.Windows.Forms.PictureBox();
            this._plusPictureBox = new System.Windows.Forms.PictureBox();
            this._memPictureBox = new System.Windows.Forms.PictureBox();
            this._memFilterBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this._typeGroupBox.SuspendLayout();
            this.MemberGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemberSplitContainer)).BeginInit();
            this.MemberSplitContainer.Panel1.SuspendLayout();
            this.MemberSplitContainer.Panel2.SuspendLayout();
            this.MemberSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._downPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._upPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._minusPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._plusPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._memPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _typeGroupBox
            // 
            this._typeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._typeGroupBox.Controls.Add(this._inhertComboBox);
            this._typeGroupBox.Controls.Add(this._namespaceComboBox);
            this._typeGroupBox.Controls.Add(this.NameLabel);
            this._typeGroupBox.Controls.Add(this._descTextBox);
            this._typeGroupBox.Controls.Add(this.NamespaceLabel);
            this._typeGroupBox.Controls.Add(this.DescLabel);
            this._typeGroupBox.Controls.Add(this.label3);
            this._typeGroupBox.Controls.Add(this._nameTextBox);
            this._typeGroupBox.Controls.Add(this.InhertLabel);
            this._typeGroupBox.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._typeGroupBox.ForeColor = System.Drawing.Color.LightGray;
            this._typeGroupBox.Location = new System.Drawing.Point(3, 2);
            this._typeGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this._typeGroupBox.Name = "_typeGroupBox";
            this._typeGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this._typeGroupBox.Size = new System.Drawing.Size(815, 202);
            this._typeGroupBox.TabIndex = 7;
            this._typeGroupBox.TabStop = false;
            this._typeGroupBox.Text = "类型";
            // 
            // _inhertComboBox
            // 
            this._inhertComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._inhertComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._inhertComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._inhertComboBox.FormattingEnabled = true;
            this._inhertComboBox.Location = new System.Drawing.Point(99, 79);
            this._inhertComboBox.Name = "_inhertComboBox";
            this._inhertComboBox.Size = new System.Drawing.Size(700, 25);
            this._inhertComboBox.Sorted = true;
            this._inhertComboBox.TabIndex = 3;
            // 
            // _namespaceComboBox
            // 
            this._namespaceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._namespaceComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._namespaceComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._namespaceComboBox.FormattingEnabled = true;
            this._namespaceComboBox.Location = new System.Drawing.Point(99, 49);
            this._namespaceComboBox.Name = "_namespaceComboBox";
            this._namespaceComboBox.Size = new System.Drawing.Size(700, 25);
            this._namespaceComboBox.Sorted = true;
            this._namespaceComboBox.TabIndex = 3;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NameLabel.ForeColor = System.Drawing.Color.LightGray;
            this.NameLabel.Location = new System.Drawing.Point(10, 23);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(45, 15);
            this.NameLabel.TabIndex = 1;
            this.NameLabel.Text = "名称:";
            // 
            // _descTextBox
            // 
            this._descTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._descTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._descTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._descTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._descTextBox.Location = new System.Drawing.Point(99, 109);
            this._descTextBox.Name = "_descTextBox";
            this._descTextBox.Size = new System.Drawing.Size(700, 27);
            this._descTextBox.TabIndex = 2;
            // 
            // NamespaceLabel
            // 
            this.NamespaceLabel.AutoSize = true;
            this.NamespaceLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NamespaceLabel.ForeColor = System.Drawing.Color.LightGray;
            this.NamespaceLabel.Location = new System.Drawing.Point(10, 54);
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
            this.DescLabel.Location = new System.Drawing.Point(12, 115);
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
            // _nameTextBox
            // 
            this._nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._nameTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._nameTextBox.Location = new System.Drawing.Point(99, 17);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.Size = new System.Drawing.Size(700, 27);
            this._nameTextBox.TabIndex = 2;
            // 
            // InhertLabel
            // 
            this.InhertLabel.AutoSize = true;
            this.InhertLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InhertLabel.ForeColor = System.Drawing.Color.LightGray;
            this.InhertLabel.Location = new System.Drawing.Point(12, 84);
            this.InhertLabel.Name = "InhertLabel";
            this.InhertLabel.Size = new System.Drawing.Size(45, 15);
            this.InhertLabel.TabIndex = 1;
            this.InhertLabel.Text = "父类:";
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
            // 
            // MemberSplitContainer.Panel2
            // 
            this.MemberSplitContainer.Panel2.Controls.Add(this.textBox1);
            this.MemberSplitContainer.Panel2.Controls.Add(this.label6);
            this.MemberSplitContainer.Panel2.Controls.Add(this.label1);
            this.MemberSplitContainer.Panel2.Controls.Add(this.textBox5);
            this.MemberSplitContainer.Panel2.Controls.Add(this.textBox2);
            this.MemberSplitContainer.Panel2.Controls.Add(this.label5);
            this.MemberSplitContainer.Panel2.Controls.Add(this.textBox3);
            this.MemberSplitContainer.Panel2.Controls.Add(this.label4);
            this.MemberSplitContainer.Panel2.Controls.Add(this.label2);
            this.MemberSplitContainer.Panel2.Controls.Add(this.textBox4);
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
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.ForeColor = System.Drawing.Color.LightGray;
            this.textBox1.Location = new System.Drawing.Point(76, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(394, 27);
            this.textBox1.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.LightGray;
            this.label6.Location = new System.Drawing.Point(8, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "分组:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 23;
            this.label1.Text = "名称:";
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox5.ForeColor = System.Drawing.Color.LightGray;
            this.textBox5.Location = new System.Drawing.Point(76, 45);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(394, 27);
            this.textBox5.TabIndex = 25;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.ForeColor = System.Drawing.Color.LightGray;
            this.textBox2.Location = new System.Drawing.Point(77, 115);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(394, 27);
            this.textBox2.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.LightGray;
            this.label5.Location = new System.Drawing.Point(8, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 15);
            this.label5.TabIndex = 20;
            this.label5.Text = "描述:";
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.ForeColor = System.Drawing.Color.LightGray;
            this.textBox3.Location = new System.Drawing.Point(77, 148);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(394, 27);
            this.textBox3.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.LightGray;
            this.label4.Location = new System.Drawing.Point(8, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 15);
            this.label4.TabIndex = 20;
            this.label4.Text = "别名:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.LightGray;
            this.label2.Location = new System.Drawing.Point(8, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = "默认值:";
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox4.ForeColor = System.Drawing.Color.LightGray;
            this.textBox4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBox4.Location = new System.Drawing.Point(77, 80);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(394, 27);
            this.textBox4.TabIndex = 26;
            // 
            // EnumEditorDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(822, 594);
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
            this.MemberSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemberSplitContainer)).EndInit();
            this.MemberSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._downPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._upPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._minusPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._plusPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._memPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _typeGroupBox;
        private System.Windows.Forms.ComboBox _inhertComboBox;
        private System.Windows.Forms.ComboBox _namespaceComboBox;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox _descTextBox;
        private System.Windows.Forms.Label NamespaceLabel;
        private System.Windows.Forms.Label DescLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.Label InhertLabel;
        private System.Windows.Forms.GroupBox MemberGroupBox;
        private System.Windows.Forms.SplitContainer MemberSplitContainer;
        private System.Windows.Forms.ListBox _memberListBox;
        private System.Windows.Forms.PictureBox _downPictureBox;
        private System.Windows.Forms.PictureBox _upPictureBox;
        private System.Windows.Forms.PictureBox _minusPictureBox;
        private System.Windows.Forms.PictureBox _plusPictureBox;
        private System.Windows.Forms.PictureBox _memPictureBox;
        private System.Windows.Forms.TextBox _memFilterBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox4;
    }
}
