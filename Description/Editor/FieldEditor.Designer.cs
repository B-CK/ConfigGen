namespace Description.Editor
{
    partial class FieldEditor
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this._desTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._defaultLabel = new System.Windows.Forms.Label();
            this._checkerComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._typeComboBox = new System.Windows.Forms.ComboBox();
            this._isConstCheckBox = new System.Windows.Forms.CheckBox();
            this._valueTypeBox = new Description.TypeBox();
            this._groupComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // _nameTextBox
            // 
            this._nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._nameTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._nameTextBox.Location = new System.Drawing.Point(79, 5);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.Size = new System.Drawing.Size(400, 25);
            this._nameTextBox.TabIndex = 12;
            this._nameTextBox.TextChanged += new System.EventHandler(this.OnFieldNameChanged);
            // 
            // _desTextBox
            // 
            this._desTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._desTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._desTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._desTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._desTextBox.Location = new System.Drawing.Point(79, 142);
            this._desTextBox.Name = "_desTextBox";
            this._desTextBox.Size = new System.Drawing.Size(400, 25);
            this._desTextBox.TabIndex = 10;
            this._desTextBox.TextChanged += new System.EventHandler(this.OnFieldTextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(8, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "分组:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.LightGray;
            this.label2.Location = new System.Drawing.Point(8, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "描述:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(8, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "检查:";
            // 
            // _defaultLabel
            // 
            this._defaultLabel.AutoSize = true;
            this._defaultLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._defaultLabel.ForeColor = System.Drawing.Color.LightGray;
            this._defaultLabel.Location = new System.Drawing.Point(8, 80);
            this._defaultLabel.Name = "_defaultLabel";
            this._defaultLabel.Size = new System.Drawing.Size(60, 15);
            this._defaultLabel.TabIndex = 7;
            this._defaultLabel.Text = "默认值:";
            // 
            // _checkerComboBox
            // 
            this._checkerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._checkerComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._checkerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._checkerComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._checkerComboBox.FormattingEnabled = true;
            this._checkerComboBox.Location = new System.Drawing.Point(79, 176);
            this._checkerComboBox.Name = "_checkerComboBox";
            this._checkerComboBox.Size = new System.Drawing.Size(400, 23);
            this._checkerComboBox.TabIndex = 14;
            this._checkerComboBox.SelectedIndexChanged += new System.EventHandler(this.OnFieldTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.LightGray;
            this.label5.Location = new System.Drawing.Point(8, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "类型:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.LightGray;
            this.label6.Location = new System.Drawing.Point(8, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "名称:";
            // 
            // _typeComboBox
            // 
            this._typeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._typeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._typeComboBox.DisplayMember = "DisplayFullName";
            this._typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._typeComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._typeComboBox.FormattingEnabled = true;
            this._typeComboBox.Location = new System.Drawing.Point(79, 39);
            this._typeComboBox.Name = "_typeComboBox";
            this._typeComboBox.Size = new System.Drawing.Size(327, 23);
            this._typeComboBox.Sorted = true;
            this._typeComboBox.TabIndex = 13;
            this._typeComboBox.ValueMember = "FullName";
            this._typeComboBox.SelectedIndexChanged += new System.EventHandler(this.TypeComboBox_SelectedIndexChanged);
            // 
            // _isConstCheckBox
            // 
            this._isConstCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._isConstCheckBox.AutoSize = true;
            this._isConstCheckBox.Location = new System.Drawing.Point(412, 43);
            this._isConstCheckBox.Name = "_isConstCheckBox";
            this._isConstCheckBox.Size = new System.Drawing.Size(67, 19);
            this._isConstCheckBox.TabIndex = 15;
            this._isConstCheckBox.Text = "只读?";
            this._isConstCheckBox.UseVisualStyleBackColor = true;
            this._isConstCheckBox.CheckedChanged += new System.EventHandler(this.OnFieldTextChanged);
            // 
            // _valueTypeBox
            // 
            this._valueTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._valueTypeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._valueTypeBox.ForeColor = System.Drawing.Color.LightGray;
            this._valueTypeBox.Location = new System.Drawing.Point(79, 71);
            this._valueTypeBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._valueTypeBox.Name = "_valueTypeBox";
            this._valueTypeBox.Size = new System.Drawing.Size(401, 32);
            this._valueTypeBox.TabIndex = 16;
            // 
            // _groupComboBox
            // 
            this._groupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._groupComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._groupComboBox.DisplayMember = "FullName";
            this._groupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._groupComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._groupComboBox.FormattingEnabled = true;
            this._groupComboBox.Location = new System.Drawing.Point(79, 111);
            this._groupComboBox.Name = "_groupComboBox";
            this._groupComboBox.Size = new System.Drawing.Size(396, 23);
            this._groupComboBox.Sorted = true;
            this._groupComboBox.TabIndex = 30;
            this._groupComboBox.SelectedIndexChanged += new System.EventHandler(this.OnFieldTextChanged);
            // 
            // FieldEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.Controls.Add(this._groupComboBox);
            this.Controls.Add(this._nameTextBox);
            this.Controls.Add(this._valueTypeBox);
            this.Controls.Add(this._desTextBox);
            this.Controls.Add(this._isConstCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._typeComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._defaultLabel);
            this.Controls.Add(this._checkerComboBox);
            this.ForeColor = System.Drawing.Color.LightGray;
            this.Name = "FieldEditor";
            this.Size = new System.Drawing.Size(492, 204);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.TextBox _desTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label _defaultLabel;
        private System.Windows.Forms.ComboBox _checkerComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox _typeComboBox;
        private System.Windows.Forms.CheckBox _isConstCheckBox;
        private TypeBox _valueTypeBox;
        private System.Windows.Forms.ComboBox _groupComboBox;
    }
}
