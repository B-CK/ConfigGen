namespace Description.Editor
{
    partial class EnumItemEditor
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
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._desTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._aliasTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this._defaultValue = new System.Windows.Forms.NumericUpDown();
            this._groupComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this._defaultValue)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.LightGray;
            this.label2.Location = new System.Drawing.Point(9, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = "默认值:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.LightGray;
            this.label4.Location = new System.Drawing.Point(9, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 15);
            this.label4.TabIndex = 20;
            this.label4.Text = "别名:";
            // 
            // _desTextBox
            // 
            this._desTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._desTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._desTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._desTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._desTextBox.Location = new System.Drawing.Point(82, 144);
            this._desTextBox.Name = "_desTextBox";
            this._desTextBox.Size = new System.Drawing.Size(396, 25);
            this._desTextBox.TabIndex = 27;
            this._desTextBox.TextChanged += new System.EventHandler(this.OnItemTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.LightGray;
            this.label5.Location = new System.Drawing.Point(9, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 15);
            this.label5.TabIndex = 20;
            this.label5.Text = "描述:";
            // 
            // _aliasTextBox
            // 
            this._aliasTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._aliasTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._aliasTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._aliasTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._aliasTextBox.Location = new System.Drawing.Point(82, 111);
            this._aliasTextBox.Name = "_aliasTextBox";
            this._aliasTextBox.Size = new System.Drawing.Size(396, 25);
            this._aliasTextBox.TabIndex = 27;
            this._aliasTextBox.TextChanged += new System.EventHandler(this.OnItemAliasChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 23;
            this.label1.Text = "名称:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.LightGray;
            this.label6.Location = new System.Drawing.Point(9, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "分组:";
            // 
            // _nameTextBox
            // 
            this._nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._nameTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._nameTextBox.Location = new System.Drawing.Point(81, 6);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.Size = new System.Drawing.Size(396, 25);
            this._nameTextBox.TabIndex = 24;
            this._nameTextBox.TextChanged += new System.EventHandler(this.OnItemNameChanged);
            // 
            // _defaultValue
            // 
            this._defaultValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._defaultValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._defaultValue.ForeColor = System.Drawing.Color.LightGray;
            this._defaultValue.Location = new System.Drawing.Point(81, 42);
            this._defaultValue.Name = "_defaultValue";
            this._defaultValue.Size = new System.Drawing.Size(396, 25);
            this._defaultValue.TabIndex = 28;
            this._defaultValue.ValueChanged += new System.EventHandler(this.OnItemValueChanged);
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
            this._groupComboBox.Location = new System.Drawing.Point(81, 78);
            this._groupComboBox.Name = "_groupComboBox";
            this._groupComboBox.Size = new System.Drawing.Size(396, 23);
            this._groupComboBox.Sorted = true;
            this._groupComboBox.TabIndex = 29;
            this._groupComboBox.SelectedIndexChanged += new System.EventHandler(this.OnItemTextChanged);
            // 
            // EnumItemEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.Controls.Add(this._groupComboBox);
            this.Controls.Add(this._defaultValue);
            this.Controls.Add(this._nameTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._aliasTextBox);
            this.Controls.Add(this._desTextBox);
            this.Controls.Add(this.label5);
            this.ForeColor = System.Drawing.Color.LightGray;
            this.Name = "EnumItemEditor";
            this.Size = new System.Drawing.Size(492, 204);
            ((System.ComponentModel.ISupportInitialize)(this._defaultValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _desTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _aliasTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.NumericUpDown _defaultValue;
        private System.Windows.Forms.ComboBox _groupComboBox;
    }
}
