namespace Desc
{
    partial class TypeBox
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
            this._stringBox = new System.Windows.Forms.TextBox();
            this._comboBox = new System.Windows.Forms.ComboBox();
            this._boolBox = new System.Windows.Forms.CheckBox();
            this._numericUpDown = new System.Windows.Forms.NumericUpDown();
            this._keyComboBox = new System.Windows.Forms.ComboBox();
            this._valueComboBox = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._numericUpDown)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _stringBox
            // 
            this._stringBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._stringBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._stringBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._stringBox.ForeColor = System.Drawing.Color.LightGray;
            this._stringBox.Location = new System.Drawing.Point(2, 2);
            this._stringBox.Name = "_stringBox";
            this._stringBox.Size = new System.Drawing.Size(351, 25);
            this._stringBox.TabIndex = 12;
            this._stringBox.TextChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // _comboBox
            // 
            this._comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._comboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._comboBox.DisplayMember = "DisplayFullName";
            this._comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBox.ForeColor = System.Drawing.Color.LightGray;
            this._comboBox.FormattingEnabled = true;
            this._comboBox.Location = new System.Drawing.Point(2, 3);
            this._comboBox.Name = "_comboBox";
            this._comboBox.Size = new System.Drawing.Size(351, 23);
            this._comboBox.Sorted = true;
            this._comboBox.TabIndex = 13;
            this._comboBox.SelectedIndexChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // _boolBox
            // 
            this._boolBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._boolBox.AutoSize = true;
            this._boolBox.Location = new System.Drawing.Point(5, 6);
            this._boolBox.Name = "_boolBox";
            this._boolBox.Size = new System.Drawing.Size(18, 17);
            this._boolBox.TabIndex = 14;
            this._boolBox.UseVisualStyleBackColor = true;
            this._boolBox.CheckedChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // _numericUpDown
            // 
            this._numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._numericUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._numericUpDown.ForeColor = System.Drawing.Color.LightGray;
            this._numericUpDown.Location = new System.Drawing.Point(2, 2);
            this._numericUpDown.Name = "_numericUpDown";
            this._numericUpDown.Size = new System.Drawing.Size(351, 25);
            this._numericUpDown.TabIndex = 15;
            this._numericUpDown.ValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // _keyComboBox
            // 
            this._keyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._keyComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._keyComboBox.DisplayMember = "DisplayFullName";
            this._keyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._keyComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._keyComboBox.FormattingEnabled = true;
            this._keyComboBox.Location = new System.Drawing.Point(2, 3);
            this._keyComboBox.Name = "_keyComboBox";
            this._keyComboBox.Size = new System.Drawing.Size(155, 23);
            this._keyComboBox.Sorted = true;
            this._keyComboBox.TabIndex = 13;
            this._keyComboBox.SelectedIndexChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // _valueComboBox
            // 
            this._valueComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._valueComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._valueComboBox.DisplayMember = "DisplayFullName";
            this._valueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._valueComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._valueComboBox.FormattingEnabled = true;
            this._valueComboBox.Location = new System.Drawing.Point(157, 3);
            this._valueComboBox.Name = "_valueComboBox";
            this._valueComboBox.Size = new System.Drawing.Size(195, 23);
            this._valueComboBox.Sorted = true;
            this._valueComboBox.TabIndex = 13;
            this._valueComboBox.SelectedIndexChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._numericUpDown);
            this.panel1.Controls.Add(this._comboBox);
            this.panel1.Controls.Add(this._keyComboBox);
            this.panel1.Controls.Add(this._stringBox);
            this.panel1.Controls.Add(this._valueComboBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(356, 30);
            this.panel1.TabIndex = 16;
            // 
            // TypeBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.Controls.Add(this._boolBox);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.LightGray;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TypeBox";
            this.Size = new System.Drawing.Size(356, 30);
            ((System.ComponentModel.ISupportInitialize)(this._numericUpDown)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _stringBox;
        private System.Windows.Forms.ComboBox _comboBox;
        private System.Windows.Forms.CheckBox _boolBox;
        private System.Windows.Forms.NumericUpDown _numericUpDown;
        private System.Windows.Forms.ComboBox _keyComboBox;
        private System.Windows.Forms.ComboBox _valueComboBox;
        private System.Windows.Forms.Panel panel1;
    }
}
